using UnityEngine;
using UnityEditor;
using HL;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    #region Defining Variables
    private SerializedProperty showRunSettings;
    private SerializedProperty showJumpSettings;
    private SerializedProperty showDashSettings;
    private SerializedProperty showFallSettings;
    private SerializedProperty showCheckSettings;
    private SerializedProperty showAssistSettings;

    private SerializedProperty maxRunSpeed;
    private SerializedProperty maxSlopeRunSpeed;
    private SerializedProperty maxSlopeAngle;
    private SerializedProperty runAcceleration;
    private SerializedProperty runDecceleration;
    private SerializedProperty accelInAir;
    private SerializedProperty deccelInAir;
    private SerializedProperty doConserveMomentum;

    private SerializedProperty jumpHeight;
    private SerializedProperty allowDoubleJump;
    private SerializedProperty jumpTimeToApex;
    private SerializedProperty jumpCutGravityMult;
    private SerializedProperty timeToHoldJumpForFullJump;
    private SerializedProperty jumpHangGravityMult;
    private SerializedProperty jumpHangTimeThreshold;
    private SerializedProperty jumpHangAccelerationMult;
    private SerializedProperty jumpHangMaxSpeedMult;

    private SerializedProperty allowWallJump;
    private SerializedProperty wallJumpForce;
    private SerializedProperty wallJumpRunLerp;
    private SerializedProperty wallJumpCooldown;
    private SerializedProperty doFlipOnWallJump;

    private SerializedProperty allowDash;
    private SerializedProperty dashVelocity;
    private SerializedProperty dashTime;
    private SerializedProperty dashCooldown;
    private SerializedProperty allowMultipleDashesBeforeTouchingGround;

    private SerializedProperty slideSpeed;
    private SerializedProperty slideAccel;

    private SerializedProperty maxFallSpeed;
    private SerializedProperty fallGravityMult;
    private SerializedProperty fallGravityMultOnSteepSlope;

    private SerializedProperty groundCheckPoint;
    private SerializedProperty groundCheckSize;
    private SerializedProperty groundLayer;
    private SerializedProperty frontWallCheckPoint;
    private SerializedProperty backWallCheckPoint;
    private SerializedProperty wallCheckSize;
    private SerializedProperty slopeCheckStartOffset;
    private SerializedProperty slopeCheckDistance;

    private SerializedProperty coyoteTime;
    private SerializedProperty jumpInputBufferTime;
    private SerializedProperty dashInputBufferTime;
    #endregion

    private void OnEnable()
    {
        #region Defining Variables
        showRunSettings = serializedObject.FindProperty("showRunSettings");
        showJumpSettings = serializedObject.FindProperty("showJumpSettings");
        showDashSettings = serializedObject.FindProperty("showDashSettings");
        showFallSettings = serializedObject.FindProperty("showFallSettings");
        showCheckSettings = serializedObject.FindProperty("showCheckSettings");
        showAssistSettings = serializedObject.FindProperty("showAssistSettings");

        maxRunSpeed = serializedObject.FindProperty("maxRunSpeed");
        maxSlopeRunSpeed = serializedObject.FindProperty("maxSlopeRunSpeed");
        maxSlopeAngle = serializedObject.FindProperty("maxSlopeAngle");
        runAcceleration = serializedObject.FindProperty("runAcceleration");
        runDecceleration = serializedObject.FindProperty("runDecceleration");
        accelInAir = serializedObject.FindProperty("accelInAir");
        deccelInAir = serializedObject.FindProperty("deccelInAir");
        doConserveMomentum = serializedObject.FindProperty("doConserveMomentum");

        jumpHeight = serializedObject.FindProperty("jumpHeight");
        jumpTimeToApex = serializedObject.FindProperty("jumpTimeToApex");
        jumpCutGravityMult = serializedObject.FindProperty("jumpCutGravityMult");
        timeToHoldJumpForFullJump = serializedObject.FindProperty("timeToHoldJumpForFullJump");
        jumpHangGravityMult = serializedObject.FindProperty("jumpHangGravityMult");
        jumpHangTimeThreshold = serializedObject.FindProperty("jumpHangTimeThreshold");
        jumpHangAccelerationMult = serializedObject.FindProperty("jumpHangAccelerationMult");
        jumpHangMaxSpeedMult = serializedObject.FindProperty("jumpHangMaxSpeedMult");
        allowDoubleJump = serializedObject.FindProperty("allowDoubleJump");

        allowWallJump = serializedObject.FindProperty("allowWallJump");
        wallJumpForce = serializedObject.FindProperty("wallJumpForce");
        wallJumpRunLerp = serializedObject.FindProperty("wallJumpRunLerp");
        wallJumpCooldown = serializedObject.FindProperty("wallJumpCooldown");
        doFlipOnWallJump = serializedObject.FindProperty("doFlipOnWallJump");

        allowDash = serializedObject.FindProperty("allowDash");
        dashVelocity = serializedObject.FindProperty("dashVelocity");
        dashTime = serializedObject.FindProperty("dashTime");
        dashCooldown = serializedObject.FindProperty("dashCooldown");
        allowMultipleDashesBeforeTouchingGround = serializedObject.FindProperty("allowMultipleDashesBeforeTouchingGround");

        slideSpeed = serializedObject.FindProperty("slideSpeed");
        slideAccel = serializedObject.FindProperty("slideAccel");

        maxFallSpeed = serializedObject.FindProperty("maxFallSpeed");
        fallGravityMult = serializedObject.FindProperty("fallGravityMult");
        fallGravityMultOnSteepSlope = serializedObject.FindProperty("fallGravityMultOnSteepSlope");

        groundCheckPoint = serializedObject.FindProperty("groundCheckPoint");
        groundCheckSize = serializedObject.FindProperty("groundCheckSize");
        groundLayer = serializedObject.FindProperty("groundLayer");
        frontWallCheckPoint = serializedObject.FindProperty("frontWallCheckPoint");
        backWallCheckPoint = serializedObject.FindProperty("backWallCheckPoint");
        wallCheckSize = serializedObject.FindProperty("wallCheckSize");
        slopeCheckStartOffset = serializedObject.FindProperty("slopeCheckStartOffset");
        slopeCheckDistance = serializedObject.FindProperty("slopeCheckDistance");

        coyoteTime = serializedObject.FindProperty("coyoteTime");
        jumpInputBufferTime = serializedObject.FindProperty("jumpInputBufferTime");
        dashInputBufferTime = serializedObject.FindProperty("dashInputBufferTime");
        #endregion
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

        // ======= Run Settings =======
        #region // ======= Run Settings =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        showRunSettings.boolValue = EditorGUILayout.Toggle("SHOW RUNNING SETTINGS", showRunSettings.boolValue);
        if (showRunSettings.boolValue)
        {
            EditorStyles.label.fontStyle = FontStyle.Normal;
            maxRunSpeed.floatValue = EditorGUILayout.FloatField(new GUIContent("Max Run Speed", "The max running speed"), maxRunSpeed.floatValue);
            maxSlopeRunSpeed.floatValue = EditorGUILayout.FloatField(new GUIContent("Max Slope Run Speed", "The max running speed on a slope"), maxSlopeRunSpeed.floatValue);
            maxSlopeAngle.floatValue = EditorGUILayout.FloatField(new GUIContent("Max Slope Angle", "The max slope angle our character can climb"), maxSlopeAngle.floatValue);
            runAcceleration.floatValue = EditorGUILayout.FloatField(new GUIContent("Run Acceleration", "How fast we can reach our max speed"), runAcceleration.floatValue);
            runDecceleration.floatValue = EditorGUILayout.FloatField(new GUIContent("Run Decceleration", "How fast we can stop"), runDecceleration.floatValue);
            accelInAir.floatValue = EditorGUILayout.Slider(new GUIContent("Run Acceleration In Air", "How fast we can reach our max speed in the air"), accelInAir.floatValue, 0f, 1f);
            deccelInAir.floatValue = EditorGUILayout.Slider(new GUIContent("Run Decceleration In Air", "How fast we can stop in the air"), deccelInAir.floatValue, 0f, 1f);
            doConserveMomentum.boolValue = EditorGUILayout.Toggle(new GUIContent("Conserve Momentum?", "If we exceed our max speed, should we let it happen until we slow down naturally?"), doConserveMomentum.boolValue);
        }
        #endregion

        // ======= Jump Settings =======
        #region // ======= Jump Settings =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        showJumpSettings.boolValue = EditorGUILayout.Toggle("SHOW JUMP SETTINGS", showJumpSettings.boolValue);
        if (showJumpSettings.boolValue)
        {
            EditorStyles.label.fontStyle = FontStyle.Normal;
            jumpHeight.floatValue = EditorGUILayout.FloatField(new GUIContent("Jump Height", "The height of the jump"), jumpHeight.floatValue);
            jumpTimeToApex.floatValue = EditorGUILayout.FloatField(new GUIContent("Jump Time To Apex", "The time it takes to reach the highest point of the jump (the apex)"), jumpTimeToApex.floatValue);
            jumpCutGravityMult.floatValue = EditorGUILayout.FloatField(new GUIContent("Jump Cut Gravity Multiplier", "The gravity multiplier when we let go of the jump button before the apex"), jumpCutGravityMult.floatValue);
            timeToHoldJumpForFullJump.floatValue = EditorGUILayout.FloatField(new GUIContent("Time To Hold Jump For Full Jump", "How long should the play hold the jump button to reach the apex of their jump? (Instead of cutting it short by letting go early)"), timeToHoldJumpForFullJump.floatValue);
            jumpHangGravityMult.floatValue = EditorGUILayout.Slider(new GUIContent("Jump Apex Gravity Multiplier", "The gravity multiplier when we are at the apex"), jumpHangGravityMult.floatValue, 0, 1);
            jumpHangTimeThreshold.floatValue = EditorGUILayout.FloatField(new GUIContent("Jump Apex Time Threshold", "How long we stay at the apex"), jumpHangTimeThreshold.floatValue);
            jumpHangAccelerationMult.floatValue = EditorGUILayout.FloatField(new GUIContent("Jump Apex Acceleration Multiplier", "The max speed we can go at the apex"), jumpHangAccelerationMult.floatValue);
            jumpHangMaxSpeedMult.floatValue = EditorGUILayout.FloatField(new GUIContent("Jump Apex Max Speed", "The max speed we can go at the apex"), jumpHangMaxSpeedMult.floatValue);
            allowDoubleJump.boolValue = EditorGUILayout.Toggle(new GUIContent("Allow Double Jump?", "Should the player be allowed to double jump?"), allowDoubleJump.boolValue);

            // Wall Jump
            allowWallJump.boolValue = EditorGUILayout.Toggle(new GUIContent("Allow Wall Jump?", "Should the player be allowed to wall jump?"), allowWallJump.boolValue);
            if (allowWallJump.boolValue)
            {
                EditorGUILayout.Space();
                wallJumpForce.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Wall Jump Force", "The force of our wall jump, both horizontally and vertically"), wallJumpForce.vector2Value);
                wallJumpRunLerp.floatValue = EditorGUILayout.Slider(new GUIContent("Wall Jump Move Control", "How much control the player has over their movement after a wall jump"), wallJumpRunLerp.floatValue, 0, 1);
                wallJumpCooldown.floatValue = EditorGUILayout.Slider(new GUIContent("Wall Jump Cooldown", "How long it takes before we can wall jump again"), wallJumpCooldown.floatValue, 0.1f, 1.5f);
                doFlipOnWallJump.boolValue = EditorGUILayout.Toggle(new GUIContent("Flip Character After Wall Jump?", "Should the character sprite flip after a wall jump?"), doFlipOnWallJump.boolValue);
                slideSpeed.floatValue = EditorGUILayout.FloatField(new GUIContent("Max Wall Slide Speed", "The max speed we slide down a wall"), slideSpeed.floatValue);
                slideAccel.floatValue = EditorGUILayout.FloatField(new GUIContent("Wall Slide Acceleration", "How fast we reach the max slide speed"), slideAccel.floatValue);
            }
        }
        #endregion

        // ======= Dash Settings =======
        #region // ======= Dash Settings =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        showDashSettings.boolValue = EditorGUILayout.Toggle("SHOW DASH SETTINGS", showDashSettings.boolValue);
        if (showDashSettings.boolValue)
        {
            EditorStyles.label.fontStyle = FontStyle.Normal;
            allowDash.boolValue = EditorGUILayout.Toggle(new GUIContent("Allow Dash?", "Should the player be allowed to dash?"), allowDash.boolValue);
            dashVelocity.floatValue = EditorGUILayout.FloatField(new GUIContent("Dash Velocity", "The distance of our dash"), dashVelocity.floatValue);
            dashTime.floatValue = EditorGUILayout.FloatField(new GUIContent("Dash Time", "How long does our dash last?"), dashTime.floatValue);
            dashCooldown.floatValue = EditorGUILayout.FloatField(new GUIContent("Dash Cooldown", "How long do we have to wait before we can dash again?"), dashCooldown.floatValue);
            allowMultipleDashesBeforeTouchingGround.boolValue = EditorGUILayout.Toggle(new GUIContent("Allow Multiple Dashes Before Touching The Ground?", "Should the player be allowed to dash multiple times after jumping or can they only do it once before having to reset when touching the ground?"), allowMultipleDashesBeforeTouchingGround.boolValue);
        }
        #endregion

        // ======= Fall Settings =======
        #region // ======= Fall Settings =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        showFallSettings.boolValue = EditorGUILayout.Toggle("SHOW FALLING SETTINGS", showFallSettings.boolValue);
        if (showFallSettings.boolValue)
        {
            EditorStyles.label.fontStyle = FontStyle.Normal;
            maxFallSpeed.floatValue = EditorGUILayout.FloatField(new GUIContent("Max Fall Speed", "How fast we fall"), maxFallSpeed.floatValue);
            fallGravityMult.floatValue = EditorGUILayout.Slider(new GUIContent("Fall Gravity Multiplier", "The gravity multiplier when falling"), fallGravityMult.floatValue, 0, 1);
            fallGravityMultOnSteepSlope.floatValue = EditorGUILayout.FloatField(new GUIContent("Fall Gravity Multiplier On Steep Slope", "The gravity multiplier when on a slope that exceeds the Max Slope Angle"), fallGravityMultOnSteepSlope.floatValue);
        }
        #endregion

        // ======= Check Settings =======
        #region // ======= Check Settings =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        showCheckSettings.boolValue = EditorGUILayout.Toggle("SHOW CHECK SETTINGS", showCheckSettings.boolValue);
        if (showCheckSettings.boolValue)
        {
            EditorStyles.label.fontStyle = FontStyle.Normal;
            EditorGUILayout.PropertyField(groundLayer, new GUIContent("Environment Layer", "The layer(s) we can run, jump and wall jump on"));
            groundCheckPoint.objectReferenceValue = (Transform)EditorGUILayout.ObjectField(new GUIContent("Ground Check Transform", "The Transform used to reference where we check for the ground"), groundCheckPoint.objectReferenceValue, typeof(Transform), true);
            frontWallCheckPoint.objectReferenceValue = (Transform)EditorGUILayout.ObjectField(new GUIContent("Front Wall Check Transform", "The Transform used to reference where we check the wall in the direction our character is facing"), frontWallCheckPoint.objectReferenceValue, typeof(Transform), true);
            backWallCheckPoint.objectReferenceValue = (Transform)EditorGUILayout.ObjectField(new GUIContent("Back Wall Check Transform", "The Transform used to reference where we check the wall in the opposite direction our character is facing"), backWallCheckPoint.objectReferenceValue, typeof(Transform), true);
            groundCheckSize.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Ground Check Size", "The size of the box we use to check the ground, use Gizmos to see a visual representation"), groundCheckSize.vector2Value);
            wallCheckSize.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Wall Check Size", "The size of the box we use to check the wall, use Gizmos to see a visual representation"), wallCheckSize.vector2Value);
            slopeCheckStartOffset.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Slope Check Start Offset", "Where the raycast for checking slopes starts in relation to our character's pivot point (travels in direction Vector2.Down)"), slopeCheckStartOffset.vector2Value);
            slopeCheckDistance.floatValue = EditorGUILayout.FloatField(new GUIContent("Slope Check Distance", "The raycast distance below the character to check if we're on a slope"), slopeCheckDistance.floatValue);
        }
        #endregion

        // ======= Assist Settings =======
        #region // ======= Assist Settings =======
        EditorGUILayout.Space();
        EditorStyles.label.fontStyle = FontStyle.Bold;
        showAssistSettings.boolValue = EditorGUILayout.Toggle("SHOW ASSIST SETTINGS", showAssistSettings.boolValue);
        if (showAssistSettings.boolValue)
        {
            EditorStyles.label.fontStyle = FontStyle.Normal;
            coyoteTime.floatValue = EditorGUILayout.Slider(new GUIContent("Coyote Time", "The amount of time given to the player to jump after they have already fallen off a platform"), coyoteTime.floatValue, 0.01f, 0.5f);
            jumpInputBufferTime.floatValue = EditorGUILayout.Slider(new GUIContent("Jump Input Buffer Time", "The amount of time given to jump if the player has pressed the jump button but the conditions haven't been met yet"), jumpInputBufferTime.floatValue, 0.01f, 0.5f);
            dashInputBufferTime.floatValue = EditorGUILayout.Slider(new GUIContent("Dash Input Buffer Time", "The amount of time given to dash if the player has pressed the dash button but the conditions haven't been met yet"), dashInputBufferTime.floatValue, 0.01f, 0.5f);
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