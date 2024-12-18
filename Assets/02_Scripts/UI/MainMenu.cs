using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //[Header("Score")]
    //public Text highScore;

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
    private int currentIndex = 0;   // ���� ���� �ε���
    private int nextIndex = 1;      // ���� ���� �ε���
    private float t = 0f;           // ���� ���൵


    private Material skyboxMaterial;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!introCanvas.gameObject.activeSelf)
        {
            introCanvas.gameObject.SetActive(true);
        }
        //PlayerPrefs.SetFloat("Highscore", 0f);
        //highScore.text = "HighScore : " + (int)PlayerPrefs.GetFloat("Highscore");


        NetworkManager.NM.ConnectStart();

        AudioManager.AM.PlayBGM(false);


        // ���� Ȱ��ȭ�� Skybox�� ã�Ƽ� �ش� Material�� �����ɴϴ�.
        skyboxMaterial = RenderSettings.skybox;
        skyboxMaterial.SetFloat("_AtmosphereThickness", 0.66f);
    }

    // Update is called once per frame
    void Update()
    {
        if (titleText == null) 
            return;

        // ���� ����
        t += Time.deltaTime * titleCHNGSpeed;
        titleText.color = Color.Lerp(colors[currentIndex], colors[nextIndex], t);

        // ������ ������ ���� �������� �Ѿ
        if (t >= 1f)
        {
            t = 0f;
            currentIndex = nextIndex;
            nextIndex = (nextIndex + 1) % colors.Length; // ��ȯ ó��
        }
    }

    [Space(20f)]
    [Header("Game Start Direction")]
    public Canvas introCanvas;
    Coroutine gameStart;
    public Animator introEnemyAnim;
    public Animator introBGAnim;
    public bool animationStart;
    public Animator camDirection;
    public intro_Camera_Script camSc;
    public Image sceneChangePanel;
    public Animator introPlayer;

    public void ToGame()
    {
        if (introCanvas.gameObject.activeSelf)
        {
            introCanvas.gameObject.SetActive(false);
        }

        //Debug.Log("start");
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


    float panelAlpha = 0f;
    IEnumerator startGameCo()
    {
        AudioManager.AM.PlayBGM(true);
        if (!animationStart)
        {
            //Debug.Log("wait");
            animationStart = true;
            introEnemyAnim.Play("react");
            introBGAnim.SetBool("animOut", true);
            camDirection.SetBool("dirStart", true);
            introPlayer.SetBool("playerOn", true);
        }

        while(!camSc.sceneCHNGReady)
        {
            yield return null;
        }


        float fadeSpeed = 1.0f; // ���� ���� �ӵ�
        while (sceneChangePanel.color.a < 1f)
        {
            Color currentColor = sceneChangePanel.color;
            sceneChangePanel.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a + Time.deltaTime * fadeSpeed);
            yield return null;
        }

        SceneManager.LoadScene("Game");
    }

    [Header("Option Menu")]
    public Image OptionMenuPanel;
    public void ToOption()
    {
        if(!OptionMenuPanel.gameObject.activeSelf)
        {
            OptionMenuPanel.gameObject.SetActive(true);
        }
        //Debug.Log("OptionOn");
    }

    public void ToRanking()
    {
        if(!NetworkManager.NM.ranking_Panel.activeSelf)
        {
            NetworkManager.NM.ranking_Panel.gameObject.SetActive(true);
            NetworkManager.NM.ShowRank();
        }
    }

    public void ToExit()
    {
        Application.Quit();
    }
}
