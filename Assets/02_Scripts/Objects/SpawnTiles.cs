using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTiles : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    public enum TileType
    {
        left,
        right,
        up_Up,
        up_flat,
        up_Down,
        down_Down,
        down_flat,
        down_Up,
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

    private float blinkTime = 0.2f;

    public TileLampControl LampControl = null;

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
            Llights = LightLTr.GetComponentsInChildren<Light>();
        }

        if (LightRTr != null)
        {
            Rlights = LightRTr.GetComponentsInChildren<Light>();
        }

        if (tileManager == null)
        {
            tileManager = gameObject.GetComponentInParent<TileManager>();
        }
    }


    //생성 될때, 혹은 ObjPool에서 나올 때
    private void OnEnable()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player_Move>();
        }

        for (int i = Llights.Length - 1; i >= 0; i--)
        {
            Llights[i].color = Color.blue;
        }


        for (int i = Rlights.Length - 1; i >= 0; i--)
        {
            Rlights[i].color = Color.blue;
        }

        GameObject obstacle;
        if (objSpawnPoint.Length != 0 && obstacleList.Length != 0)
        {
            for (int i = 0; i < objSpawnPoint.Length; i++)
            {
                obstacle = Instantiate(obstacleList[UnityEngine.Random.Range(0, obstacleList.Length)]) as GameObject;
                obstacle.transform.SetParent(objSpawnPoint[i]);
                obstacle.transform.localPosition = new Vector3(0f, 0.3f, 0f);
            }
        }

        if (LightLTr != null)
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
        while (true)
        {
            for (int i = Llights.Length - 1; i >= 0; i--)
            {
                Llights[i].color = Color.red;
                yield return new WaitForSeconds(blinkTime);
                Llights[i].color = Color.blue;
            }
            yield return null;
        }

    }
    IEnumerator lightRBlinkCo()
    {
        while (true)
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

    private void OnDestroy()
    {
        if(player != null)
        {
            player = null;
        }

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


    public enum animType
    {
        waterSpoil,
        VehicleAppear,
        obstacleDown,
        leftRight,
        VehicleTweenAppear,
    }

    [Header("Anim Play")]
    public bool animShow;
    public animType animationType;
    [SerializeField] private Vector3 showTargetVec;
    public GameObject targetImage = null;    //타겟 image or somethig
    public float imageScale = 0f;       //타겟 image or something의 크기
    public ParticleSystem particle = null;
    [SerializeField] private bool leftRightbool;
    [SerializeField] private int leftRightIndex = 0;
    public bool reverseAnim = false;

    // Update is called once per frame
    void Update()
    {
        if (tileManager != null && tileManager.lightOn)
        {
            if(LampControl != null)
            {
                LampControl.LampLightOnOff(tileManager.lightOn);
            }
        }

        if (isAnimPattern)
        {
            showTargetVec = player.gameObject.transform.position;
            float distanceSqr = Vector3.Distance(this.transform.position, showTargetVec);    //플레이어와 타일간의 거리

            switch (animationType)
            {
                case animType.waterSpoil:

                    if (distanceSqr > 25f)
                        return;

                    if (!animShow)
                    {
                        StartCoroutine(animationShowCo());
                    }
                    break;
                case animType.VehicleAppear:

                    if (distanceSqr > 23f)
                        return;

                    if (!animShow)
                    {
                        vehicleShow();
                    }
                    break;
                case animType.obstacleDown:

                    if (distanceSqr > 33f)
                        return;

                    if (!animShow)
                    {
                        ObstacleDown();
                    }
                    break;
                case animType.leftRight:

                    if (distanceSqr > 55f)
                        return;

                    if (!animShow)
                    {
                        RandomLR();
                    }
                    break;

                case animType.VehicleTweenAppear:

                    if (distanceSqr > 23f)
                        return;

                    if (!animShow)
                    {
                        TweenVehicle();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void vehicleShow()
    {
        animShow = true;

        if(targetImage != null)
        {
            if (!targetImage.gameObject.activeSelf)
            {
                targetImage.gameObject.SetActive(true);
            }

            if(targetImage.gameObject.TryGetComponent(out Animator anim))
            {
                if(!reverseAnim)
                {
                    if (targetImage.gameObject.name.Equals("appear"))
                    {
                        AudioManager.AM.PlaySfx(AudioManager.Sfx.bulldoze_appear);
                        anim.SetBool("appear", true);
                    }
                    else if (targetImage.gameObject.name.Equals("appear_move"))
                    {
                        AudioManager.AM.PlaySfx(AudioManager.Sfx.vehicle_beep);
                        anim.SetBool("appear_move", true);
                        anim.SetFloat("appear_speed", (player.speed * 0.1f));
                    }
                }
                else
                {
                    if (targetImage.gameObject.name.Equals("appear"))
                    {
                        AudioManager.AM.PlaySfx(AudioManager.Sfx.bulldoze_appear);
                        anim.SetBool("appear_reverse", true);
                    }
                    else if (targetImage.gameObject.name.Equals("appear_move"))
                    {
                        AudioManager.AM.PlaySfx(AudioManager.Sfx.vehicle_beep);
                        anim.SetBool("appear_move_reverse", true);
                    }
                }

            }
        }
    }

    public void TweenVehicle()
    {
        animShow = true;

        if (targetImage != null)
        {
            if (!targetImage.gameObject.activeSelf)
            {
                targetImage.gameObject.SetActive(true);
            }

            if (targetImage.gameObject.TryGetComponent(out Animator anim))
            {
                if (targetImage.gameObject.name.Equals("tween_move"))
                {
                    AudioManager.AM.PlaySfx(AudioManager.Sfx.bulldoze_appear);
                    anim.SetBool("tween_move", true);
                    anim.SetFloat("appear_speed", (player.speed * 0.1f));
                }
                else if (targetImage.gameObject.name.Equals("appear_tween"))
                {
                    AudioManager.AM.PlaySfx(AudioManager.Sfx.vehicle_beep);
                    anim.SetBool("appear_move", true);
                }
            }
        }
    }

    IEnumerator animationShowCo()
    {
        if (particle != null && !particle.gameObject.activeSelf)
            particle.gameObject.SetActive(true);

        animShow = true;

        if(targetImage != null)
        {
            AudioManager.AM.PlaySfx(AudioManager.Sfx.water_spoil);
            imageScale = 0f;
            while (imageScale < 7.5f)
            {
                targetImage.transform.localScale = new Vector3(imageScale, targetImage.transform.localScale.y, imageScale);
                imageScale += 0.05f * (player.speed * 0.5f);
                yield return new WaitForSeconds(0.05f);
            }

            targetImage.transform.localScale = new Vector3(imageScale, targetImage.transform.localScale.y, imageScale);
        }
    }


    public void ObstacleDown()
    {
        animShow = true;

        if (targetImage != null)
        {
            if (targetImage.gameObject.TryGetComponent(out Animator anim))
            {
                AudioManager.AM.PlaySfx(AudioManager.Sfx.obstacleDown);
                anim.SetBool("down", true);
                anim.SetFloat("downSpeed", (player.speed * 0.3f));
            }
        }
    }

    public void RandomLR()
    {
        animShow = true;

        leftRightIndex = UnityEngine.Random.Range(0, 2);
        leftRightbool = Convert.ToBoolean(leftRightIndex);

        if(targetImage != null)
        {
            if(!targetImage.gameObject.activeSelf)
            {
                targetImage.gameObject.SetActive(true);

                if (targetImage.gameObject.TryGetComponent(out Animator anim))
                {
                    AudioManager.AM.PlaySfx(AudioManager.Sfx.crane_leftright);
                    anim.SetBool("isLeft", leftRightbool);
                }
            }

        }
    }
}
