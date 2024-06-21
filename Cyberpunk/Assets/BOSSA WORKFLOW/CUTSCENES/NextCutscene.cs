using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextCutscene : MonoBehaviour
{
    public List<GameObject> panels;
    private int currentIndex = 0;

    public void EnableNextPanel()
    {
        if (panels == null || panels.Count == 0) return;

        // Disable the current panel if it exists
        if (currentIndex < panels.Count)
        {
            panels[currentIndex].SetActive(false);
        }

        // Increment the index to point to the next panel
        currentIndex++;

        // Enable the next panel if it exists
        if (currentIndex < panels.Count)
        {
            panels[currentIndex].SetActive(true);
        }
    }
}
