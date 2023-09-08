using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_3y3net
{

    public class RandomTransition : BaseTransition
    {

        public float probPerSecond = 0.1f;

        float _timer;

        public override void InitializeTransition(StateController stController)
        {
            //Debug.Log("InitializeTransition Random");
            RestartValues();
        }

        public void RestartValues()
        {
            _timer = 1f; //test each second for prob
        }

        public override void OnTransition(StateController stController)
        {
            RestartValues();
        }

        public override bool TransitionCondition(StateController stController)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) //Test for chance
            {
                RestartValues();
                if (UnityEngine.Random.Range(0.0f, 1.0f) < probPerSecond)
                    return true;
            }
            return false;
        }
    }
}