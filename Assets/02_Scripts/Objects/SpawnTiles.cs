using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public enum TileType
    {
        left,
        right, 
        up, 
        upY,
        down,
        flat,
    }

    [Header("Tile Spawn Type")]
    public Transform[] objSpawnPoint;

    public GameObject[] obstacleList;

    public TileType tileType;

    public Vector3 generateVec;

    [Space(10f)]
    [Header("Light")]
    public Material redOn;
    public Material blueOn;

    public GameObject LightLTr;
    public MeshRenderer[] Llights;
    Coroutine lightLBlink;

    public GameObject LightRTr;
    public MeshRenderer[] Rlights;

    Coroutine lightRBlink;
    private float blinkTime = 0.5f;

    [Space(10f)]
    [Header("Animation")]
    public bool isAnimPattern;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateVec = new Vector3(0f, 0.3f, 0f);

        if (LightLTr != null)
        {
            Llights = LightLTr.GetComponentsInChildren<MeshRenderer>();
        }

        if (LightRTr != null)
        {
            Rlights = LightRTr.GetComponentsInChildren<MeshRenderer>();
        }
    }


    //생성 될때, 혹은 ObjPool에서 나올 때
    private void OnEnable()
    {
        GameObject obstacle;
        if(objSpawnPoint.Length != 0 && obstacleList.Length != 0)
        {
            for(int i = 0; i < objSpawnPoint.Length; i++)
            {
                obstacle = Instantiate(obstacleList[Random.Range(0, obstacleList.Length)]) as GameObject;
                obstacle.transform.SetParent(objSpawnPoint[i]);
                obstacle.transform.localPosition = new Vector3(0f, 0.3f, 0f);
            }
        }

        if(LightLTr != null)
        {
            if (lightLBlink != null)
            {
                StopCoroutine(lightLBlink);
                lightLBlink = StartCoroutine(lightLBlinkCo());
            }
            else
            {
                lightLBlink = StartCoroutine(lightLBlinkCo());
            }

        }


        if (LightRTr != null)
        {
            if (lightRBlink != null)
            {
                StopCoroutine(lightRBlink);
                lightRBlink = StartCoroutine(lightRBlinkCo());
            }
            else
            {
                lightRBlink = StartCoroutine(lightRBlinkCo());
            }
        }
    }

    IEnumerator lightLBlinkCo()
    {
        while(true)
        {
            for (int i = Llights.Length - 1; i >= 0; i--)
            {
                Llights[i].material = redOn;
                yield return new WaitForSeconds(blinkTime);
                Llights[i].material = blueOn;
            }
            yield return null;
        }

    }


    IEnumerator lightRBlinkCo()
    {
        while(true)
        {
            for (int i = Rlights.Length - 1; i >= 0; i--)
            {
                Rlights[i].material = redOn;
                yield return new WaitForSeconds(blinkTime);
                Rlights[i].material = blueOn;
            }
            yield return null;
        }
    }



    //Destroy 될 때, 혹은 ObjPool로 돌아갈 때
    private void OnDisable()
    {
        if (objSpawnPoint != null)
        {
            if (lightLBlink != null)
            {
                StopCoroutine(lightLBlink);
            }

            if (lightRBlink != null)
            {
                StopCoroutine(lightRBlink);
            }
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
