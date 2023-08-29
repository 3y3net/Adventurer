using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD_GameManager
{
    public class GameGoalsList : PersistentData
    {
        public static GameGoalsList instance;

        public GameGoal[] gameGoals = new GameGoal[0];

        protected override void SpecificAwake()
        {
            instance = this;
        }

        protected override string SetKey()
        {
            return "GameGoals";
        }

        protected override void Save()
        {
            for (int i = 0; i < gameGoals.Length; i++)
            {

                string k = key + "_active_" + i;
                saveLoad.Save(k, gameGoals[i].active);
            }
        }

        protected override void Load()
        {
            bool state = false;
            for (int i = 0; i < gameGoals.Length; i++)
            {
                string k = key + "_active_" + i;
                if (saveLoad.Load(k, ref state))
                    gameGoals[i].active = state;
            }
        }
    }
}