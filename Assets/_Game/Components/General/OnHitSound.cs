using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitSound : MonoBehaviour
{
    Rigidbody2D rb;
    AudioSource audioSource;
    [SerializeField]AudioClip onHitSound;
    const float maxHitVelocity = 200;
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = onHitSound;
        rb = GetComponent<Rigidbody2D>();
        if(rb == null) { GetComponentInChildren<Rigidbody2D>(); }
        if (rb == null) { GetComponentInParent<Rigidbody2D>(); }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
            OnHitWithRidgidBody(other);
    }
    protected virtual void OnHitWithRidgidBody(Collision2D other)
    {
        Vector2 normal = other.otherRigidbody.velocity.normalized;
        Vector2 scalarProj = normal * Vector2.Dot(normal, other.rigidbody.velocity);
        float hitVelocity = scalarProj.magnitude;
        float hitVolume = Mathf.Clamp01(hitVelocity/ maxHitVelocity);
        audioSource.volume = hitVolume;
        audioSource.Play();
    }
}
