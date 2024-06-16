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
                AIType.Boss => this,
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

        #endregion

        private void AttackTarget(AIManager ai)
        {
            if (ai.currentAttack.isRangedAction)
            {
                ai.aiAnimatorManager.PlayTargetAnimation(ai.currentAttack.attackAnimationName, rangedWeapon.stopMovement);
                ai.currentRecoveryTime = ai.currentAttack.recoveryTime;
            }
            else
            {
                ai.aiAnimatorManager.PlayTargetAnimation(ai.currentAttack.attackAnimationName, ai.aiStatsManager.currentMeleeWeapon.stopMovement);
                ai.currentRecoveryTime = ai.currentAttack.recoveryTime;
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
    }
}