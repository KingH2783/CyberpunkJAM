using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AnimationTrigger : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NextCutscene panelManager = animator.GetComponent<NextCutscene>();
        if (panelManager == null)
        {
            panelManager = FindObjectOfType<NextCutscene>();
        }

        if (panelManager != null)
        {
            panelManager.EnableNextPanel();
        }
        else
        {
            Debug.LogError("UIPanelManager not found.");
        }
    }
}
