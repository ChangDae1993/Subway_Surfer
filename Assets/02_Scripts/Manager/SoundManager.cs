using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager SM;

    [Header("Audio Source")]
    public AudioSource bgmSource;
    public AudioSource ambianceSource; // 환경음
    public AudioSource sfxSourcePrefab; // SFX용 AudioSource 프리팹

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;


    [Header("SFX Clips")]
    public List<AudioClip> sfxClips;
    private Dictionary<string, AudioClip> sfxDictionary;


    private void Awake()
    {
        if (SM == null)
        {
            SM = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // SFX Dictionary 초기화
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips)
        {
            sfxDictionary[clip.name] = clip;
        }

        // 이전 설정 불러오기
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    // Update is called once per frame
    private void Update()
    {
        // 실시간 볼륨 반영
        //bgmSource.volume = bgmVolume;
    }

    public void PlaySFX(string clipName)
    {
        if (sfxDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            PlaySFX(clip);
        }
        else
        {
            Debug.LogWarning($"효과음 '{clipName}'을(를) 찾을 수 없습니다.");
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource newSfxSource = Instantiate(sfxSourcePrefab, transform);
            newSfxSource.volume = sfxVolume;
            newSfxSource.PlayOneShot(clip);
            Destroy(newSfxSource.gameObject, clip.length); // 재생 후 삭제
        }
    }


    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("ABCVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public void FadeOutBGM(float duration)
    {
        StartCoroutine(FadeOutCoroutine(bgmSource, duration));
    }


    private IEnumerator FadeOutCoroutine(AudioSource source, float duration)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // 초기화
    }
}
