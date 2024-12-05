using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject LightLTr;
    public Light[] Llights;
    Coroutine lightLBlink;

    public GameObject LightRTr;
    public Light[] Rlights;
    Coroutine lightRBlink;

    private float blinkTime = 0.3f;

    [Space(10f)]
    [Header("Animation")]
    public bool isAnimPattern;

    [SerializeField] private Player_Move player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player_Move>();
        generateVec = new Vector3(0f, 0.3f, 0f);

        if (LightLTr != null)
        {
            //Debug.Log("LightL On");
            Llights = LightLTr.GetComponentsInChildren<Light>();
        }

        if (LightRTr != null)
        {
            //Debug.Log("LightR On");
            Rlights = LightRTr.GetComponentsInChildren<Light>();
        }
    }


    //���� �ɶ�, Ȥ�� ObjPool���� ���� ��
    private void OnEnable()
    {

        for (int i = Llights.Length - 1; i >= 0; i--)
        {
            Llights[i].color = Color.blue;
        }


        for (int i = Rlights.Length - 1; i >= 0; i--)
        {
            Rlights[i].color = Color.blue;
        }

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
                Llights[i].color =Color.red;
                yield return new WaitForSeconds(blinkTime);
                Llights[i].color = Color.blue;
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
                Rlights[i].color = Color.red;
                yield return new WaitForSeconds(blinkTime);
                Rlights[i].color = Color.blue;
            }
            yield return null;
        }
    }



    //Destroy �� ��, Ȥ�� ObjPool�� ���ư� ��
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


    [Space(10f)]
    [Header("Anim Play")]
    public bool animShow;
    [SerializeField] private float maxDis = 50f;         //��ư ����� ������ ���� �Ÿ�
    //public float playerDis = 0f;        //�÷��̾�� �� Ÿ�� ���� �Ÿ�
    [SerializeField] private Vector3 showTargetVec;
    public GameObject targetImage = null;    //Ÿ�� image or somethig
    public float imageScale = 0f;       //Ÿ�� image or something�� ũ��
    // Update is called once per frame
    void Update()
    {
        if (isAnimPattern)
        {
            showTargetVec = new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y, 15f);

            float distanceSqr = (this.transform.position - player.gameObject.transform.position + showTargetVec).sqrMagnitude;    //�÷��̾�� Ÿ�ϰ��� �Ÿ�
            if (distanceSqr > 48f * 48f)
                return;

            if(!animShow)
            {
                //PerformAnimationLogic(Mathf.Sqrt(distanceSqr));
                StartCoroutine(animationShowCo());
            }
        }
    }
    //void PerformAnimationLogic(float distance)
    //{
    //    animShow = true;
    //    Debug.Log("���� ����");
    //    Debug.Log(distance);
    //    imageScale = 0f;

    //    imageScale = (maxDis - distance) * 0.01f;

    //    if (targetImage.transform.localScale.x <= 0.8f)
    //    {
    //        targetImage.transform.localScale = new Vector3(imageScale, imageScale, imageScale);
    //    }
    //    else
    //    {
    //        targetImage.gameObject.SetActive(false);
    //    }
    //}

    IEnumerator animationShowCo()
    {
        animShow = true;
        imageScale = 0f;
        while (imageScale < 0.8f)
        {
            targetImage.transform.localScale = new Vector3(imageScale, imageScale, imageScale);
            imageScale += 0.01f * player.speed;
            yield return new WaitForSeconds(0.1f);
        }

        targetImage.gameObject.SetActive(false);
    }
}
