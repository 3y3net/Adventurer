using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI_3y3net
{

    public class PatrolState : BaseState
    {
        public PatrolPoint patrolPath;
        public float patrolSpeed = 0.5f;
        public bool patrolInverse = false;
        int nextWayPoint;

        private void Awake()
        {
            stateName = StateName.Patrol;
            if(patrolPath==null)
            {
                patrolPath = GameLogic.instance.GetPatrolPoint();
            }
        }

        public override void OnEnterStateExtended(StateController stController)
        {            
            stController.navMeshAgent.updateRotation = false;
            stController.navMeshAgent.updatePosition = true;
            speed = patrolSpeed;
            //Debug.Log("Patrol State: " + gameObject.name);
        }

        public override void OnExitStateExtended(StateController stController)
        {

        }

        public override void UpdateStateExtended(StateController stController)
        {
            Patrol(stController);
        }

        private void Patrol(StateController stController)
        {
            stController.SetTarget(patrolPath.Waypoints[nextWayPoint].position);

            if (stController.navMeshAgent.remainingDistance <= stController.navMeshAgent.stoppingDistance && !stController.navMeshAgent.pathPending)
            {
                if (patrolInverse)
                    nextWayPoint = (nextWayPoint - 1) < 0 ? patrolPath.Waypoints.Count-1 : nextWayPoint - 1;
                else
                    nextWayPoint = (nextWayPoint + 1) % patrolPath.Waypoints.Count;
            }
        }

    }
}