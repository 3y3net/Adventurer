using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{
    public class TimerTransition : BaseTransition
    {
        public float totalTime = 10f;
        public float randomDelta = 0.0f;

        float _timer;
        float _randomDelta;

        public override void InitializeTransition(StateController stController)
        {
            //Debug.Log("InitializeTransition Timer");
            RestartValues();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
        }

        public void RestartValues()
        {
            _timer = totalTime;
            _randomDelta = UnityEngine.Random.Range(-randomDelta, randomDelta);
        }

        public override void OnTransition(StateController stController)
        {
            RestartValues();
        }

        public override bool TransitionCondition(StateController stController)
        {
            return (_timer + _randomDelta) <= 0f;
        }
    }
}
