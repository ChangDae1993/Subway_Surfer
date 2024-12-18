using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RankSetting : MonoBehaviour
{
    public List<Text> names = new List<Text>();
    public List<Text> scores = new List<Text>();

    public void SettingRank(string combinedData)
    {

        // '|'로 결합된 문자열을 분리
        string[] data = combinedData.Split('|');

        if (data.Length < 2)
        {
            Debug.LogError("Data format error: Expected two parts separated by '|'.");
            return;
        }

        string[] nameArray = data[0].Split(',');
        string[] scoreArray = data[1].Split(',');

        // 이름과 점수 처리
        for (int i = 0; i < nameArray.Length; i++)
        {
            if (i < names.Count)
            {  
                // names와 scores의 리스트 크기를 확인하여 범위 초과 방지
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
