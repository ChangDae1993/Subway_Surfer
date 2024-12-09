using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Name_Input : MonoBehaviour
{
    public Text infoTxt;
    public Text[] nameFields;  // 3���� UI �ؽ�Ʈ �ʵ�
    private int currentIndex = 0;  // ���� �Է� ���� ĭ
    private bool isBlinking = false; // ������ ����
    private string playerName = ""; // �÷��̾� �̸� ����

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
            yield return new WaitForSeconds(0.5f); // ������ ����
        }
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (currentIndex >= nameFields.Length) return; // ��� �Է��� ������ ����

        foreach (char c in Input.inputString) // �Էµ� ���� ó��
        {
            if (char.IsLetter(c)) // ���ĺ��� ���
            {
                nameFields[currentIndex].text = c.ToString().ToUpper();
                currentIndex++;
                ResetCursor();
            }
            else if (c == '\b' && currentIndex > 0) // �齺���̽� ó��
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
            // ���� üũ: currentIndex�� �迭 ũ�� �̻��̶�� �ڷ�ƾ ����
            if (currentIndex >= nameFields.Length)
            {
                isBlinking = false;
                yield break; // �ڷ�ƾ ����
            }

            nameFields[currentIndex].text = (nameFields[currentIndex].text == "_") ? "" : "_";
            yield return new WaitForSeconds(0.5f); // ������ ����
        }
    }

    private void FinishNameInput()
    {
        StopCoroutine(BlinkCursor());
        isBlinking = false;
        playerName = string.Join("", System.Array.ConvertAll(nameFields, field => field.text));
        Debug.Log($"Final Player Name: {playerName}");

        //�ٸ� ��ư �ѱ�
        PlayBtn.gameObject.SetActive(true);
        MenuBtn.gameObject.SetActive(true);
        // ���� ���� ���� �߰�
        SaveScore(playerName, scoreScript.score);
    }

    void SaveScore(string name, float score)
    {
        Debug.Log($"Name: {name}, Score: {score}");
        // ���� DB ����
    }
}
