using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EndingCutscene : BaseTimeline
{
    [SerializeField] TimelineAsset FinalCutsceneTimelineAsset;

    private void Awake()
    {
        GameManager.instance.GoodEnding.AddListener(Ending);
        GameManager.instance.BadEnding.AddListener(Ending);

        PlayTimeline();
    }

    public void Ending()
    {
        PlayTimeline(FinalCutsceneTimelineAsset);
    }

    private void OnDestroy()
    {
        GameManager.instance.GoodEnding.RemoveListener(Ending);
        GameManager.instance.BadEnding.RemoveListener(Ending);
    }
}
