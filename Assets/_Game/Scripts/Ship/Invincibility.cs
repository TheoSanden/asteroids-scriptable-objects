using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class Invincibility : MonoBehaviour
    {
        [SerializeField] private ScriptableData<bool> invincibilityBool;
        [SerializeField] private Collider2D collider;
        public bool Invincible
        {
            get => invincibilityBool.Data;
        }

        private void Start()
        {
            invincibilityBool.Data = false;
        }
        public void Activate(float seconds)
        {
            invincibilityBool.Data = true;
            StartCoroutine(CoroutineHelper.SetAfterSeconds(result => invincibilityBool.Data = result, false, seconds));
        }
        public void Activate(float seconds, bool disableCollider)
        {
            invincibilityBool.Data = true;
            StartCoroutine(CoroutineHelper.SetAfterSeconds(result => invincibilityBool.Data = result, false, seconds));
            if (disableCollider)
            {
                collider.enabled = false;
                StartCoroutine(CoroutineHelper.SetAfterSeconds(result => collider.enabled = result, true, seconds));
            };
        }
    }
}
