using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlls : MonoBehaviour
{
    [ContextMenu("Test")]
    void Test()
    {
        LerpTo(EasingFunction.GetEasingFunction(EasingFunction.Ease.EaseInOutCubic), new Vector3(512, 0, transform.position.z), 3);
    }
    bool inTransition;
    public void ScreenSgake(float intensity, float time, float frequency)
    {
        if (inTransition) return;
        inTransition = true;
        StartCoroutine(IScreenShake(intensity, time, frequency));
    }
    public void LerpTo(EasingFunction.Function easingFunction, Vector2 position, float time)
    {
        if (inTransition) return;
        inTransition = true;
        StartCoroutine(ILerpTo(easingFunction, position, time));
    }
    IEnumerator IScreenShake(float intensity, float time, float frequency)
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
        inTransition = false;
    }
    IEnumerator ILerpTo(EasingFunction.Function easingFunction, Vector3 position, float time)
    {
        float timer = 0;
        Vector3 originalPosition = this.transform.position;
        Vector3 path = position - originalPosition;
        while (timer < time)
        {
            transform.position = originalPosition + (path * easingFunction(0, 1, timer / time));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = position;
        inTransition = false;
    }

}
