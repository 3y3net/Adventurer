using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImGameState : ReactionBase
{

    public enum Operation { none, set, add, substract, multiply, divide }

    public GameStates gameState;
    public bool stateCondition;
    public GameValues gameValue;
    Operation operation;
    public int value;

    GameState gs;

    protected override void SpecificInit()
    {
        gs = GameObject.FindObjectOfType<GameState>();
        if (!gs)
            throw new UnityException("GameState could not be found, ensure that it exists in the Persistent scene.");
    }

    protected override void ImmediateReaction()
    {        
        if (gameState != GameStates.Ignore)
            gs.gameStates[(int)gameState] = stateCondition;
        if (gameValue != GameValues.None)
        {
            switch(operation)
            {
                case Operation.set:
                    gs.gameValues[(int)gameValue] = value;
                    break;
                case Operation.add:
                    gs.gameValues[(int)gameValue] += value;
                    break;
                case Operation.substract:
                    gs.gameValues[(int)gameValue] -= value;
                    break;
                case Operation.multiply:
                    gs.gameValues[(int)gameValue] *= value;
                    break;
                case Operation.divide:
                    gs.gameValues[(int)gameValue] *= value;
                    break;
            }
        }
    }
}
