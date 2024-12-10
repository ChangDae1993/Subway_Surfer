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
        while (/*currentRotation.x > 180f || */currentRotation.x > -90f)
        {
            //Debug.Log("Spin");

            // x축 회전을 조금씩 감소
            currentRotation.x -= 0.05f;

            // 변경된 오일러 각도를 적용
            directinoalLight.transform.rotation = Quaternion.Euler(currentRotation);

            // 회전 값이 특정 값 이하로 도달하면 추가 조건 실행
            if (currentRotation.x <= 5f) // 예: x축 회전이 10도 이하일 때
            {
                tileManager.lightOn = true;
            }

            yield return new WaitForSeconds(0.1f); // 대기 시간 설정
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
