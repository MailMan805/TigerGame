using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class EndingCutscene : BaseTimeline
{
    [SerializeField] TimelineAsset FinalCutsceneTimelineAsset;
    public PlayableDirector PlayableDirector;

    private void Awake()
    {
        GameManager.instance.GoodEnding.AddListener(Ending);
        GameManager.instance.BadEnding.AddListener(Ending);

        PlayTimeline();
    }

    public void Ending()
    {
        Debug.Log("Ending");
        PlayableDirector.playableAsset = FinalCutsceneTimelineAsset;
        PlayableDirector.Play();
        StartCoroutine(wait());

    }

    private void OnDestroy()
    {
        GameManager.instance.GoodEnding.RemoveListener(Ending);
        GameManager.instance.BadEnding.RemoveListener(Ending);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene(0);
    }
}
