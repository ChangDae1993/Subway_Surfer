using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RankSetting : MonoBehaviour
{
    public List<Text> names = new List<Text>();
    public List<Text> scores = new List<Text>();

    public void SettingRank(string combinedData)
    {

        // '|'�� ���յ� ���ڿ��� �и�
        string[] data = combinedData.Split('|');

        if (data.Length < 2)
        {
            Debug.LogError("Data format error: Expected two parts separated by '|'.");
            return;
        }

        string[] nameArray = data[0].Split(',');
        string[] scoreArray = data[1].Split(',');

        // �̸��� ���� ó��
        for (int i = 0; i < nameArray.Length; i++)
        {
            if (i < names.Count)
            {  
                // names�� scores�� ����Ʈ ũ�⸦ Ȯ���Ͽ� ���� �ʰ� ����
                names[i].text = nameArray[i];
                scores[i].text = scoreArray[i];
                Debug.Log(names[i].text + " :: " + scores[i].text);
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
