using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource FXSource;
    public AudioClip fireworks1;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayFireworks()
    {
        FXSource.clip = fireworks1;
        FXSource.Play();
    }
}
