using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FlowerEndSummer.Common
{
    [CreateAssetMenu(fileName = "EventSystem", menuName = "Events/EventSystem")]
    public class EventBase : ScriptableObject
    {
        private List<EventListener> listeners = new List<EventListener>();

        public void Raise()
        {
            foreach (var variable in listeners)
            {
                variable.OnEventRaised();
            }
        }

        public void RegisterListener(EventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(EventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}