using UnityEngine;

namespace Ship
{
    public class Health : MonoBehaviour
    {
        private const int MIN_HEALTH = 0;
        public delegate void OnHealthChange();
        public event OnHealthChange onHealthChange;
        [SerializeField] private Variables.IntVariable _maxHealth;
        private int health = 0;
        public int CurrentHealth
        {
            get => health;
        }
        private void Start()
        {
            health = _maxHealth.Value;
            onHealthChange?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            health = Mathf.Max(MIN_HEALTH, health - damage);
            onHealthChange?.Invoke();
        }
    }
}
