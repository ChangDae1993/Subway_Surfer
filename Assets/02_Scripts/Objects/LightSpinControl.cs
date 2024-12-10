using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightSpinControl : MonoBehaviour
{
    public Transform directinoalLight;
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
        Quaternion targetRotation = Quaternion.Euler(0f, directinoalLight.transform.eulerAngles.y, directinoalLight.transform.eulerAngles.z);

        while (Quaternion.Angle(directinoalLight.transform.rotation, targetRotation) > 0.1f)
        {
            Debug.Log("Spin");
            directinoalLight.transform.rotation = Quaternion.Lerp(directinoalLight.transform.rotation, targetRotation, Time.deltaTime * 0.003f);
            yield return null; // 매 프레임마다 업데이트
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
