using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Cinemachine_CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;

    public float enrageShakeStart;
    public float enrageShakeGain;

    public AnimationCurve shakeCurve;
    private CinemachineBasicMultiChannelPerlin noiseModule;

    private bool isIncreasingShake;

    void OnEnable()
    {
        PlayerRage.OnEnrageStart += StartEnrageShake;
        PlayerRage.OnEnrageEnd += EndEnrageShake;

    }

    void OnDisable()
    {
        PlayerRage.OnEnrageStart -= StartEnrageShake;
        PlayerRage.OnEnrageEnd -= EndEnrageShake;
    }

    void Start()
    {
        noiseModule = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    void StartEnrageShake()
    {
        noiseModule.m_FrequencyGain = enrageShakeStart;
        isIncreasingShake = true;
    }

    void EndEnrageShake()
    {
        noiseModule.m_FrequencyGain = 0;
        isIncreasingShake = false;

    }
}
