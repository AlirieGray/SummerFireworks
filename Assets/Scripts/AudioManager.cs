using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource FXSource;
    public AudioClip fireworks1;
    public AudioClip mainTheme;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
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