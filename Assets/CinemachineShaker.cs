using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShaker : MonoBehaviour
{
    public static CinemachineShaker Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float startigIntensity;
    private Vector3 offSetTarget, currentOffSet;

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void Shaker(float instesity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = instesity;
        shakeTimer = time;
    }
    // Update is called once per frame
    private void Update()
    {
        StartCoroutine(shake());
    }
    private IEnumerator shake()
    {
        shakeTimer -= Time.deltaTime;
        while (shakeTimer <= 0f)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            yield return null;
        }
    }
}
