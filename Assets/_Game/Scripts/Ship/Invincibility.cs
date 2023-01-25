using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class Invincibility : MonoBehaviour
    {
        [SerializeField] Collider2D collider;

        public void Activate(float seconds)
        {
            collider.enabled = false;
            StartCoroutine(CoroutineHelper.SetAfterSeconds(result => collider.enabled = result, true, seconds));
        }
    }
}
