using UnityEngine;
using UnityEngine.UI;


public class Score_script : MonoBehaviour
{
    [HideInInspector] public float score = 0f;
    public float exp = 0f;


    public int Level = 1;
    //public int maxLevel = 10;
    [SerializeField] private float scoreToNextLevel = 10f;

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
        exp = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerM.isDead)
        {
            return;
        }


        if (exp >= scoreToNextLevel)
        {
            LevelUp();
        }

        exp += Time.deltaTime;
        score_txt.text = ((int)score).ToString();
    }

    void LevelUp()
    {
        scoreToNextLevel *= 1.2f;
        Level++;

        playerM.SetSpeed(0.5f);
    }

    public void OnDeath()
    {
        //점수 저장하기
        //if(PlayerPrefs.GetFloat("Highscore") < score)
        //{
        //    PlayerPrefs.SetFloat("Highscore", score);
        //}
        deathMenu.ToggleEndMenu(score);
    }
}
