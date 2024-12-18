using System.Collections;
using UnityEngine;

public class LightSpinControl : MonoBehaviour
{
    [Header("Directional Light Setting")]
    public Transform directinoalLight;
    public TileManager tileManager;

    Coroutine spin;

    [Header("SkyBox Setting")]
    public float targetThickness;  // 목표로 할 두께
    public float changeSpeed;  // 변화 속도
    private Material skyboxMaterial;

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

        // 현재 활성화된 Skybox를 찾아서 해당 Material을 가져옵니다.
        skyboxMaterial = RenderSettings.skybox;
        skyboxMaterial.SetFloat("_AtmosphereThickness", 0.66f);
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
    void Update()
    {
        if (skyboxMaterial != null)
        {
            // 현재 값 가져오기
            float currentThickness = skyboxMaterial.GetFloat("_AtmosphereThickness");

            // 값 점차 증가시키기
            if (currentThickness < targetThickness)
            {
                currentThickness += changeSpeed * Time.deltaTime; // 일정 시간에 따라 증가
                skyboxMaterial.SetFloat("_AtmosphereThickness", currentThickness);
            }
        }
    }
}
