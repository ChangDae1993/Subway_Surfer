using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public Text scoreText;
    //public Text newhighScoreText;

    public Image backgroundImg;

    public bool isShowned = false;

    private float transition = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //newhighScoreText.gameObject.SetActive(false);
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
    }

    public void ToggleEndMenu(float score)
    {
        isShowned = true;
        gameObject.SetActive(true);
        //if((int)score >= (int)PlayerPrefs.GetFloat("Highscore"))
        //{
        //    newhighScoreText.gameObject.SetActive(true);
        //    Debug.Log("new high Score");
        //}
        scoreText.text = ((int)score).ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
