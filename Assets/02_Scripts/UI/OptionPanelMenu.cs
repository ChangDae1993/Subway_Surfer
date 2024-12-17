using UnityEngine;
using UnityEngine.UI;

public class OptionPanelMenu : MonoBehaviour
{
    public Scrollbar bgmVolume;
    public Scrollbar sfxVolume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetBGMVolume(bgmVolume.value);
        SetSFXVolume(sfxVolume.value);
    }

    private void OnEnable()
    {
        bgmVolume.value = PlayerPrefs.GetFloat("BGMVolume");
        sfxVolume.value = PlayerPrefs.GetFloat("SFXVolume");
    }


    public void ClosePanel()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }


    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("ABCVolume", AudioManager.AM.abcVolume);
        PlayerPrefs.SetFloat("BGMVolume", AudioManager.AM.bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", AudioManager.AM.sfxVolume);
        PlayerPrefs.Save();

        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SetBGMVolume(float volume)
    {
        AudioManager.AM.bgmVolume = volume;
        AudioManager.AM.abcVolume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.SetFloat("ABCVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.AM.sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
