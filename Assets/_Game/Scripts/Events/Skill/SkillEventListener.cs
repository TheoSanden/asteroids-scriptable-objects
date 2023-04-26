using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class SkillEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public SkillEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<Skill> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Skill obj)
    {
        Response.Invoke(obj);
    }
}
