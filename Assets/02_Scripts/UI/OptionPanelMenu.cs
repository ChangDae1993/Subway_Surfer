using UnityEngine;

public class OptionPanelMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBGMVolume(float volume)
    {
        SoundManager.SM.bgmVolume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SoundManager.SM.sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
