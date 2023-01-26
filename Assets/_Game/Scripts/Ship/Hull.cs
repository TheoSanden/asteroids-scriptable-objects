using DefaultNamespace.ScriptableEvents;
using UnityEngine;
using Variables;

namespace Ship
{
    public class Hull : MonoBehaviour
    {
        //[SerializeField] private IntVariable _health;
        [SerializeField] private ScriptableEventIntReference _onHealthChangedEvent;
        [SerializeField] private IntReference _healthRef;
        [SerializeField] private IntObservable _healthObservable;
        [SerializeField] private Health _health;
        [SerializeField] private FloatVariable _maxImpactDamage;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Invincibility invincibility;
        [SerializeField] private CameraControlls _cameraControlls;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (string.Equals(other.gameObject.tag, "Asteroid") && !invincibility.Invincible)
            {
                _health.TakeDamage(CalculateImpactDamage(other));
                _cameraControlls.InvokeScreenShake(other.gameObject.GetComponent<Asteroids.Asteroid>().Settings.SizePercentage * 8, 0.3f, 4);
                invincibility.Activate(1);
            }
        }
        private int CalculateImpactDamage(Collision2D other)
        {
            Asteroids.Asteroid asteroid = other.gameObject.GetComponent<Asteroids.Asteroid>();
            return Mathf.CeilToInt(_maxImpactDamage.Value * asteroid.Settings.SizePercentage);
        }
    }
}
