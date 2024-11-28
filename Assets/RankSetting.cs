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
        string[] nameArray = data[0].Split(',');
        string[] scoreArray = data[1].Split(',');

        // 이름과 점수 처리
        for (int i = 0; i < nameArray.Length; i++)
        {
            names[i].text = nameArray[i];
            scores[i].text = scoreArray[i];
        }
    }
}
