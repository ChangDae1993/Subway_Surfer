using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Text pauseTxt;
    [SerializeField] private bool isBlinking;
    public Image score_bg;
    public Text score_txt;
    public Score_script score_Script;
    public Player_Move player_move;

    public Button resumeBtn;
    public Button exitBtn;
    public Text countDownTxt;

    private void Awake()
    {
        if (AudioManager.AM.bgmEffect == null)
        {
            AudioManager.AM.bgmEffect = Camera.main.GetComponent<AudioLowPassFilter>();
        }
        this.gameObject.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Debug.Log(Time.unscaledDeltaTime);
    //}

    private void OnEnable()
    {
        if(AudioManager.AM.bgmEffect != null)
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
            //Debug.Log("Blink in");
            pauseTxt.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f); // ±ôºýÀÓ °£°Ý
            pauseTxt.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.5f); // ±ôºýÀÓ °£°Ý
        }
    }

    public void OnResume()
    {
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
        //panel ²ô±â
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("Menu");
    }

}
