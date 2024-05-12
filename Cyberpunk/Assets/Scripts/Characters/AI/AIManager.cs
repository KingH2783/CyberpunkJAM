using UnityEngine;

namespace HL
{
    public class AIManager : CharacterManager
    {
        // ======= Script Caches =======
        [HideInInspector] public Rigidbody2D rb { get; private set; }
        [HideInInspector] public AILocomotion aiLocomotion { get; private set; }
        [HideInInspector] public AIStatsManager aiStatsManager { get; private set; }
        [HideInInspector] public AIAnimatorManager aiAnimatorManager { get; private set; }

        public bool showTransforms;
        public bool showFlags;
        public bool showCurrentAIState;
        public bool showAISettings;

        // ======= Current State =======
        //[Header("Current State")]
        public State currentState;
        public CharacterManager currentTarget;
        public AIAttackAction currentAttack;
        [HideInInspector] public float currentRecoveryTime = 0;

        // ======= A.I Attacks =======
        //[Header("")]
        public AIAttackAction[] aiAttacks;

        // ======= A.I Settings =======
        //[Header("A.I Settings")]
        public AIType aiType;
        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;
        public float detectionRadius = 8;
        public float maxCirclingDistance = 5f;
        public float maxAggroRange = 16f;
        public float stoppingDistance = 1.8f;
        public float waitTimeBeforeFirstAttack = 1f;
        public bool allowAIToPerformCombos;
        [Range(0, 100)] public int comboLikelyHood = 50;
        //public bool allowAIToPerformDodge;
        //[Range(0, 100)] public int dodgeLikelyHood = 50;
        public Transform bulletSpawnPoint;

        // ======= A.I Target Info =======
        [HideInInspector] public float distanceFromCompanion;
        [HideInInspector] public float distanceFromTarget;

        protected override void Awake()
        {
            base.Awake();

            rb = GetComponent<Rigidbody2D>();
            aiLocomotion = GetComponent<AILocomotion>();
            aiStatsManager = GetComponent<AIStatsManager>();
            aiAnimatorManager = GetComponent<AIAnimatorManager>();
        }

        protected override void Update()
        {
            base.Update();
            HandleStateMachine(deltaUpdate);
            aiAnimatorManager.SetAnimatorParams();
        }

        private void HandleStateMachine(float delta)
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this);

                if (nextState != null)
                    currentState = nextState;
            }

            if (currentTarget != null)
                distanceFromTarget = Vector3.Distance(currentTarget._transform.position, _transform.position);

            currentRecoveryTime = (currentRecoveryTime > 0) ? currentRecoveryTime - delta : 0;
        }
    }
}