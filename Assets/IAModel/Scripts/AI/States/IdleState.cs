using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI_3y3net
{

    public class IdleState : BaseState
    {
        public Transform IdlePoint;
        public float moveSpeed = 3f;
        public float directionChangeTime = 1.5f;

        float _directionChangeTimer = 0.0f;

        private void Awake()
        {
            stateName = StateName.Idle;
        }

        public override void OnEnterStateExtended(StateController stController)
        {
            stController.navMeshAgent.updateRotation = false;
            stController.navMeshAgent.updatePosition = true;
            speed = 0;
            stController.SetTarget(IdlePoint.position);
            //Debug.Log("ENTERS");
        }

        public override void OnExitStateExtended(StateController stController)
        {

        }

        public override void UpdateStateExtended(StateController stController)
        {
            DoIdle(stController);
        }

        private void DoIdle(StateController stController)
        {            
            if (stController.navMeshAgent.remainingDistance > stController.navMeshAgent.stoppingDistance || stController.navMeshAgent.pathPending)
            {
                stController.SetTarget(IdlePoint.position);
                speed = moveSpeed;
                //Debug.Log("SE MUEVE "+ stController.navMeshAgent.remainingDistance);
            }
            else
            {
                speed = 0;
            }

            // Reduce Timer
            _directionChangeTimer += Time.deltaTime;
            if (_directionChangeTimer > directionChangeTime && directionChangeTime>0.1)
            {
                seeking = (int)Mathf.Sign(UnityEngine.Random.Range(-1.0f, 1.0f));
                _directionChangeTimer = 0.0f;
            }
        }

    }
}