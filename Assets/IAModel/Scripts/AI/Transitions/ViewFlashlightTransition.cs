using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI_3y3net
{

    public class ViewFlashlightTransition : BaseTransition {
        public override void OnTransitionTrigger(StateController stController, Collider other, TriggerType type, LayerMask layerMask)
        {

        }

        public override void InitializeTransition(StateController stController)
        {

        }

        public override void OnTransition(StateController stController)
        {

        }

        public override bool TransitionCondition(StateController stController)
        {
            if (stController.triggersDetected.Flashlight != null)
                return true;
            return false;
        }
    }
}