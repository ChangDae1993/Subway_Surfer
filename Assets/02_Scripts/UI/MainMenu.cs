using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text highScore;
    public Text titleText;

    Coroutine title;
    public float colorChngVal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayerPrefs.SetFloat("Highscore", 0f);
        highScore.text = "HighScore : " + (int)PlayerPrefs.GetFloat("Highscore");

        if(title != null)
        {
            StopCoroutine(title);
            title = StartCoroutine(titleStart());
        }
        else
        {
            title = StartCoroutine(titleStart());
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}


    IEnumerator titleStart()
    {
        //while (titleText.color.r <= 1f)
        //{
        //    titleText.color = new Color32(255, 255, 255, 255);
        //    yield return null;
        //}
        yield return null;
    }


    public void ToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ToOption()
    {
        Debug.Log("OptionOn");
    }

    public void ToRanking()
    {
        SceneManager.LoadScene("Ranking");
    }

    public void ToExit()
    {
        Application.Quit();
    }
}
