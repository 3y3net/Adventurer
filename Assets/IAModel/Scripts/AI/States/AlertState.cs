using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{
    public class AlertState : BaseState
    {

        public float directionChangeTime = 1.5f;

        // Private Fields
        float _directionChangeTimer = 0.0f;

        private void Awake()
        {
            stateName = StateName.Alert;
        }

        public override void OnEnterStateExtended(StateController stController)
        {
            
            stController.navMeshAgent.updateRotation = false;
            stController.navMeshAgent.updatePosition = true;
            _directionChangeTimer = 0f;

            stController.navMeshAgent.isStopped = true;
            speed = 0;            
            //Debug.Log("Alerted State: " + gameObject.name);
        }

        public override void OnExitStateExtended(StateController stController)
        {

        }

        public override void UpdateStateExtended(StateController stController)
        {
            SetSeek(stController);            
        }

        private void SetSeek(StateController stController)
        {
            // Reduce Timer
            _directionChangeTimer += Time.deltaTime;
            if (_directionChangeTimer > directionChangeTime)
            {
                seeking = (int)Mathf.Sign(Random.Range(-1.0f, 1.0f));
                _directionChangeTimer = 0.0f;
            }
        }
    }
}