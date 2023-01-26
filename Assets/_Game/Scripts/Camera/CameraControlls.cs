using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlls : MonoBehaviour
{
    bool isShaking;
    public void InvokeScreenShake(float intensity, float time, float frequency)
    {
        if (isShaking) return;
        isShaking = true;
        StartCoroutine(ScreenShake(intensity, time, frequency));
    }
    IEnumerator ScreenShake(float intensity, float time, float frequency)
    {
        float timer = 0;
        Vector3 originalPosition = this.transform.position;
        float _intensity;
        while (timer < time)
        {
            _intensity = (1 - (timer / time)) * intensity;
            float wave = Mathf.Sin(((timer * frequency) / time) * Mathf.PI * 2);
            Vector3 offset = new Vector3(wave * _intensity, 0, 0);
            this.transform.position = originalPosition + offset;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        this.transform.position = originalPosition;
        isShaking = false;
    }
}
