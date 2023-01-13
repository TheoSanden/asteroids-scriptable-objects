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
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (string.Equals(other.gameObject.tag, "Asteroid"))
            {
                _healthObservable.ApplyChange(-1);
            }
        }
        private int CalculateImpact() 
        {
            return 0;
        }
    }
}
