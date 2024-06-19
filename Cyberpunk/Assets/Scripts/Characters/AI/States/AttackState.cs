using System.Collections;
using UnityEngine;

namespace HL
{
    public class AttackState : State
    {
        private bool willDoComboOnNextAttack = false;
        private RangedWeapon rangedWeapon;
        private int roundsLeftInClip;

        public override State Tick(AIManager ai)
        {
            if (ai.isDead)
            {
                ResetStateFlags(ai);
                return deathState;
            }

            if (ai.currentTarget.isDead)
            {
                ResetStateFlags(ai);
                return idleState;
            }

            return ai.aiType switch
            {
                AIType.BasicMelee => ProcessBasicMeleeAttack(ai),
                AIType.BasicRanged => ProcessBasicRangedAttack(ai),
                AIType.Heavy => this,
                AIType.FastGrounded => this,
                AIType.FastFlying => this,
                AIType.Boss => ProcessBossAttack(ai),
                _ => this,
            };
        }

        #region AI Types

        private State ProcessBasicMeleeAttack(AIManager ai)
        {
            // Target dies
            if (ai.currentTarget.isDead)
            {
                ResetStateFlags(ai);
                ai.currentTarget = null;
                return idleState;
            }

            // Target is too far, get closer
            if (ai.distanceFromTarget > ai.maxCirclingDistance)
            {
                ResetStateFlags(ai);
                return pursueTargetState;
            }

            ai.aiLocomotion.StopAIMovement();

            // If we're gonna combo next, assign when ready
            if (!ai.isPerformingAction && willDoComboOnNextAttack)
            {
                // Assign combo attack to current attack
                ai.currentAttack = ai.currentAttack.comboAttack;
            }

            // Attack and figure out if we can combo
            if (!ai.isPerformingAction && ai.currentAttack != null)
            {
                AttackTarget(ai);
                RollForComboChance(ai);
            }

            // We check if we still have an attack -->
            // If we do that means we can combo so go back to the top of this state
            if (ai.currentAttack == null)
                return combatStanceState;
            else
                return this;
        }

        private State ProcessBasicRangedAttack(AIManager ai)
        {
            rangedWeapon = ai.aiStatsManager.currentRangedWeapon;

            // Target dies
            if (ai.currentTarget.isDead)
            {
                ResetStateFlags(ai);
                ai.currentTarget = null;
                return idleState;
            }

            // Target is too far, get closer
            if (ai.distanceFromTarget > ai.maxCirclingDistance)
            {
                ResetStateFlags(ai);
                return pursueTargetState;
            }

            ai.aiLocomotion.StopAIMovement();

            // Attack
            if (!ai.isPerformingAction && 
                ai.currentAttack != null &&
                ai.currentRecoveryTime <= 0)
            {
                if (roundsLeftInClip == 0)
                {
                    ResetStateFlags(ai);
                    return combatStanceState;
                }
                else
                    AttackTarget(ai);
            }

            // We check if we still have an attack -->
            // If we do that means we can combo so go back to the top of this state
            if (ai.currentAttack == null)
                return combatStanceState;
            else
                return this;
        }

        private State ProcessBossAttack(AIManager ai)
        {
            rangedWeapon = ai.aiStatsManager.currentRangedWeapon;

            // Target dies
            if (ai.currentTarget.isDead)
            {
                ResetStateFlags(ai);
                ai.currentTarget = null;
                return idleState;
            }

            // Target is too far, get closer
            if (ai.distanceFromTarget > ai.maxCirclingDistance)
            {
                ResetStateFlags(ai);
                return pursueTargetState;
            }

            if (!ai.isDashing)
                ai.aiLocomotion.StopAIMovement();

            // Attack and figure out if we can combo
            if (!ai.isPerformingAction && ai.currentAttack != null)
            {
                AttackTarget(ai);
                ai.currentAttack = null;
            }

            // We check if we still have an attack -->
            // If we do that means we can combo so go back to the top of this state
            if (ai.currentAttack == null)
                return combatStanceState;
            else
                return this;
        }

        #endregion

