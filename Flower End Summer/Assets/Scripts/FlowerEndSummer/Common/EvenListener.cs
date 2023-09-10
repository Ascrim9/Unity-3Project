namespace FlowerEndSummer.Common
{
    using UnityEngine;
    using UnityEngine.Events;

    public class EventListener : MonoBehaviour
    {
        public EventBase gameEvent;
        public UnityEvent response;
        

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}