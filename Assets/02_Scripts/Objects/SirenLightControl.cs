using System.Collections;
using UnityEngine;

public class SirenLightControl : MonoBehaviour
{
    public Light leftSiren;
    public Light rightSiren;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SirenOn()
    {
        StartCoroutine(sirenStartCo());
    }

    public IEnumerator sirenStartCo()
    {
        while (true)
        {
            leftSiren.color = Color.blue;
            rightSiren.color = Color.blue;
            yield return new WaitForSeconds(0.25f);
            leftSiren.color = Color.red;
            rightSiren.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            yield return null;
        }
    }
}
