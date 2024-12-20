using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public Text scoreText;
    //public Text newhighScoreText;
    public float TextCHNGSpeed = 1f;

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


    public Image backgroundImg;

    public bool isShowned = false;

    private float transition = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //scoreText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isShowned)
        {
            return;
        }
        transition += Time.deltaTime;
        backgroundImg.color = Color.Lerp(new Color(0,0,0,0), Color.black, transition);

        if(scoreText.gameObject.activeSelf)
        {
            // ���� ����
            t += Time.deltaTime * TextCHNGSpeed;
            scoreText.color = Color.Lerp(colors[currentIndex], colors[nextIndex], t);

            // ������ ������ ���� �������� �Ѿ
            if (t >= 1f)
            {
                t = 0f;
                currentIndex = nextIndex;
                nextIndex = (nextIndex + 1) % colors.Length; // ��ȯ ó��
            }
        }
    }

    public void ToggleEndMenu(float score)
    {
        isShowned = true;
        gameObject.SetActive(true);
        //if ((int)score >= (int)PlayerPrefs.GetFloat("Highscore"))
        //{
        //    newhighScoreText.gameObject.SetActive(true);
        //}
        scoreText.text = ((int)score).ToString();
    }

    public void Restart()
    {
        AudioManager.AM.PlayBGM(true);
        AudioManager.AM.PlayAmbience(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        AudioManager.AM.PlayAmbience(true);
        SceneManager.LoadScene("Menu");
    }
}
