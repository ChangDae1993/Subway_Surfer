using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightSpinControl : MonoBehaviour
{
    public Transform directinoalLight;
    public TileManager tileManager;
    Coroutine spin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spin == null)
        {
            spin = StartCoroutine(lightSpin());
        }
        else
        {
            StopCoroutine(spin);
            spin = StartCoroutine(lightSpin());
        }
    }

    IEnumerator lightSpin()
    {
        // ���� ������Ʈ�� ���Ϸ� ���� ��������
        Vector3 currentRotation = directinoalLight.transform.eulerAngles;

        // Ư�� ȸ�� �� ������ �� ���� �߰� (��: x�� ȸ���� 180�� ����)
        while (currentRotation.x > -90f)
        {
            currentRotation.x -= 0.05f;

            directinoalLight.transform.rotation = Quaternion.Euler(currentRotation);

            if (currentRotation.x <= 5f)
            {
                tileManager.lightOn = true;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
