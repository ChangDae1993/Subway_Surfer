using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reflection;
using System;
using UnityEngine.SocialPlatforms.Impl;
//using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager NM;


    public Text test;
    public int score;
    public Text RankingText;

    public GameObject ranking_Panel;

    [DllImport("__Internal")]
    private static extern void WebSocketSetting();


    [DllImport("__Internal")]
    private static extern void SendScore(string name, int score);

    public Text textval;
    [DllImport("__Internal")]
    private static extern void PrintNumber(string number);


    //[DllImport("__Internal")]
    //public static extern void RegisterScore();



    [DllImport("__Internal")]
    public static extern void ShowRanking();


    private void Awake()
    {
        if (NM == null)
        {
            NM = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {

    }


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        score++;
    //        test.text += score.ToString();
    //        SendScore(score);

    //        textval.text = "Hello WebSocket!";
    //        PrintNumber(textval.text);
    //    }
    //}

    public void scoreToServer(string name, int score)
    {
        Debug.Log($"[scoreToServer] Before SendScore - Name: {name}, Score: {score}");
        SendScore(name, score);
    }

    public void ShowRank()
    {
        Debug.Log("ShowRanking 호출!");
        ShowRanking();
    }

    public void ConnectStart()
    {
        WebSocketSetting();
    }
}