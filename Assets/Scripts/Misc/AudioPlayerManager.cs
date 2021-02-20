using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlayerManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of songs to play in order")]
    private AudioClip[] playlist;

    public static AudioPlayerManager instance = null;
    private AudioSource src;
    private AudioLowPassFilter lp;
    private int trackNumber;
    private float[] trackCutoffs;
    private float lastCutoff;
    private float transition = 0.3f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        if (instance == this) return;
        Destroy(gameObject);
    }

    void Start()
    {
        trackCutoffs = new float[playlist.Length];
        trackCutoffs[0] = 21000f;
        for (int i = 1; i < playlist.Length; i++)
        {
            trackCutoffs[i] = 770f;
        }
        src = GetComponent<AudioSource>();
        lp = GetComponent<AudioLowPassFilter>();
        trackNumber = Random.Range(0, playlist.Length);
        src.clip = playlist[trackNumber];

        StartCoroutine(PlayNextTrack(src, lp));
    }

    /* Muffle the track to cutoff frequency f & return
     * the previous cutoff frequency. */
    public IEnumerator Muffle(float f)
    {
        float elapsed = 0;
        while (elapsed < transition)
        {
            elapsed += Time.deltaTime;
            lp.cutoffFrequency = Mathf.Lerp(lp.cutoffFrequency, f, elapsed / transition);
            yield return null;
        }
    }

    private bool audioNotPlaying()
    {
        return !src.isPlaying;
    }

    private IEnumerator PlayNextTrack(AudioSource src, AudioLowPassFilter lp)
    {
        while (true)
        {
            src.Play();
            while (src.isPlaying)
            {
                if (SceneManager.GetActiveScene().name == "SampleScene")
                {
                    StartCoroutine(Muffle(
                        Mathf.Min(
                            trackCutoffs[trackNumber] + (ScoreManager.singleton.GetMass() * 0.6f), 
                            22000)
                        )
                    );
                } 
                else
                {
                    StartCoroutine(Muffle(500f));
                }
                yield return new WaitForSeconds(0.75f);
            }

            StopCoroutine(Muffle(trackCutoffs[trackNumber]));
            trackNumber += 1;

            /* Loop to start */
            if (trackNumber >= playlist.Length)
            {
                trackNumber = 0;
            }
            
            /* Reset lowpass filter */
            lp.cutoffFrequency = trackCutoffs[trackNumber];
            src.clip = playlist[trackNumber];
        }

    }
}