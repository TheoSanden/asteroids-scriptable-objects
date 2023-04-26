using UnityEngine;

namespace Ship
{
    public class Health : MonoBehaviour
    {
        //Move this later in case we want animations to play and such
        GameEvent gameOverEvent;
        private const int MIN_HEALTH = 0;
        public delegate void OnHealthChange();
        public event OnHealthChange onHealthChange;
        public event OnHealthChange onHealthZero;
        [SerializeField] private Variables.IntVariable _maxHealth;
        private int health = 0;
        public int CurrentHealth
        {
            get => health;
        }
        private void Start()
        {
            gameOverEvent = Resources.Load<GameEvent>("Events/GameOverEvent");
            Debug.Log(gameOverEvent);
            health = _maxHealth.Value;
            onHealthChange?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            health = Mathf.Max(MIN_HEALTH, health - damage);
            onHealthChange?.Invoke();

            if (health <= MIN_HEALTH)
            {
                onHealthZero?.Invoke();
                gameOverEvent?.Raise();
            }
        }
    }
}
