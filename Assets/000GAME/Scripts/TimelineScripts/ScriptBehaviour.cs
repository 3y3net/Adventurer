using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.AI;

[Serializable]
public class ScriptBehaviour : PlayableBehaviour
{
	public enum Action { Destination, Parameter }
	public Action action;

	public Vector3 destination;
	public string parameter;
	public int value;
	public int ActivateIndex;
	private bool clipPlayed = false;

	private PlayableDirector director;

	public override void OnPlayableCreate(Playable playable)
	{
		director = (playable.GetGraph().GetResolver() as PlayableDirector);
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if(!clipPlayed && info.weight > 0f)
		{
			//Play Clip
			switch (action) {
				case Action.Destination:
				//PlayerManager.Instance.SetDestination (destination);
					break;
			case Action.Parameter:
					//PlayerManager.Instance.SetParameter (parameter, value);
					break;
            }

			clipPlayed = true;
		}
	}
}