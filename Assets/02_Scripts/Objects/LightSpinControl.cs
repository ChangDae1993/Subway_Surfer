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
        // 현재 오브젝트의 오일러 각도 가져오기
        Vector3 currentRotation = directinoalLight.transform.eulerAngles;

        // 특정 회전 값 이하일 때 조건 추가 (예: x축 회전이 180도 이하)
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
