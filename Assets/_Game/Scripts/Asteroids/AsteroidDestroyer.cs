using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public class AsteroidDestroyer : MonoBehaviour
    {
        // [SerializeField] private AsteroidSet _asteroids;

        [SerializeField] private AsteroidSpawner spawner;
        [SerializeField] private Score score;
        [SerializeField] private SoundPooler soundPooler;
        [SerializeField] private AudioClip onDestroySound;
        public void OnAsteroidDestroyed(GameObject go)
        {
            print(go.name);
            Asteroid asteroid = go.GetComponent<Asteroid>();
            score.Modify(Mathf.CeilToInt(asteroid.Settings.SizePercentage * 5));

            soundPooler.PlayAt(onDestroySound, go.transform.position, 0.3f * asteroid.Settings.SizePercentage, 3 - (2 * asteroid.Settings.SizePercentage));

            if (asteroid.Settings.SizePercentage < 0.5f)
            {
                DestroyAsteroid(asteroid);
                return;
            }

            spawner.SpawnAtPosition(go.transform.position, asteroid.Settings.SizePercentage / 2);
            //spawner.SpawnAtPosition(go.transform.position, asteroid.Settings.SizePercentage / 2);
            Destroy(asteroid.gameObject);
        }
        private void DestroyAsteroid(Asteroid asteroid)
        {
            Destroy(asteroid.gameObject);
        }
    }
}