        private void AttackTarget(AIManager ai)
        {
            ai.currentRecoveryTime = ai.currentAttack.recoveryTime;

            if (ai.aiType != AIType.Boss)
            {
                if (ai.currentAttack.isRangedAction)
                    ai.aiAnimatorManager.PlayTargetAnimation(ai.currentAttack.attackAnimationName, rangedWeapon.stopMovement);
                else
                    ai.aiAnimatorManager.PlayTargetAnimation(ai.currentAttack.attackAnimationName, ai.aiStatsManager.currentMeleeWeapon.stopMovement);
            }
            else
            {
                ai.aiAnimatorManager.PlayTargetAnimation(ai.currentAttack.attackAnimationName, true);
                switch (ai.currentAttack.bossAttackType)
                {
                    case BossAttackType.RangedLow:
                        RangedLowAttack(ai);
                        break;
                    case BossAttackType.RangedHigh:
                        RangedHighAttack(ai);
                        break;
                    case BossAttackType.Charge:
                        ChargeAttack(ai);
                        break;
                    case BossAttackType.Shockwave:
                        ShockwaveAttack(ai);
                        break;
                    case BossAttackType.MissileRain:
                        MissileRainAttack(ai);
                        break;
                    default:
                        break;
                }
            }
        }

        public IEnumerator ShootTarget(AIManager ai)
        {
            for (int i = rangedWeapon.ammoCapacity; i > 0; i--)
            {
                yield return new WaitForSeconds(rangedWeapon.fireRate);

                // Shoot
                GameObject bulletGameObject = Instantiate(rangedWeapon.bulletType, ai.bulletSpawnPoint.position, ai.bulletSpawnPoint.rotation);
                Bullet bullet = bulletGameObject.GetComponent<Bullet>();
                bullet.characterWhoFiredMe = ai;
                bullet.weapon = rangedWeapon;

                ai.currentRecoveryTime = rangedWeapon.fireRate;
                roundsLeftInClip -= 1;
            }
        }

        private void RollForComboChance(AIManager ai)
        {
            float comboChance = Random.Range(0, 100);

            if (ai.allowAIToPerformCombos &&
                ai.currentAttack.actionCanCombo &&
                comboChance <= ai.comboLikelyHood)
            {
                willDoComboOnNextAttack = true;
            }
            else
            {
                willDoComboOnNextAttack = false;
                ai.currentAttack = null;
            }
        }

        private IEnumerator Reload(AIManager ai)
        {
            if (roundsLeftInClip == rangedWeapon.ammoCapacity)
                yield return null;

            ai.currentRecoveryTime = rangedWeapon.reloadTime;

            //player.playerAnimatorManager.PlayTargetAnimation("Reload");

            yield return new WaitForSeconds(rangedWeapon.reloadTime);

            roundsLeftInClip = rangedWeapon.ammoCapacity;
        }

        private void ResetStateFlags(AIManager ai)
        {
            if (rangedWeapon != null)
                StartCoroutine(Reload(ai));

            willDoComboOnNextAttack = false;
            ai.currentAttack = null;
        }

        #region Boss Attacks

        private void RangedLowAttack(AIManager ai)
        {
            // Shoot
            GameObject bulletGameObject = Instantiate(rangedWeapon.bulletType, ai.bulletSpawnPointLow.position, ai.bulletSpawnPointLow.rotation);
            Bullet bullet = bulletGameObject.GetComponent<Bullet>();
            bullet.characterWhoFiredMe = ai;
            bullet.weapon = rangedWeapon;
        }

        private void RangedHighAttack(AIManager ai)
        {
            // Shoot
            GameObject bulletGameObject = Instantiate(rangedWeapon.bulletType, ai.bulletSpawnPointHigh.position, ai.bulletSpawnPointHigh.rotation);
            Bullet bullet = bulletGameObject.GetComponent<Bullet>();
            bullet.characterWhoFiredMe = ai;
            bullet.weapon = rangedWeapon;
        }

        private void ChargeAttack(AIManager ai)
        {
            ai.isInvulnerable = true;
            ai.isDashing = true;
            
            Vector2 chargeDirection = ai.isFacingRight ? new(1, 0) : new(-1, 0);
            ai.rb.velocity = chargeDirection.normalized * ai.currentAttack.chargeVelocity;

            StartCoroutine(StopChargeAttack(ai));
        }

        private IEnumerator StopChargeAttack(AIManager ai)
        {
            yield return new WaitForSeconds(ai.currentAttack.chargeTime);
            ai.isInvulnerable = false;
            ai.isDashing = false;
            ai.rb.velocity = Vector2.zero;
        }

        private void ShockwaveAttack(AIManager ai)
        {
            
        }

        private void MissileRainAttack(AIManager ai)
        {
            MissileSpawner missileSpawner = FindObjectOfType<MissileSpawner>();
            StartCoroutine(missileSpawner.SpawnMissiles(ai));
        }

        #endregion
    }
}