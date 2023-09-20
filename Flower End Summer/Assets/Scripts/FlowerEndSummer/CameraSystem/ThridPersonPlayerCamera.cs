using System;
using System.Collections;
using UnityEngine;


namespace FlowerEndSummer
{
    [RequireComponent(typeof(Camera))]
    public class ThridPersonPlayerCamera : MonoBehaviour
    {
        public Transform playerTransform;
        public Vector3 camOffset = new Vector3(0.4f, 0.5f, -2.0f);
        public Vector3 pivotOffset = new Vector3(0.0f, 1.0f, 0.0f);

        public float Smooth = 1.0f;
         public float HorizontalAimSpeed = 6.0f;
         public float VerticalAimSpeed = 6.0f;
        public float MaxVAngle = 30.0f;
        public float MinVAngle = -60.0f;

        public float RecoilValue = 10.0f;
        
        
        private float angleH = 0.0f;
        private float angleV = 0.0f;

        private Transform cameraTrnasform; //Camera cashing;
        private Camera myCamera;
        private Vector3 relCameraPos;
        private float relCameraPosMag;
        private Vector3 smoothPivotOffset;
        private Vector3 smoothCamOffset;
        private Vector3 targetPivotOffset;
        private Vector3 targetCamOffset;

        private float defaultFOV;
        private float targetFOV;
        private float targetMaxVerticalAngle;
        private float recoilAngle = 0.0f;


        public float GetH
        {
            get => angleH;
        }

        private void Awake()
        {
            //cashing
            cameraTrnasform = GetComponent<Transform>();
            myCamera = cameraTrnasform.GetComponent<Camera>();

            var position = playerTransform.position;
            
            cameraTrnasform.position = position + Quaternion.identity * pivotOffset
                                                + Quaternion.identity * camOffset;
            cameraTrnasform.rotation = Quaternion.identity;
            ;

                
            //카메라 플레이어 충돌체크
            relCameraPos = cameraTrnasform.position - position;
            relCameraPosMag = relCameraPos.magnitude - 0.5f;

            smoothPivotOffset = pivotOffset;
            smoothCamOffset = camOffset;
            defaultFOV = myCamera.fieldOfView;
            angleH = playerTransform.eulerAngles.y;
            
            
            ResetTargetOffset();
            ResetFOV();
            ResetMaxVerticalAngle();

        }

        public void ResetTargetOffset()
        {
            targetPivotOffset = pivotOffset;
            targetCamOffset = camOffset;
        }

        public void ResetFOV()
        {
            targetFOV = defaultFOV;
        }

        public void ResetMaxVerticalAngle()
        {
            targetMaxVerticalAngle = targetMaxVerticalAngle;
        }

        public void BounceVertical(float degree)
        {
            recoilAngle = degree;
        }

        public void SetTargetOffset(Vector3 newPivotOffset, Vector3 newCamOffset)
        {
            targetPivotOffset = newPivotOffset;
            targetCamOffset = newCamOffset;
        }

        public void SetFOV(float customFOV)
        {
            targetFOV = customFOV;
        }

        bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
        {
            Vector3 target = playerTransform.position + (Vector3.up * deltaPlayerHeight);

            
            Debug.DrawRay(checkPos, target - checkPos * 10.0f, Color.blue);
            
            if (Physics.SphereCast(checkPos, 0.2f, target - checkPos, out RaycastHit hit, relCameraPosMag))
            {
                if (hit.transform != playerTransform && !hit.transform.GetComponent<Collider>().isTrigger)
                {
                    return false;
                }
            }
            return true;
        }

        bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float maxDistnace)
        {
            Vector3 origin = playerTransform.position + (Vector3.up * deltaPlayerHeight);
            Debug.DrawRay(checkPos, checkPos-origin * 10.0f, Color.red);
            
            if (Physics.SphereCast(checkPos, 0.2f, checkPos - origin, out RaycastHit hit, relCameraPosMag))
            {
                if (hit.transform != playerTransform && !hit.transform.GetComponent<Collider>().isTrigger)
                {
                    return false;
                }
            }
            return true;
        }


        bool DoubleViewingPosCheck(Vector3 checkPos, float offset)
        {
            
            float playerFocusHeight = playerTransform.GetComponent<CapsuleCollider>().height * 0.75f;
            return ViewingPosCheck(checkPos, playerFocusHeight) &&
                   ReverseViewingPosCheck(checkPos, playerFocusHeight, offset);
        }


        private void Update()
        {
            //마우스 이동
            angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1.0f, 1.0f) * HorizontalAimSpeed;
            angleV+= Mathf.Clamp(Input.GetAxis("Mouse Y"), -1.0f, 1.0f) * targetMaxVerticalAngle;
            
            //수직 이동 제한
            angleV = Mathf.Clamp(angleV, MinVAngle, MaxVAngle);
            
            //반동
            angleV = Mathf.LerpAngle(angleV, angleV + recoilAngle, 10.0f * Time.deltaTime);
            
            //카메라 회전
            Quaternion camYRotation = Quaternion.Euler(0.0f, angleH, 0.0f);
            Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0.0f);
            cameraTrnasform.rotation = aimRotation;

            //Set FOV
            myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, targetFOV, Time.deltaTime);

            Vector3 baseTempPosition = playerTransform.position + camYRotation * targetPivotOffset;
            Vector3 noCollisionOffset = targetCamOffset; //조존 앞에 이동 안되게

            for (float zOffset = targetCamOffset.z; zOffset <= 0.0f; zOffset += 0.5f)
            {
                if (DoubleViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset
                        , Mathf.Abs(zOffset)) || zOffset == 0.0f)
                {
                    break;
                }
            }
            
            //Repositon Camera
            smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, Smooth * Time.deltaTime);
            smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, Smooth * Time.deltaTime);

            cameraTrnasform.position = playerTransform.position + camYRotation * smoothPivotOffset +
                                       aimRotation * smoothCamOffset;


            if (recoilAngle > 0.0f)
            {
                recoilAngle -= RecoilValue * Time.deltaTime;
            }
            else if (recoilAngle < 0.0f)
            {
                recoilAngle += recoilAngle * Time.deltaTime;
            }
        }
        
        
        //
        public float GetCurrentPivotMagnitude(Vector3 finalOffset)
        {
            return Mathf.Abs((finalOffset - smoothPivotOffset).magnitude);
        }
    }
}