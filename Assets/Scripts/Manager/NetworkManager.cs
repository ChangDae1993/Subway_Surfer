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
        ////�̰� jslib�� ���úκ�
        //Debug.Log("�غ� ����");
        //WebSocketSetting();

    }


    private void Update()
    {
        //Debug.Log("�غ� ����");
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Debug.Log("setting ����");
        //    //�̰� jslib�� ���úκ�
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