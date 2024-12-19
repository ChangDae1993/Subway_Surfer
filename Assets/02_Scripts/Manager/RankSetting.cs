using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RankSetting : MonoBehaviour
{
    public List<Text> names = new List<Text>();
    public List<Text> scores = new List<Text>();
    public List<Text> times = new List<Text>(); // �ð� �����͸� ǥ���� UI ��� ����Ʈ

    public void SettingRank(string combinedData)
    {
        // '|'�� ���յ� ���ڿ��� �и�
        string[] data = combinedData.Split('|');

        if (data.Length < 3) // �̸�, ����, �ð� �����͸� �����ؾ� ��
        {
            Debug.LogError("Data format error: Expected three parts separated by '|'.");
            return;
        }

        string[] nameArray = data[0].Split(',');
        string[] scoreArray = data[1].Split(',');
        string[] timeArray = data[2].Split(',');

        // �̸�, ����, �ð� ó��
        for (int i = 0; i < nameArray.Length; i++)
        {
            if (i < names.Count && i < scores.Count && i < times.Count) // ���� �ʰ� ����
            {
                names[i].text = nameArray[i];
                scores[i].text = scoreArray[i];
                times[i].text = timeArray[i]; // �ð� ǥ��
                Debug.Log($"{names[i].text} :: {scores[i].text} :: {times[i].text}");
            }
            else
            {
                Debug.LogWarning("More entries in the ranking data than UI elements.");
            }
        }
    }

    public void ClosePanel()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }
}
