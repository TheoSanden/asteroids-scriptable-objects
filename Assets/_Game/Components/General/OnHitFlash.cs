using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OnHitFlash : MonoBehaviour
{
    [SerializeField] List<string> hitTag;
    [SerializeField] Material hitMaterial;
    [SerializeField] float hitDuration = 0.001f;
    Material baseMaterial;

    private SpriteRenderer sr;
    private void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        baseMaterial = sr.material;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitTag.Contains(collision.tag)) 
        {
            StartCoroutine(Hit());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitTag.Contains(collision.collider.tag))
        {
            StartCoroutine(Hit());
        }
    }
    private IEnumerator Hit()
    {
        Material previousMaterial = sr.material;
        sr.sharedMaterial = sr.material = hitMaterial;
        yield return new WaitForSeconds(hitDuration);
        sr.sharedMaterial = sr.material = baseMaterial;
    }

}
