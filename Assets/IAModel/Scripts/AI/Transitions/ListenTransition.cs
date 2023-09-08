using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{
    public class ListenTransition : BaseTransition
    {
        public bool NegativeCondition = false;
        public float triggerDistance = 100000f;

        public override void InitializeTransition(StateController stController)
        {

        }

        public override void OnTransition(StateController stController)
        {

        }

        public override bool TransitionCondition(StateController stController)
        {
            if (stController.triggersDetected.Noise != null && stController.triggersDetected.distanceNoise < triggerDistance && !NegativeCondition)
            {
                stController.currentTarget = stController.triggersDetected.lastListenedNoise;
                return true;
            }
            else if (stController.triggersDetected.Noise != null && NegativeCondition)
                return false;

            return NegativeCondition;
        }
    }
}