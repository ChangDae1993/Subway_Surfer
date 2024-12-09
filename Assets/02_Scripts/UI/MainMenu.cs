using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Score")]
    public Text highScore;

    [Header("Title Text")]
    public Text titleText;
    public float titleCHNGSpeed = 1f;

    private Color[] colors = {
        new Color(0, 0, 1),   // Blue
        new Color(1, 0, 1),   // Magenta
        new Color(1, 0, 0),   // Red
        new Color(1, 1, 0),   // Yellow
        new Color(0, 1, 0),   // Green
        new Color(0, 1, 1)    // Cyan
    };
    private int currentIndex = 0;   // 현재 색상 인덱스
    private int nextIndex = 1;      // 다음 색상 인덱스
    private float t = 0f;           // 보간 진행도

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayerPrefs.SetFloat("Highscore", 0f);
        highScore.text = "HighScore : " + (int)PlayerPrefs.GetFloat("Highscore");
    }

    // Update is called once per frame
    void Update()
    {
        if (titleText == null) return;

        // 색상 보간
        t += Time.deltaTime * titleCHNGSpeed;
        titleText.color = Color.Lerp(colors[currentIndex], colors[nextIndex], t);

        // 보간이 끝나면 다음 색상으로 넘어감
        if (t >= 1f)
        {
            t = 0f;
            currentIndex = nextIndex;
            nextIndex = (nextIndex + 1) % colors.Length; // 순환 처리
        }
    }

    Coroutine gameStart;
    public Animator introEnemyAnim;
    public void ToGame()
    {
        //introEnemyAnim.SetBool("introStart", true);
        //gameStart = StartCoroutine(startGameCo());
        if (gameStart != null)
        {
            StopCoroutine(gameStart);
            gameStart = StartCoroutine(startGameCo());
        }
        else
        {
            gameStart = StartCoroutine(startGameCo());
        }
    }

    IEnumerator startGameCo()
    {
        introEnemyAnim.SetBool("introStart", true);
        introEnemyAnim.SetBool("introStart", false);
        yield return new WaitForSeconds(5f);
        Debug.Log("wait");

        SceneManager.LoadScene("Game");
        yield return null;
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
