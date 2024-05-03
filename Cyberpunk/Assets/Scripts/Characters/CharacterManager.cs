using UnityEngine;

namespace HL
{
    public class CharacterManager : MonoBehaviour
    {
        [HideInInspector] public Transform _transform { get; private set; }
        [HideInInspector] public CharacterStatsManager characterStatsManager { get; private set; }
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager { get; private set; }

        [Header("Character Flags")]
        public bool isDead;
        public bool isInvulnerable;

        protected virtual void Awake()
        {
            _transform = transform;
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }
    }
}