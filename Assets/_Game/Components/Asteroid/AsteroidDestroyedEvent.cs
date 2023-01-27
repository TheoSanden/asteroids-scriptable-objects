using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace DefaultNamespace.GameEvents
{
    [CreateAssetMenu(fileName = "new Asteroid Event", menuName = "ScriptableObjects/Events/Asteroid", order = 0)]
    public class GameEventAsteroid : ScriptableObject
    {
        private List<GameEventListener> _listeners = new List<GameEventListener>();
        private UnityEvent<Asteroids.Asteroid> _event;

        public void Raise(Asteroids.Asteroid value)
        {
            _event?.Invoke(value);
        }

        public void Register(GameEventListener onEvent)
        {
            _listeners.Add(onEvent);
        }

        public void Unregister(GameEventListener onEvent)
        {
            _listeners.Remove(onEvent);
        }
    }
}
