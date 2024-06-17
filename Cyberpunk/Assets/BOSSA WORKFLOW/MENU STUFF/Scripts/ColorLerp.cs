using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorLerp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI uiText; // The Text component you want to modify
    [SerializeField] float duration = 2f; // Duration of each lerp

    void Start()
    {
        StartCoroutine(LerpRandomBrightColorLoop());
    }

    IEnumerator LerpRandomBrightColorLoop()
    {
        Color startColor = RandomBrightColor();
        uiText.color = startColor;

        while (true)
        {
            Color endColor = RandomBrightColor();
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                uiText.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            startColor = endColor; // Set the end color as the new start color
        }
    }

    Color RandomBrightColor()
    {
        // Random.ColorHSV with hue from 0 to 1, saturation from 0.7 to 1, and value from 0.7 to 1
        return Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.7f, 1f);
    }

}
