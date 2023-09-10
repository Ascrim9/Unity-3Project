using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace FlowerEndSummer
{
    public class DynamicParkourController : MonoBehaviour
    {
        [SerializeField] private List<DynamicParkourSettings> pakourSettings;
        private ObjectWolf objectWolf;
        private Animator animator;
        private PlayerController playerController;
        private bool inAction;

        private void Awake()
        {
            objectWolf = GetComponent<ObjectWolf>();
            animator = GetComponent<Animator>();

            playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (Input.GetButton(("Jump")) && !inAction)
            {
                var wolfData = objectWolf.PerformRaycast();
                if (wolfData.isObjectHit)
                {
                    foreach (var variable in pakourSettings)
                    {
                        if (variable.CheckDir(wolfData, transform))
                        {
                            StartCoroutine(IParkourActioning(variable));
                            break;
                        }
                    }
                    
                }
            }
            // objectWolf.PerformRaycast();

        }


        private IEnumerator IParkourActioning(DynamicParkourSettings action)
        {
            Debug.Log("PARKURU!");
            inAction = true;
            
            playerController.SetControl(false);
            
            animator.CrossFade(action.AnimationName, 0.2f);
            yield return null;

            var animationState = animator.GetNextAnimatorStateInfo(0);
            

            float timer = 0.0f;
            while (timer <= animationState.length)
            {
                timer += Time.deltaTime;

                // 회전할 때 자동 회전 기능 취소
                if (action.IsTargetMatch)
                {
                    MatchTarget(action);
                }

                if (animator.IsInTransition(0) && timer > 0.5f)
                {
                    break;
                }

                yield return null;
            }

            playerController.SetControl(true);
            inAction = false;
        }


        void MatchTarget(DynamicParkourSettings action)
        {
            if (animator.isMatchingTarget) return;

            // Adjust the match position based on the current object's position and direction
            Vector3 adjustedMatchPos = transform.TransformPoint(action.MatchPos);

            // Clamp the target match time to a value between 0 and 1
            float clampedMatchTime = Mathf.Clamp(action.TargetMatchTime, 0, 1);

            animator.MatchTarget(
                action.MatchPos,
                transform.rotation,
                action.AvataMatch,
                new MatchTargetWeightMask(action.MatchPosWeight, 0),
                action.TargetStartTime,
                action.TargetMatchTime);
        }
    }
}