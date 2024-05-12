using UnityEngine;
using UnityEditor;
using HL;

[CustomEditor(typeof(AIManager))]
public class AIManagerEditor : Editor
{
    private AIManager ai;

    private SerializedProperty currentState;
    private SerializedProperty currentTarget;
    private SerializedProperty currentAttack;
    private SerializedProperty aiType;
    private SerializedProperty detectionLayer;
    private SerializedProperty layersThatBlockLineOfSight;
    private SerializedProperty aiAttacks;

    private void OnEnable()
    {
        ai = (AIManager)target;

        currentState = serializedObject.FindProperty("currentState");
        currentTarget = serializedObject.FindProperty("currentTarget");
        currentAttack = serializedObject.FindProperty("currentAttack");
        aiType = serializedObject.FindProperty("aiType");
        detectionLayer = serializedObject.FindProperty("detectionLayer");
        layersThatBlockLineOfSight = serializedObject.FindProperty("layersThatBlockLineOfSight");
        aiAttacks = serializedObject.FindProperty("aiAttacks");
    }

    public override void OnInspectorGUI()
    {
        // ======= BEGINNING =======
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();
        EditorStyles.label.fontStyle = FontStyle.Normal;

        // ======= SCRIPTS =======
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.ObjectField("Editor", MonoScript.FromScriptableObject(this), GetType(), false);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);
        }

        // ======= SERIALIZED =======
        serializedObject.Update();

        // ======= FLAGS =======
        #region // ======= FLAGS =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        ai.showFlags = EditorGUILayout.Toggle("SHOW FLAGS", ai.showFlags);
        if (ai.showFlags)
        {
            EditorStyles.label.fontStyle = FontStyle.Normal;
            ai.isRunning = EditorGUILayout.Toggle("Is Running", ai.isRunning);
            ai.isGrounded = EditorGUILayout.Toggle("Is Grounded", ai.isGrounded);
            ai.isJumping = EditorGUILayout.Toggle("Is Jumping", ai.isJumping);
            //ai.isDashing = EditorGUILayout.Toggle("Is Dashing", ai.isDashing);
            ai.isOnSlope = EditorGUILayout.Toggle("Is On Slope", ai.isOnSlope);
            ai.isDead = EditorGUILayout.Toggle("Is Dead", ai.isDead);
            ai.isInvulnerable = EditorGUILayout.Toggle("Is Invulnerable", ai.isInvulnerable);
            ai.isPerformingAction = EditorGUILayout.Toggle("Is Performing Action", ai.isPerformingAction);
            ai.canDoCombo = EditorGUILayout.Toggle("Can Do Combo", ai.canDoCombo);
        }
        #endregion

        // ======= Current A.I State =======
        #region // ======= Current A.I State =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        ai.showCurrentAIState = EditorGUILayout.Toggle("SHOW CURRENT STATE", ai.showCurrentAIState);
        if (ai.showCurrentAIState)
        {
            EditorStyles.label.fontStyle = FontStyle.Normal;
            EditorGUILayout.PropertyField(currentState);
            EditorGUILayout.PropertyField(currentTarget);
            EditorGUILayout.PropertyField(currentAttack);
            ai.currentRecoveryTime = EditorGUILayout.FloatField("Current Recovery Time", ai.currentRecoveryTime);
        }
        #endregion

        // ======= A.I Settings =======
        #region // ======= A.I Settings =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        ai.showAISettings = EditorGUILayout.Toggle("SHOW A.I SETTINGS", ai.showAISettings);
        if (ai.showAISettings)
        {
            EditorGUILayout.PropertyField(aiAttacks, new GUIContent("AI Attack List"));
            EditorGUILayout.PropertyField(aiType, new GUIContent("AI TYPE"));

            if (ai.aiType == AIType.BasicMelee)
            {
                // ======= Layers and Floats =======
                EditorGUILayout.Space();
                EditorStyles.label.fontStyle = FontStyle.Normal;
                EditorGUILayout.PropertyField(detectionLayer);
                EditorGUILayout.PropertyField(layersThatBlockLineOfSight);
                ai.detectionRadius = EditorGUILayout.FloatField("Detection Radius", ai.detectionRadius);
                ai.maxAggroRange = EditorGUILayout.FloatField("Max Aggro Range", ai.maxAggroRange);
                ai.maxCirclingDistance = EditorGUILayout.FloatField("Max Circling Distance", ai.maxCirclingDistance);
                ai.stoppingDistance = EditorGUILayout.FloatField("Stopping Distance", ai.stoppingDistance);

                // ======= Combos =======
                EditorGUILayout.Space();
                EditorStyles.label.fontStyle = FontStyle.Bold;
                ai.allowAIToPerformCombos = EditorGUILayout.Toggle("Allow A.I To Perform Combos?", ai.allowAIToPerformCombos);
                if (ai.allowAIToPerformCombos)
                {
                    EditorStyles.label.fontStyle = FontStyle.Normal;
                    ai.comboLikelyHood = EditorGUILayout.IntSlider("Combo Chance (Percentage)", ai.comboLikelyHood, 0, 100);
                }
            }
            else if (ai.aiType == AIType.BasicRanged)
            {
                // ======= Layers and Floats =======
                EditorGUILayout.Space();
                EditorStyles.label.fontStyle = FontStyle.Normal;
                EditorGUILayout.PropertyField(detectionLayer);
                EditorGUILayout.PropertyField(layersThatBlockLineOfSight);
                ai.detectionRadius = EditorGUILayout.FloatField("Detection Radius", ai.detectionRadius);
                ai.maxAggroRange = EditorGUILayout.FloatField("Max Aggro Range", ai.maxAggroRange);
                ai.maxCirclingDistance = EditorGUILayout.FloatField("Max Circling Distance", ai.maxCirclingDistance);
                ai.stoppingDistance = EditorGUILayout.FloatField("Stopping Distance", ai.stoppingDistance);
            }
        }
        #endregion

        // ======= TRANSFORMS =======
        #region // ======= TRANSFORMS =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        ai.showTransforms = EditorGUILayout.Toggle("SHOW TRANSFORMS", ai.showTransforms);
        if (ai.showTransforms)
        {
            EditorStyles.label.fontStyle = FontStyle.Normal;
            ai.lineOfSightTransform = (Transform)EditorGUILayout.ObjectField("Line Of Sight Transform", ai.lineOfSightTransform, typeof(Transform), true);

            if (ai.aiType == AIType.BasicRanged)
                ai.bulletSpawnPoint = (Transform)EditorGUILayout.ObjectField("Bullet Spawn Point", ai.bulletSpawnPoint, typeof(Transform), true);
        }
        #endregion

        // ======= END =======
        EditorStyles.label.fontStyle = FontStyle.Normal;
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(target);
    }
}
