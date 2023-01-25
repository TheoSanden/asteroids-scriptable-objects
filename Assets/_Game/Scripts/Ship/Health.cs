using UnityEngine;

namespace Ship
{
    public class Health : MonoBehaviour
    {
        public delegate void OnHealthChange();
        public event OnHealthChange onHealthChange;
        [SerializeField] private int health = 0;
        public int CurrentHealth
        {
            get => health;
        }
        private void Start()
        {
            health = MAX_HEALTH;
            onHealthChange?.Invoke();
        }
        private const int MIN_HEALTH = 0;
        public const int MAX_HEALTH = 16;

        public void TakeDamage(int damage)
        {
            health = Mathf.Max(MIN_HEALTH, health - damage);
            onHealthChange?.Invoke();
        }
    }
}
