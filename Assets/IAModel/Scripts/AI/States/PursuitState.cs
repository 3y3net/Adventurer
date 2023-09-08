using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{
    public class PursuitState : BaseState
    {

        public float pursuitSpeed = 2f;
        public float attackDistance = 3f;

        public ForcedTransition doTransition = null;

        private void Awake()
        {
            stateName = StateName.Pursuit;
        }

        public override void OnEnterStateExtended(StateController stController)
        {
            stController.navMeshAgent.updateRotation = false;
            stController.navMeshAgent.updatePosition = true;
            speed = pursuitSpeed;
        }

        public override void OnExitStateExtended(StateController stController)
        {

        }

        public override void UpdateStateExtended(StateController stController)
        {
            Pursuit(stController);
        }

        private void Pursuit(StateController stController)
        {
            stController.navMeshAgent.isStopped = false;
            stController.SetTarget(stController.triggersDetected.lastViewedPlayer);
            speed = pursuitSpeed;
            stController.faceCurrentTarget = false;
            attackType = 0;

            if (stController.triggersDetected.Player != null)
            {
                float Distance = Vector3.Distance(stController.triggersDetected.Player.transform.position, stController.agentTransform.position);
                if (Distance < attackDistance)
                {
                    
                    stController.navMeshAgent.isStopped = true;

                    Vector3 lookPos = stController.triggersDetected.Player.transform.position - stController.agentTransform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    stController.agentTransform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20.0f);


                    speed = 0;
                    stController.faceCurrentTarget = true;

                    if (stController.injuredState <2)
                        attackType = UnityEngine.Random.Range(1, 6);
                }
            }
            else
            {
                float Distance = Vector3.Distance(stController.triggersDetected.lastViewedPlayer, stController.agentTransform.position);
                if (Distance <= stController.navMeshAgent.stoppingDistance)
                {
                    //Switch to alerted state
                    if (doTransition != null)
                    {
                        doTransition.ActivateTransition();
                    }
                }
            }
        }
    }
}