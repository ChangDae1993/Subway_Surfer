using UnityEngine;
using static AudioManager;

public class AudioManager : MonoBehaviour
{

    public static AudioManager AM;

    [Header("BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx
    {
        bulldoze_appear,
        obstacleDown,
        vehicle_beep,
        water_spoil,
        footstep,

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
}
