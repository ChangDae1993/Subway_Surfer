using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Camera mainCam;
    public Text pauseTxt;
    [SerializeField] private bool isBlinking;
    [Header("Score")]
    public Image score_bg;
    public Text score_txt;
    public Score_script score_Script;

    [Header("Option")]
    public Scrollbar bgmVolume;
    public Scrollbar sfxVolume;

    [Space(10f)]
    public Player_Move player_move;

    public Button resumeBtn;
    public Button exitBtn;
    public Text countDownTxt;

    private void Awake()
    {
        AudioManager.AM.bgmEffect = mainCam.GetComponent<AudioLowPassFilter>();

        this.gameObject.SetActive(false);
    }
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

        if (AudioManager.AM.bgmEffect != null)
        {
            AudioManager.AM.EffectBGM(true);
        }

        Time.timeScale = 0f;
        StartCoroutine(BlinkText());
        score_bg.gameObject.SetActive(false);

        resumeBtn.gameObject.SetActive(true);
        exitBtn.gameObject.SetActive(true);
        score_txt.gameObject.SetActive(true);
        score_txt.text = "Score : " + ((int)score_Script.score).ToString();

        countDownTxt.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if(AudioManager.AM.bgmEffect != null)
        {
            AudioManager.AM.EffectBGM(false);
        }

        StopAllCoroutines();
        if (player_move.PauseMenuOn)
            player_move.PauseMenuOn = false;
        Time.timeScale = 1f;
        isBlinking = false;
        score_bg.gameObject.SetActive(true);
    }

    private IEnumerator BlinkText()
    {
        isBlinking = true;
        while (isBlinking)
        {
            pauseTxt.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f); // ±ôºýÀÓ °£°Ý
            pauseTxt.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.5f); // ±ôºýÀÓ °£°Ý
        }
    }

    public void OnResume()
    {
        PlayerPrefs.SetFloat("ABCVolume", AudioManager.AM.abcVolume);
        PlayerPrefs.SetFloat("BGMVolume", AudioManager.AM.bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", AudioManager.AM.sfxVolume);
        PlayerPrefs.Save();


        score_txt.gameObject.SetActive(false);
        resumeBtn.gameObject.SetActive(false);
        exitBtn.gameObject.SetActive(false);

        countDownTxt.gameObject.SetActive(true);
        StartCoroutine(countDown());
    }

    IEnumerator countDown()
    {
        int Timer = 10;
        int time = 5;
        while(Timer - time > 0f)
        {
            countDownTxt.text = (Timer - time).ToString();
            time++;
            yield return new WaitForSecondsRealtime(1f); // ±ôºýÀÓ °£°Ý
        }

        //panel ²ô±â
        this.gameObject.SetActive(false);
    }
    
    public void OnExit()
    {
        PlayerPrefs.SetFloat("ABCVolume", AudioManager.AM.abcVolume);
        PlayerPrefs.SetFloat("BGMVolume", AudioManager.AM.bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", AudioManager.AM.sfxVolume);
        PlayerPrefs.Save();

        //panel ²ô±â
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("Menu");
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
