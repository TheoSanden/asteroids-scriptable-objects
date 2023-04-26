using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private Asteroid asteroidPrefab;
        [SerializeField] private float minSpawnTime;
        [SerializeField] private float maxSpawnTime;
        [SerializeField] private int minAmount;
        [SerializeField] private int maxAmount;

        public bool Pause
        {
            get => pause;
            set => pause = value;
        }
        bool pause = false;
        private float timer;
        private float nextSpawnTime;
        private Camera _camera;
        private AsteroidFactory factory;

        private enum SpawnLocation
        {
            Top,
            Bottom,
            Left,
            Right
        }

        private void Start()
        {
            _camera = Camera.main;
            factory = FindObjectOfType<AsteroidFactory>();
            Spawn();
            UpdateNextSpawnTime();
        }
        private void Stop()
        {

        }
        private void Update()
        {
            UpdateTimer();

            if (!ShouldSpawn())
                return;

            Spawn();
            UpdateNextSpawnTime();
            timer = 0f;
        }

        private void UpdateNextSpawnTime()
        {
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }

        private void UpdateTimer()
        {
            timer += Time.deltaTime;
        }

        private bool ShouldSpawn()
        {
            return (timer >= nextSpawnTime && !pause);
        }

        public void Spawn()
        {
            var amount = Random.Range(minAmount, maxAmount + 1);

            for (var i = 0; i < amount; i++)
            {
                var location = GetSpawnLocation();
                var position = GetStartPosition(location);
                Asteroid go = Instantiate(asteroidPrefab, position, Quaternion.identity);
                go.Initialize(factory.GetRandomAsteroid(), Asteroid.SpawnType.Edge);
            }
        }
        public void Spawn(float size)
        {
            var amount = Random.Range(minAmount, maxAmount + 1);
            var location = GetSpawnLocation();
            var position = GetStartPosition(location);
            Asteroid go = Instantiate(asteroidPrefab, position, Quaternion.identity);
            go.Initialize(factory.GetAsteroid(size), Asteroid.SpawnType.Edge);

        }
        public void SpawnAtPosition(Vector3 position, float size)
        {
            Asteroid go = Instantiate(asteroidPrefab, position, Quaternion.identity);
            go.Initialize(factory.GetAsteroid(size), Asteroid.SpawnType.Radial);
        }
        private static SpawnLocation GetSpawnLocation()
        {
            var roll = Random.Range(0, 4);

            return roll switch
            {
                1 => SpawnLocation.Bottom,
                2 => SpawnLocation.Left,
                3 => SpawnLocation.Right,
                _ => SpawnLocation.Top
            };
        }

        private Vector3 GetStartPosition(SpawnLocation spawnLocation)
        {
            var pos = new Vector3 { z = Mathf.Abs(_camera.transform.position.z) };

            const float padding = 5f;
            switch (spawnLocation)
            {
                case SpawnLocation.Top:
                    pos.x = Random.Range(0f, Screen.width);
                    pos.y = Screen.height + padding;
                    break;
                case SpawnLocation.Bottom:
                    pos.x = Random.Range(0f, Screen.width);
                    pos.y = 0f - padding;
                    break;
                case SpawnLocation.Left:
                    pos.x = 0f - padding;
                    pos.y = Random.Range(0f, Screen.height);
                    break;
                case SpawnLocation.Right:
                    pos.x = Screen.width - padding;
                    pos.y = Random.Range(0f, Screen.height);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spawnLocation), spawnLocation, null);
            }

            return _camera.ScreenToWorldPoint(pos);
        }
    }
}
