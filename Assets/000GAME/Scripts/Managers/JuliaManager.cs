using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class JuliaManager : MonoBehaviour
{
    public static JuliaManager Instance;
    public PlayerMovement Player;
	public Animator animator;

    void Awake()
    {
        Instance = this;
    }

	public void SetDestination(Vector3 dest) {
		Player.ForceDestination (dest);
	}

	public void SetParameter(string name, int value) {
		animator.SetInteger (name, value);
	}
}