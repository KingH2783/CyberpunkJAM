using UnityEngine;

namespace HL
{
    [CreateAssetMenu(menuName = "A.I Attack Action")]
    public class AIAttackAction : ScriptableObject
    {
        [Header("Action Settings")]
        public bool isBoss;
        public string attackAnimationName;
        public int attackScore = 3;
        public float recoveryTime = 2;
        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 2;
        public bool isRangedAction;
        public bool actionCanCombo;
        public AIAttackAction comboAttack;
        public BossAttackType bossAttackType;
        public float chargeVelocity;
        public float chargeTime;
        
    }
}