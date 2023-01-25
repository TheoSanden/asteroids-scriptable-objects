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
        bool invincible = false;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (string.Equals(other.gameObject.tag, "Asteroid") && !invincible)
            {
                _health.TakeDamage(CalculateImpactDamage(other));
                invincible = true;
                StartCoroutine(CoroutineHelper.SetAfterSeconds<bool>(result => invincible = result, false, 0.5f));
            }
        }
        private int CalculateImpactDamage(Collision2D other)
        {
            Asteroids.Asteroid asteroid = other.gameObject.GetComponent<Asteroids.Asteroid>();
            return Mathf.CeilToInt(_maxImpactDamage.Value * asteroid.Settings.SizePercentage);
        }
    }
}
