using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI_3y3net
{

    public class ForcedTransition : BaseTransition
    {
        bool activateTransition = false;

        public override void InitializeTransition(StateController stController)
        {
            
        }

        public override void OnTransition(StateController stController)
        {
            
        }

        public void ActivateTransition()
        {
            activateTransition = true;
        }

        public override bool TransitionCondition(StateController stController)
        {
            if(activateTransition)
            {
                activateTransition = false;
                return true;
            }
            return false;
        }
    }
}