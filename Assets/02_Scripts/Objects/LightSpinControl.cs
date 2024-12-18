using System.Collections;
using UnityEngine;

public class LightSpinControl : MonoBehaviour
{
    [Header("Directional Light Setting")]
    public Transform directinoalLight;
    public TileManager tileManager;

    Coroutine spin;

    [Header("SkyBox Setting")]
    public float targetThickness;  // ��ǥ�� �� �β�
    public float changeSpeed;  // ��ȭ �ӵ�
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

        // ���� Ȱ��ȭ�� Skybox�� ã�Ƽ� �ش� Material�� �����ɴϴ�.
        skyboxMaterial = RenderSettings.skybox;
        skyboxMaterial.SetFloat("_AtmosphereThickness", 0.66f);
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
    void Update()
    {
        if (skyboxMaterial != null)
        {
            // ���� �� ��������
            float currentThickness = skyboxMaterial.GetFloat("_AtmosphereThickness");

            // �� ���� ������Ű��
            if (currentThickness < targetThickness)
            {
                currentThickness += changeSpeed * Time.deltaTime; // ���� �ð��� ���� ����
                skyboxMaterial.SetFloat("_AtmosphereThickness", currentThickness);
            }
        }
    }
}
