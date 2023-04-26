using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    float lerpToGameOverScreenTime = 2.0f;
    CameraControlls camControlls;
    void Start()
    {
        camControlls = Camera.main.GetComponent<CameraControlls>();
    }
    public void OnGameOver()
    {
        camControlls.LerpTo(EasingFunction.EaseInOutSine, this.transform.position, lerpToGameOverScreenTime);
    }
}
