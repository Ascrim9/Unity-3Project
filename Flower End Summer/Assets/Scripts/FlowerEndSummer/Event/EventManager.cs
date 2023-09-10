using System;
using UnityEngine;
using UnityEngine.Events;

namespace FlowerEndSummer.Event
{
    public class EventManager : MonoBehaviour
    {
        public UnityEvent myEvent;
        
        private void Awake() {
            if (myEvent == null)
                myEvent = new UnityEvent();

            myEvent.AddListener(SomeFunction);
            myEvent.AddListener(KAKAO);
        }

        public void TriggerEvent() {
            myEvent.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                myEvent.Invoke();
            }
        }

        private void SomeFunction() {
            // Do something here45
            Debug.Log("SomeFUnction");
        }
        private void KAKAO() {  
            // Do something here
            Debug.Log("TKAKAO");
        }
    }
}