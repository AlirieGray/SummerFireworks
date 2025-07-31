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

    public void PlayFireworks()
    {
        FXSource.clip = fireworks1;
        StartCoroutine(CountdownFireworks());
    }

    public void PlayTheme()
    {
        musicSource.clip = mainTheme;
        musicSource.Play();
    }

    IEnumerator CountdownFireworks()
    {
        yield return new WaitForSeconds(0.7f);
        FXSource.Play();
    }
}