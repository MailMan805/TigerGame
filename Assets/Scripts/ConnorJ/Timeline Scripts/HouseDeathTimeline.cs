using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DeathTimeline : BaseTimeline
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnHouseLevelLoaded.AddListener(PlayDeathTimeline);
    }

    void PlayDeathTimeline()
    {
        if (GameManager.DiedInLevel)
        {
            PlayTimeline();
        }
    }

    private void OnDisable()
    {
        GameManager.instance.OnHouseLevelLoaded.RemoveListener(PlayDeathTimeline);
    }
}
