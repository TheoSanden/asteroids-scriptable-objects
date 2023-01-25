using System;
using UnityEngine;

namespace Ship
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Laser _laserPrefab;
        [SerializeField] private AudioClip shootSound;
        private AudioSource audioSource;

        bool coolDown = false;
        float coolDownTime = 0.3f;
        float rapidFireTimer = 0;
        float rapidFireFrequency = 0.08f;
        float rapidFireShotAmount = 3;
        float rapidFireShots = 0;
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = shootSound;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Shoot();
            RapidFire();

        }

        private void Shoot()
        {
            var trans = transform;
            Instantiate(_laserPrefab, trans.position, trans.rotation);
            audioSource.Play();
        }
        private void RapidFire()
        {
            if (Input.GetKey(KeyCode.Mouse1) && !coolDown)
            {
                if (rapidFireTimer > rapidFireFrequency) { rapidFireTimer = 0; }
                if (rapidFireTimer == 0) { Shoot(); rapidFireShots++; }
                if (rapidFireShots >= rapidFireShotAmount) { rapidFireShots = 0; coolDown = true; StartCoroutine(CoroutineHelper.SetAfterSeconds(result => coolDown = result, false, coolDownTime)); }
            }
            rapidFireTimer += Time.deltaTime;
        }
    }
}
