using System;
using UnityEngine;

namespace Ship
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Laser _laserPrefab;
        [SerializeField] private AudioClip shootSound;
        private AudioSource audioSource;
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = shootSound;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Shoot();
        }

        private void Shoot()
        {
            var trans = transform;
            Instantiate(_laserPrefab, trans.position, trans.rotation);
            audioSource.Play();
        }
    }
}
