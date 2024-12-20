using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Name_Input : MonoBehaviour
{
    public Text infoTxt;
    public Text[] nameFields;  // 3개의 UI 텍스트 필드
    private int currentIndex = 0;  // 현재 입력 중인 칸
    private bool isBlinking = false; // 깜빡임 상태
    private string playerName = ""; // 플레이어 이름 저장

    public Button PlayBtn;
    public Button MenuBtn;

    public Score_script scoreScript;
    void Start()
    {
        PlayBtn.gameObject.SetActive(false);
        MenuBtn.gameObject.SetActive(false);
        ResetFields();
        StartCoroutine(BlinkCursor());
    }

    private void OnEnable()
    {
        PlayBtn.gameObject.SetActive(false);
        MenuBtn.gameObject.SetActive(false);
        ResetFields();
        StartCoroutine(BlinkCursor());
        StartCoroutine(BlinkInfo());
    }

    IEnumerator BlinkInfo()
    {
        while (true)
        {
            infoTxt.text = (infoTxt.text == "Input Name Here") ? "" : "Input Name Here";
            yield return new WaitForSeconds(0.5f); // 깜빡임 간격
        }
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (currentIndex >= nameFields.Length) return; // 모든 입력이 끝나면 종료

        foreach (char c in Input.inputString) // 입력된 문자 처리
        {
            if (char.IsLetter(c)) // 알파벳만 허용
            {
                nameFields[currentIndex].text = c.ToString().ToUpper();
                currentIndex++;
                ResetCursor();
            }
            else if (c == '\b' && currentIndex > 0) // 백스페이스 처리
            {
                currentIndex--;
                nameFields[currentIndex].text = "_";
                ResetCursor();
            }

            if (currentIndex >= nameFields.Length)
            {
                FinishNameInput();
                return;
            }
        }
    }

    private void ResetFields()
    {
        foreach (Text field in nameFields)
        {
            field.text = "_";
        }
    }

    private void ResetCursor()
    {
        StopCoroutine(BlinkCursor());
        isBlinking = false;
        StartCoroutine(BlinkCursor());
    }

    private IEnumerator BlinkCursor()
    {
        isBlinking = true;
        while (isBlinking)
        {
            // 범위 체크: currentIndex가 배열 크기 이상이라면 코루틴 종료
            if (currentIndex >= nameFields.Length)
            {
                isBlinking = false;
                yield break; // 코루틴 종료
            }

            nameFields[currentIndex].text = (nameFields[currentIndex].text == "_") ? "" : "_";
            yield return new WaitForSeconds(0.5f); // 깜빡임 간격
        }
    }

    private void FinishNameInput()
    {
        StopCoroutine(BlinkCursor());
        isBlinking = false;
        playerName = string.Join("", System.Array.ConvertAll(nameFields, field => field.text));
        Debug.Log($"Final Player Name: {playerName}");

        if (string.IsNullOrEmpty(playerName) || playerName.All(char.IsDigit))
        {
            playerName = "AAA";
            Debug.Log("유효하지 않은 이름이 감지되어 기본값으로 설정합니다.");
        }

        Debug.Log($"Sending playerName: {playerName}");  // playerName 값 확인
        Debug.Log($"최종 이름: {playerName}");
        Debug.Log($"SaveScore 호출됨: 이름 - {playerName}, 점수 - {scoreScript.score}");


        //다른 버튼 켜기
        PlayBtn.gameObject.SetActive(true);
        MenuBtn.gameObject.SetActive(true);

        //SaveScore(playerName, scoreScript.score);
#if !UNITY_EDITOR
        NetworkManager.NM.scoreToServer(playerName, (int)scoreScript.score);
#endif
    }

    // 점수 저장 로직 추가
    //public void SaveScore(string name, float score)
    //{
    //    // 실제 DB 저장
    //    Debug.Log($"[SaveScore] Name: {name}, Score: {score}");
    //    NetworkManager.NM.scoreToServer(name,(int)score);
    //    //Debug.Log($"Name: {name}, Score: {score}");
    //}
}
