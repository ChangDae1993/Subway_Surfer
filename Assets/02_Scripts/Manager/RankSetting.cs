using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RankSetting : MonoBehaviour
{
    public List<Text> names = new List<Text>();
    public List<Text> scores = new List<Text>();
    public List<Text> times = new List<Text>(); // 시간 데이터를 표시할 UI 요소 리스트

    public void SettingRank(string combinedData)
    {
        // '|'로 결합된 문자열을 분리
        string[] data = combinedData.Split('|');

        if (data.Length < 3) // 이름, 점수, 시간 데이터를 포함해야 함
        {
            Debug.LogError("Data format error: Expected three parts separated by '|'.");
            return;
        }

        string[] nameArray = data[0].Split(',');
        string[] scoreArray = data[1].Split(',');
        string[] timeArray = data[2].Split(',');

        // 이름, 점수, 시간 처리
        for (int i = 0; i < nameArray.Length; i++)
        {
            if (i < names.Count && i < scores.Count && i < times.Count) // 범위 초과 방지
            {
                names[i].text = nameArray[i];
                scores[i].text = scoreArray[i];
                times[i].text = timeArray[i]; // 시간 표시
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
