using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class Score_script : MonoBehaviour
{
    private float score = 0f;


    public int Level = 1;
    public int maxLevel = 10;
    private int scoreToNextLevel = 10;

    public Text score_txt;

    public Player_Move playerM;

    public DeathMenu deathMenu;

    private void Awake()
    {
        if(playerM == null)
        {
            playerM.GetComponent<Player_Move>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //score_txt.text = "agadgjvakljgas";
    }

    // Update is called once per frame
    void Update()
    {
        if(playerM.isDead)
        {
            return;
        }


        if(score >= scoreToNextLevel)
        {
            LevelUp();
        }

        score += Time.deltaTime * Level;
        score_txt.text = ((int)score).ToString();
    }

    void LevelUp()
    {

        if(Level == maxLevel)
        {
            return;
        }
        Debug.Log("Level Up");

        scoreToNextLevel *= 2;
        Level++;

        playerM.SetSpeed(Level);

    }

    public void OnDeath()
    {
        deathMenu.ToggleEndMenu(score);
    }
}
