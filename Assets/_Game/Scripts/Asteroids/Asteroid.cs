using DefaultNamespace.ScriptableEvents;
using System.Collections;
using System;
using UnityEngine;
using Variables;
using Random = UnityEngine.Random;

namespace Asteroids
{
    [System.Serializable]
    public class AsteroidSettings
    {
        [SerializeField] float size;
        [SerializeField] float sizePercentage;
        [SerializeField] Sprite sprite;
        public AsteroidSettings(Texture2D texture)
        {
            this.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f),1);
            CalculateSize();
        }
        private void CalculateSize()
        {
            Vector4 padding = UnityEngine.Sprites.DataUtility.GetPadding(sprite);
            float a, b;
            a = sprite.rect.width - padding.x - padding.z;
            b = sprite.rect.height - padding.y - padding.w;
            size = Mathf.Sqrt(a * b);
            sizePercentage = size / (Mathf.Sqrt(sprite.rect.width * sprite.rect.height));
        }
        public float Size
        {
            get => size;
        }
        public float SizePercentage
        {
            get => sizePercentage;
        }
        public Sprite Sprite
        {
            get => sprite;
        }
    }
    [RequireComponent(typeof(Rigidbody2D))]
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private ScriptableEventInt _onAsteroidDestroyed;
        
        [Header("Config:")]
        [SerializeField] private float _minForce;
        [SerializeField] private float _maxForce;
        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;
        [SerializeField] private float _minTorque;
        [SerializeField] private float _maxTorque;
        [SerializeField] private FloatVariable _maxHealth;
        [SerializeField] private AudioClip _onHitSound;

        [Header("References:")]
        [SerializeField] private Transform _shape;

        private AsteroidSettings settings;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private AudioSource _audioSource;
        private Vector3 _direction;
        private int _instanceId;
        private int _totalHitPoints;
        private int _hitPoints;
        bool isDestroying = false;
        public void Initialize(AsteroidSettings settings)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _instanceId = GetInstanceID();
            _spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
            this.settings = settings;         
            _spriteRenderer.sprite = settings.Sprite;
            SetDirection();
            AddForce();
            AddTorque();
            AddCollider();
            SetOnHitSound();
            CalculateHealth();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (string.Equals(other.tag, "Laser"))
            {
               HitByLaser();
               Destroy(other.gameObject);
            }
        }

        private void HitByLaser()
        {
            if (!isDestroying) 
            {
                _audioSource.Play();
                _hitPoints -= 1;
            }
            if(_hitPoints <= 0) 
            {
                isDestroying = true;
                StartCoroutine(DestroyAfterSound());
            }
        }
        IEnumerator DestroyAfterSound()
        {
            while (_audioSource.isPlaying) 
            {
                yield return new WaitForEndOfFrame();
            }
            Destroy(gameObject);
        }
        // TODO Can we move this to a single listener, something like an AsteroidDestroyer?
        public void OnHitByLaser(IntReference asteroidId)
        {
            if (_instanceId == asteroidId.GetValue())
            {
                Destroy(gameObject);
            }
        }
        
        public void OnHitByLaserInt(int asteroidId)
        {
            if (_instanceId == asteroidId)
            {
                Destroy(gameObject);
            }
        }
        
        private void SetDirection()
        {
            var size = new Vector2(50f, 50f);
            var target = new Vector3
            (
                Random.Range(-size.x, size.x),
                Random.Range(-size.y, size.y)
            );

            _direction = (target - transform.position).normalized;
        }

        private void AddForce()
        {
            var force = Random.Range(_minForce, _maxForce);
            _rigidbody.AddForce( _direction * force, ForceMode2D.Impulse);
        }

        private void AddTorque()
        {
            var torque = Random.Range(_minTorque, _maxTorque);
            var roll = Random.Range(0, 2);

            if (roll == 0)
                torque = -torque;
            
            _rigidbody.AddTorque(torque, ForceMode2D.Impulse);
        }
        private void AddCollider() 
        {
            _spriteRenderer.gameObject.AddComponent<PolygonCollider2D>();
        }
        private void CalculateHealth()
        {
            _totalHitPoints = _hitPoints = Mathf.CeilToInt(settings.SizePercentage * _maxHealth.Value);
        }
        private void SetOnHitSound()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.clip = _onHitSound;
            _audioSource.pitch = 3 - (2 * settings.SizePercentage);
            _audioSource.volume = 0.3f;
        }
    }
}
