using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{

    public enum TriggerType { Enter, Stay, Exit }

    public abstract class BaseTransition : MonoBehaviour
    {

        public BaseState transitionState;

        public abstract bool TransitionCondition(StateController stController);
        public abstract void OnTransition(StateController stController);
        public abstract void InitializeTransition(StateController stController);
        public virtual void OnTransitionTrigger(StateController stController, Collider other, TriggerType type, LayerMask layerMask)
        {
            //Called when a collider enters the sphere sensor

        }
    }
}