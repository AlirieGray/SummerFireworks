using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager manager;
    public AudioSource musicSource;
    public AudioSource FXSource;
    public AudioClip fireworks1;
    public AudioClip mainTheme;
    public AudioClip collectResource;
    public AudioClip nicePling;
    public AudioClip chomp;
    public AudioClip grind;

    void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (manager != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        PlayTheme();
    }

    public void PlayGrind()
    {
        FXSource.clip = grind;
        FXSource.Play();
    }

    public void PlayFireworks()
    {
        StartCoroutine(CountdownFireworks());
    }

    IEnumerator CountdownFireworks()
    {
        yield return new WaitForSeconds(0.7f);
        FXSource.PlayOneShot(fireworks1);
    }
    public void PlayTheme()
    {
        musicSource.clip = mainTheme;
        musicSource.Play();
    }

    public void PlayFireworkNow()
    {
        FXSource.clip = fireworks1;
        FXSource.Play();
    }

    public void PlayMonsterChomp()
    {
        FXSource.clip = chomp;
        FXSource.Play();
    }

    public void PlayCollectResource()
    {
        FXSource.clip = collectResource;
        FXSource.Play();
    }

    public void PlayNicePling()
    {
        FXSource.clip = nicePling;
        FXSource.Play();
    }
}