using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{
    public class InspectNoiseState : BaseState
    {
        public float inspectSpeed = 0.5f;
        public ForcedTransition doTransition = null;

        private void Awake()
        {
            stateName = StateName.InspectNoise;
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
            stController.SetTarget(stController.triggersDetected.lastListenedNoise);
            speed = inspectSpeed;
            stController.faceCurrentTarget = false;
            attackType = 0;

            float Distance = Vector3.Distance(stController.triggersDetected.lastListenedNoise, stController.agentTransform.position);
            if (Distance <= stController.navMeshAgent.stoppingDistance)
            {
                if (stController.triggersDetected.Noise!=null && stController.triggersDetected.Noise.GetComponent<NoiseObjectProperties>() != null)
                    stController.triggersDetected.Noise.GetComponent<NoiseObjectProperties>().DeactivateNoise();
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