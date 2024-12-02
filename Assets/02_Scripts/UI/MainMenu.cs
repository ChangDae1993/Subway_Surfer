using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text highScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highScore.text = "HighScore : " + (int)PlayerPrefs.GetFloat("Highscore");
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void ToGame()
    {
        SceneManager.LoadScene("Game");
    }
}
