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



    public Text textval;
    [DllImport("__Internal")]
    private static extern void PrintNumber(string number);


    [DllImport("__Internal")]
    private static extern void WebSocketSetting();
    private void Start()
    {

    }


    private void Update()
    {

#if !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {

            score++;
            test.text += score.ToString();
            SendScore(score);

            textval.text = "this is text.?!";
            PrintNumber(textval.text);
        }
#endif
        if (Input.GetKeyDown(KeyCode.Space))
        {

            score++;
            test.text += score.ToString();

            textval.text = "this is text.?!";
        }
    }

    public void ConnectStart()
    {
        WebSocketSetting();
    }

}