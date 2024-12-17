using System.Xml.Linq;
using UnityEngine;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AM;
    [Header("Ambience")]
    public AudioClip abcClip;
    public float abcVolume;
    AudioSource abcPlayer;


    [Header("BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    [HideInInspector] public AudioLowPassFilter bgmEffect = null;


    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx
    {
        bulldoze_appear = 0,
        obstacleDown = 1,
        vehicle_beep = 2,
        water_spoil = 3,
        footstep,
        impactCrane = 7,
        enterWater = 8,
        crash = 9,
        falling = 10,
        roll = 11,

    }

    private void Awake()
    {
        if (AM == null)
        {
            AM = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
    }

    void Init()
    {
        GameObject abcObject = new GameObject("AmbiencePlayer");
        abcObject.transform.parent = this.transform;
        abcPlayer = abcObject.AddComponent<AudioSource>();
        abcPlayer.playOnAwake = false;
        abcPlayer.loop = true;
        abcPlayer.volume = abcVolume;
        abcPlayer.clip = abcClip;

        PlayAmbience(true);

        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = this.transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = this.transform;
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < channels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
            sfxPlayers[i].bypassListenerEffects = true;
        }

    }

    private void Update()
    {
        abcPlayer.volume = abcVolume;
        bgmPlayer.volume = bgmVolume;

        for (int i = 0; i < channels; i++)
        {
            sfxPlayers[i].volume = sfxVolume;
            sfxPlayers[i].bypassListenerEffects = true;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int i = 0; i < channels; i++)
        {
            int loopindex = (i + channelIndex) % channels;

            if (sfxPlayers[loopindex].isPlaying)
                continue;

            int randomIndex = 0;
            if(sfx == Sfx.footstep)
            {
                randomIndex = Random.Range(0, 3);
            }


            channelIndex = loopindex;
            sfxPlayers[loopindex].clip = sfxClips[(int)sfx + randomIndex];
            sfxPlayers[loopindex].Play();
            break;
        }
    }

    public void PlayAmbience(bool isPlay)
    {
        if (isPlay)
        {
            abcPlayer.Play();
        }
        else
        {
            abcPlayer.Stop();
        }
    }

    public void PlayBGM(bool isPlay)
    {
        if(isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBGM(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

}
