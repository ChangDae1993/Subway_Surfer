using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
//using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public Text test;
    public int score;

    [DllImport("__Internal")]
    private static extern void WebSocketSetting();


    [DllImport("__Internal")]
    private static extern void SendScore(int score);

    public Text textval;
    [DllImport("__Internal")]
    private static extern void PrintNumber(string number);


    //[DllImport("__Internal")]
    //public static extern void RegisterScore();

    [DllImport("__Internal")]
    public static extern void ShowRanking();

    private void Start()
    {

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            score++;
            test.text += score.ToString();
            SendScore(score);

            textval.text = "this is text.?!";
            PrintNumber(textval.text);
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ShowRanking();
        }


    }
    public void ConnectStart()
    {
        WebSocketSetting();
    }

}