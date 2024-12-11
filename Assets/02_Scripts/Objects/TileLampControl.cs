using System.Collections.Generic;
using UnityEngine;

public class TileLampControl : MonoBehaviour
{
    //public Light[] lights;

    public Light leftLampLight = null;
    public Light rightLampLight = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(leftLampLight != null)
        {
            leftLampLight.gameObject.SetActive(false);
        }

        if(rightLampLight != null)
        {
            rightLampLight.gameObject .SetActive(false);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void LampLightOnOff(bool onoff)
    {
        if(onoff)
        {
            if (leftLampLight != null)
            {
                leftLampLight.gameObject.SetActive(true);
            }

            if (rightLampLight != null)
            {
                rightLampLight.gameObject.SetActive(true);
            }
        }
        else
        {
            if (leftLampLight != null)
            {
                leftLampLight.gameObject.SetActive(false);
            }

            if (rightLampLight != null)
            {
                rightLampLight.gameObject.SetActive(false);
            }
        }

    }
}
