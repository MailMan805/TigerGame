using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// BaseTimeline can be used as an inheritor so that each timeline can have conditions.
/// To play the timeline in the inherited script, use PlayTimeline()
/// </summary>
public class BaseTimeline : MonoBehaviour
{
    /// <summary>
    /// TimelineAsset for the script to play. Able to be set within the inspector window.
    /// </summary>
    [SerializeField] [Tooltip("TimelineAsset for the script to play.")] protected TimelineAsset _TimelineAsset;
    /// <summary>
    /// The PlayableDirector Component that will play _TimelineAsset.
    /// </summary>
    protected PlayableDirector _Director;

    private void Awake()
    {
        _Director = GetComponent<PlayableDirector>();
        if (_Director != null && _TimelineAsset != null)
        {
            _Director.playableAsset = _TimelineAsset;
        }
    }

    /// <summary>
    /// Plays the TimelineAsset variable _TimelineAsset.
    /// </summary>
    protected void PlayTimeline()
    {
        if (_TimelineAsset == null || _Director == null) return;

        print("Playing Timeline " + _TimelineAsset.name);

        _Director.Play();
    }
}
