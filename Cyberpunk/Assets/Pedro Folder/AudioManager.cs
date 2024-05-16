using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Volumes")]
    [SerializeField] private AK.Wwise.RTPC masterVolumeRTPC;
    [SerializeField] private AK.Wwise.RTPC sfxVolumeRTPC;
    [SerializeField] private AK.Wwise.RTPC musicVolumeRTPC;
}
