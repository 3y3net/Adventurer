using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyWatcher : MonoBehaviour {

    public GameStates state;
    public bool value;
    public bool fullReverse;
    public GameObject obj;
    public bool enable;

    GameState gameState;

    void Start()
    {
        gameState = FindObjectOfType<GameState>();
        if (!gameState)
            throw new UnityException("GameState could not be found, ensure that it exists in the Persistent scene.");
    }

    void Update()
    {
        if (gameState.gameStates[(int)state] == value && obj.activeSelf != enable)
            obj.SetActive(enable);
        if (gameState.gameStates[(int)state] != value && obj.activeSelf == enable && fullReverse)
            obj.SetActive(!enable);
    }
}
