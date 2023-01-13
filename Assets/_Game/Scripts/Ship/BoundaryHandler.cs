using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryHandler : MonoBehaviour
{
    Renderer renderer;
    Camera mainCamera;
    Bounds frustrumBounds;
    bool hasEnteredFrustrum;
    private void OnEnable()
    {
        renderer = GetComponentInChildren<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponent<Renderer>();
        }
        mainCamera = Camera.main;
        CalculateFrustrum();

    }

    void CalculateFrustrum()
    {
        if (mainCamera == null) return;
        frustrumBounds = new Bounds();
        var center = (this.transform.position - mainCamera.transform.position);
        center = mainCamera.transform.forward * Vector3.Dot(mainCamera.transform.forward, center);
        var frustrumHeight = center.magnitude * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustrumWidth = frustrumHeight * mainCamera.aspect;
        frustrumBounds.center = center + mainCamera.transform.position;
        frustrumBounds.extents = new Vector3(frustrumWidth, frustrumHeight);
    }

    private void OnDrawGizmos()
    {
        if (renderer == null)
        {
            return;
        }
        var bounds = renderer.bounds;
        Gizmos.matrix = Matrix4x4.identity;
        bool isInside = InsideFrustrum();
        Gizmos.color = (isInside) ? Color.green : Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);

        //Draw Frustrum Bounds
        Gizmos.DrawWireCube(frustrumBounds.center, frustrumBounds.extents * 2);
    }
    private bool InsideFrustrum()
    {
        var shipBounds = renderer.bounds;
        return frustrumBounds.Intersects(shipBounds);
    }
    private void MirrorPosition()
    {
        //Add horizontal and vertical mirroring (Instead of diagonal)
        Vector3 mirroredVector = frustrumBounds.center - transform.position;
        this.transform.position = mirroredVector;
        this.GetComponent<Rigidbody2D>().AddForce(transform.forward);
    }
    private void Update()
    {
        if (!InsideFrustrum() && hasEnteredFrustrum)
        {
            hasEnteredFrustrum = false;
            MirrorPosition();
        }
        if (InsideFrustrum() && !hasEnteredFrustrum) { hasEnteredFrustrum = true; }
    }

}
