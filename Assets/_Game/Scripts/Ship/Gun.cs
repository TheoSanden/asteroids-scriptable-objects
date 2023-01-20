using System;
using UnityEngine;

namespace Ship
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Laser _laserPrefab;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Shoot();
        }

        private void Shoot()
        {
            var trans = transform;
            Instantiate(_laserPrefab, trans.position, trans.rotation);
        }
    }
}
