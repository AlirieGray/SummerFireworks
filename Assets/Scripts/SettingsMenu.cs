using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider FXSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void UpdateFxVolume()
    {

        AudioManager.manager.FXSource.volume = FXSlider.value;
    }

    public void UpdateMusicVolume()
    {
        AudioManager.manager.musicSource.volume = musicSlider.value;
    }
}
