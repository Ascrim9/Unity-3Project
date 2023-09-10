using FlowerEndSummer.Event;
using UnityEngine;

namespace UnityTemplateProjects.Test
{
    public class Publisher
    {
        public EventManager eventmanager;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                eventmanager.myEvent.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("CallME");
            }
        }


        public void CallMe2()
        {
            Debug.Log("CALLME Methoad");
        }
    }
}