using UnityEngine;

namespace HL
{
    public enum ActionMaps
    {
        None,
        Menu,
        Player,
        Dialogue
    }

    public enum AIType
    {
        BasicMelee,
        BasicRanged,
        Heavy,
        FastGrounded,
        FastFlying,
        Boss
    }

    public enum BossAttackType
    {
        RangedLow,
        RangedHigh,
        Charge,
        Shockwave,
        MissileRain
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}