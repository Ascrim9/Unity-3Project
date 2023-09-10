using System;
using FlowerEndSummer.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace FlowerEndSummer
{
    public class ObjectWolf : MonoBehaviour
    {
        [SerializeField] private Vector3 forwardRayOffset = new Vector3(0, 2.5f, 0);
        [SerializeField] private float forwardRayLength = 0.8f;
        [SerializeField] private float heightRayLength = 5;
        [SerializeField] private LayerMask obstacleLayer;

        public WolfRaycastData PerformRaycast()
        {
            var wolfRaycastData = new WolfRaycastData();

            var forwardOrigin = transform.position + forwardRayOffset;

            wolfRaycastData.isObjectHit = Physics.Raycast(
                forwardOrigin,
                transform.forward,
                out wolfRaycastData.forwardRaycastHit,
                forwardRayLength,
                obstacleLayer);
            
            Debug.DrawRay(forwardOrigin, transform.forward * forwardRayLength,
                wolfRaycastData.isObjectHit ? Color.red : Color.green);


            if (wolfRaycastData.isObjectHit is true)
            {
                var heightOrigin = wolfRaycastData.forwardRaycastHit.point + Vector3.up * heightRayLength;
                wolfRaycastData.isHeightHit = Physics.Raycast(heightOrigin, Vector3.down,
                    out wolfRaycastData.heightRaycastHit, heightRayLength, obstacleLayer);
                
                Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength,
                    wolfRaycastData.isHeightHit ? Color.red : Color.green);
            }

            return wolfRaycastData;
        }
    }

    public struct WolfRaycastData
    {
        public bool isObjectHit;
        public bool isHeightHit;
        public RaycastHit forwardRaycastHit;
        public RaycastHit heightRaycastHit;
    }
}
