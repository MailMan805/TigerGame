using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverFade : MonoBehaviour
{
    public float fadeSpeed = 0.2f;

    private Canvas canvas;
    private Image panelImage;

    private float time = 0f;

    const float OPAQUE = 1f;
    const float TRANSPARENT = 0f;

    const float MAX_TIME = 1f;

    const float DOUBLE_SPEED = 2f;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        panelImage = GetComponentInChildren<Image>();

        GameManager.instance.OnMainLevelLoaded.AddListener(FadeInScreen);
        GameManager.instance.OnHouseLevelLoaded.AddListener(FadeInScreen);
        

        GameManager.instance.OnDeath.AddListener(FadeOutScreen);
        GameManager.instance.LeaveHouse.AddListener(FadeOutScreen);
        GameManager.instance.LeaveLevel.AddListener(FadeOutScreen);
    }

    void FadeOutScreen()
    {
        print("Fading out...");
        StopAllCoroutines();
        StartCoroutine(Fader(TRANSPARENT, OPAQUE, DOUBLE_SPEED));
    }

    void FadeInScreen()
    {
        print("Fading in...");
        StopAllCoroutines();
        StartCoroutine(Fader(OPAQUE, TRANSPARENT));
    }

    IEnumerator Fader(float startingAlpha, float endingAlpha, float multiplier = 1f)
    {
        time = 0f;
        canvas.enabled = true;
        Color newAlpha = panelImage.color;
        newAlpha.a = startingAlpha;

        panelImage.color = newAlpha;

        while (time < MAX_TIME)
        {
            newAlpha.a = Mathf.Lerp(startingAlpha, endingAlpha, time);
            IncrementTime(multiplier);

            panelImage.color = newAlpha;

            yield return null;
        }

        newAlpha.a = endingAlpha;
        panelImage.color = newAlpha;

        if(newAlpha.a == 0f) { canvas.enabled = false; }

        yield return null;
    }

    void IncrementTime(float multiplier=1f)
    {
        time += fadeSpeed * Time.deltaTime * multiplier;

        if (time > 1f)
        {
            time = 1f;
        }
    }

    public void InstantBlackout() {
        Color newAlpha = panelImage.color;
        newAlpha.a = 1;

        panelImage.color = newAlpha;
    }
}
