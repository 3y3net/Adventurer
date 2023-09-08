using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{
    public enum StateName { Alert, InspectNoise, InspectSmell, Patrol, Pursuit, Idle}

    public abstract class BaseState : MonoBehaviour
    {

        public bool ExitOnFirstTransition = true;
        public List<BaseTransition> transitions;

        public List<AudioClip> enterState;
        public List<AudioClip> stayState;
        public float probPerSecond = 0.1f;

        public StateName stateName;

        [HideInInspector]
        public float speed;
        [HideInInspector]
        public int seeking;
        [HideInInspector]
        public int attackType=0;

        float _timer;

        public void OnEnterState(StateController stController)
        {
            speed = 0;
            seeking = 0;
            RestartTimerValues();
            foreach (BaseTransition bt in transitions)
                bt.InitializeTransition(stController);

            if (enterState.Count > 0)
            {
                stController.audioStates.clip = enterState[UnityEngine.Random.Range(0, enterState.Count)];
                stController.audioStates.Play();
            }

            OnEnterStateExtended(stController);
        }
        
        public void OnExitState(StateController stController)
        {
            OnExitStateExtended(stController);
        }        

        public void UpdateState(StateController stController)
        {
            if (FireTimer() && stayState.Count > 0 && !stController.audioStates.isPlaying)
            {
                stController.audioStates.clip = stayState[UnityEngine.Random.Range(0, stayState.Count)];
                stController.audioStates.Play();
            }
            UpdateStateExtended(stController);
        }

        public abstract void OnEnterStateExtended(StateController stController);
        public abstract void OnExitStateExtended(StateController stController);
        public abstract void UpdateStateExtended(StateController stController);

        public void RestartTimerValues()
        {
            _timer = 1f; //test each second for prob
        }

        protected virtual bool FireTimer()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) //Test for chance
            {
                RestartTimerValues();
                if (Random.Range(0.0f, 1.0f) < probPerSecond)
                    return true;
            }
            return false;
        }
    }
}