using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudioSource : MonoBehaviour
{
    [Header("Audio Clip")]
    public AudioClip clip;
    public float fadeInTime;

    [Header("Distance Culling")]
    public Transform listener;
    public float maxDistance;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;

        listener = GameObject.FindGameObjectWithTag("Player").transform;

        if(clip != null)
        {
            audioSource.clip = clip;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayAmbient());
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, listener.position);

        if (audioSource.isPlaying)
        {
            audioSource.enabled = distance <= maxDistance;
            
        }
    }

    public IEnumerator PlayAmbient()
    {
        if(fadeInTime > 0f)
        {
            yield return StartCoroutine(FadeIn());
        }

        audioSource.Play();
    }

    public IEnumerator FadeIn()
    {
        float timer = 0f;
        audioSource.volume = 0f;

        while(timer < fadeInTime)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            yield return null;
        }

        audioSource.volume = 1f;
    }
}
