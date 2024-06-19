using UnityEngine;
using UnityEditor;
using HL;

[CustomEditor(typeof(AIAttackAction))]
public class AIAttackActionEditor : Editor
{
    private AIAttackAction attack;

    private SerializedProperty comboAttack;
    private SerializedProperty bossAttackType;

    private void OnEnable()
    {
        attack = (AIAttackAction)target;

        comboAttack = serializedObject.FindProperty("comboAttack");
        bossAttackType = serializedObject.FindProperty("bossAttackType");
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
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), GetType(), false);
        }

        // ======= SERIALIZED =======
        serializedObject.Update();

        // ======= AI Attack Action Variables =======
        #region // ======= AI Attack Action Variables =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        attack.isBoss = EditorGUILayout.Toggle("Is this a Boss' Attack?", attack.isBoss);
        if (attack.isBoss)
        {
            EditorGUILayout.PropertyField(bossAttackType);

            EditorGUILayout.Space();
            EditorStyles.label.fontStyle = FontStyle.Normal;
            attack.attackAnimationName = EditorGUILayout.TextField("Attack Animation Name", attack.attackAnimationName);
            attack.attackScore = EditorGUILayout.IntField("Attack Score", attack.attackScore);
            attack.recoveryTime = EditorGUILayout.FloatField("Recovery Time", attack.recoveryTime);
            attack.minimumDistanceNeededToAttack = EditorGUILayout.FloatField("Minimum Distance Needed To Attack", attack.minimumDistanceNeededToAttack);
            attack.maximumDistanceNeededToAttack = EditorGUILayout.FloatField("Maximum Distance Needed To Attack", attack.maximumDistanceNeededToAttack);
            EditorGUILayout.Space();

            if (attack.bossAttackType == BossAttackType.RangedLow)
            {

            }
            else if (attack.bossAttackType == BossAttackType.RangedHigh)
            {

            }
            else if (attack.bossAttackType == BossAttackType.Charge)
            {
                attack.chargeVelocity = EditorGUILayout.FloatField("Charge Velocity", attack.chargeVelocity);
                attack.chargeTime = EditorGUILayout.FloatField("Charge Time", attack.chargeTime);
            }
            else if (attack.bossAttackType == BossAttackType.Shockwave)
            {

            }
            else if (attack.bossAttackType == BossAttackType.MissileRain)
            {

            }
        }
        else
        {
            attack.isRangedAction = EditorGUILayout.Toggle("Is this a Ranged Attack?", attack.isRangedAction);
            if (attack.isRangedAction)
            {
                EditorStyles.label.fontStyle = FontStyle.Normal;
                attack.attackAnimationName = EditorGUILayout.TextField("Attack Animation Name", attack.attackAnimationName);
                attack.attackScore = EditorGUILayout.IntField("Attack Score", attack.attackScore);
                attack.recoveryTime = EditorGUILayout.FloatField("Recovery Time", attack.recoveryTime);
                attack.minimumDistanceNeededToAttack = EditorGUILayout.FloatField("Minimum Distance Needed To Attack", attack.minimumDistanceNeededToAttack);
                attack.maximumDistanceNeededToAttack = EditorGUILayout.FloatField("Maximum Distance Needed To Attack", attack.maximumDistanceNeededToAttack);
                attack.actionCanCombo = false;
            }
            else // Melee
            {
                EditorStyles.label.fontStyle = FontStyle.Normal;
                attack.attackAnimationName = EditorGUILayout.TextField("Attack Animation Name", attack.attackAnimationName);
                attack.attackScore = EditorGUILayout.IntField("Attack Score", attack.attackScore);
                attack.recoveryTime = EditorGUILayout.FloatField("Recovery Time", attack.recoveryTime);
                attack.minimumDistanceNeededToAttack = EditorGUILayout.FloatField("Minimum Distance Needed To Attack", attack.minimumDistanceNeededToAttack);
                attack.maximumDistanceNeededToAttack = EditorGUILayout.FloatField("Maximum Distance Needed To Attack", attack.maximumDistanceNeededToAttack);
                attack.actionCanCombo = EditorGUILayout.Toggle("Can we Combo into another Attack?", attack.actionCanCombo);
                if (attack.actionCanCombo)
                    EditorGUILayout.PropertyField(comboAttack);
            }
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
