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

    public enum CutscenesEnum
    {
        None,
        FindingTheOpenDoor,
        CheckingPC,
        GuardOpensDoor,
        AlarmGoesOff,
        RunningWithAarm,
        HopeOutOfWindow,
        HopeFallsDown,
        MikeEnters,
        MikeCarriesHope,
        HopeOnMedicalChair,
        HopeEyesClosed,
        HopeEyesOpen,
        MikeGivesGlasses,
        Malfunction,
        MJSavingHope,
        MJShootingBack,
        MJandHopeRunning,
        BossChoking,
        MJEntering,
        HopeOnGround,
        MJHoldingHopesHand,
        Ending
    }
}