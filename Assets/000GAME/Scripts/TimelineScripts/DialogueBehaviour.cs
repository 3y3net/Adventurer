using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
    public string characterName;
    public string dialogueLine;
	public Color dialogueColor=Color.white;

	public bool hasToPause = false;

	private bool clipPlayed = false;
	private bool pauseScheduled = false;
	private PlayableDirector director;
	private TextManager textManager;
    public int uniqueId = -1;

    public override void OnPlayableCreate(Playable playable)
	{
		director = (playable.GetGraph().GetResolver() as PlayableDirector);
		textManager = GameObject.FindObjectOfType<TextManager> ();
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if(!clipPlayed && info.weight > 0f)
		{
			//UIManager.Instance.SetDialogue(characterName, dialogueLine, dialogueSize);
            if(uniqueId!=-1 && LocalizableData.instance!=null)
                dialogueLine=LocalizableData.instance.languageText[uniqueId];
            textManager.DisplayMessage (dialogueLine, dialogueColor, 0);
			if(Application.isPlaying)
			{
				if(hasToPause)
				{
					pauseScheduled = true;
				}
			}

			clipPlayed = true;
		}
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if(pauseScheduled)
		{
			pauseScheduled = false;
			//UIManager.Instance.PauseTimeline(director);
		}
		else
		{
			//UIManager.Instance.ClearDialogue ();
			textManager.DisplayMessage ("", dialogueColor, 0);
		}

		clipPlayed = false;
	}
}