using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeonEffect : MonoBehaviour
{

    [SerializeField] float minSoftnessValue = 0f;
    [SerializeField] float maxSoftnessValue = 0.4f;
    [SerializeField] float duration = 2f;
    [SerializeField] TextMeshProUGUI titleText;

    private void Start()
    {
        StartCoroutine(LerpSoftnessLoop());
    }


    IEnumerator LerpSoftnessLoop()
    {
        float startSoftness = minSoftnessValue;
        float endSoftness = maxSoftnessValue;

        while (true)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float currentSoftness = Mathf.Lerp(startSoftness, endSoftness, t);
                titleText.fontMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, currentSoftness);
                yield return null;
            }

            //Swap start and end values for continous looping
            float temp = startSoftness;
            startSoftness = endSoftness;
            endSoftness = temp;

        }
    }

}
