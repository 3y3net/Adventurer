using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{
    public class InspectSmellState : BaseState
    {
        public float inspectSpeed = 0.5f;
        public ForcedTransition doTransition = null;

        private void Awake()
        {
            stateName = StateName.InspectSmell;
        }

        public override void OnEnterStateExtended(StateController stController)
        {
            stController.navMeshAgent.updateRotation = false;
            stController.navMeshAgent.updatePosition = true;
            speed = inspectSpeed;
        }

        public override void OnExitStateExtended(StateController stController)
        {

        }

        public override void UpdateStateExtended(StateController stController)
        {
            stController.navMeshAgent.isStopped = false;
            stController.SetTarget(stController.triggersDetected.lastSmelledObject);
            speed = inspectSpeed;
            stController.faceCurrentTarget = false;
            attackType = 0;

            float Distance = Vector3.Distance(stController.triggersDetected.lastSmelledObject, stController.agentTransform.position);
            if (Distance <= stController.navMeshAgent.stoppingDistance)
            {
                if(stController.triggersDetected.Smelled.GetComponent<SmellObjectProperties>()!= null)
                    stController.triggersDetected.Smelled.GetComponent<SmellObjectProperties>().DeactivateSmell();
                //Switch to alerted state
                if (doTransition != null)
                {
                    //Debug.Log("DO TRANSITION");
                    doTransition.ActivateTransition();
                }
            }
        }
    }
}