using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class NetworkManager : MonoBehaviour
{
    public Text test;
    public int score;
    [DllImport("__Internal")]
    private static extern void SendScore(int score);
    [DllImport("__Internal")]
    private static extern void WebSocketSetting();
    private void Start()
    {
        ////이게 jslib의 세팅부분
        //Debug.Log("준비 시작");
        //WebSocketSetting();

    }


    private void Update()
    {
        //Debug.Log("준비 시작");
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Debug.Log("setting 제발");
        //    //이게 jslib의 세팅부분
        //    WebSocketSetting();
        //}


        if (Input.GetKeyDown(KeyCode.Space))
        {

            score++;
            test.text += score.ToString();
            SendScore(score);
        }
    }

    public void ConnectStart()
    {
        WebSocketSetting();
    }

}