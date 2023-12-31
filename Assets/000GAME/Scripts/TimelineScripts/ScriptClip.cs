using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ScriptClip : PlayableAsset, ITimelineClipAsset
{
	public ScriptBehaviour template = new ScriptBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
		var playable = ScriptPlayable<ScriptBehaviour>.Create (graph, template);
        
        return playable;
    }
}