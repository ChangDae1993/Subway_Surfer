using System.Collections;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Player_Move : MonoBehaviour
{
    [Header("Move")]
    public Vector3 moveVec;
    public float speed;
    public float startTime;
    public Left_Right_CHKR dirChKR;


    [Header("Camera")]
    public Camera_Script CamS;
    [Header("Score")]
    public Score_script scoreS;



    [Header("Player")]
    public bool isDead = false;
    public Rigidbody rb;
    public GameObject playerModel;
    public Animator animator;

    private void Awake()
    {
        //if (controller == null)
        //    controller = GetComponent<CharacterController>();

        if (scoreS == null)
            scoreS = GetComponent<Score_script>();
        

        if (CamS == null)
            CamS = GetComponent<Camera_Script>();

        if(rb == null)
            rb = GetComponent<Rigidbody>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }


    public bool turnInput = false;
    Coroutine turn;

    Vector3 dir = Vector3.forward;


    [Header("Pause Menu")]
    public bool PauseMenuOn = true;
    public Image pauseMenuPanel;
    // Update is called once per frame

    [Header("Tutorial")]
    public bool tutorialOn = false;
    public Image TutorialPanel;

    void Update()
    {
        if (isDead)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!PauseMenuOn)
            {
                PauseMenuOn = true;
                pauseMenuPanel.gameObject.SetActive(true);
                Debug.Log(PauseMenuOn);
            }
            else
            {
                Debug.Log("Off");
                PauseMenuOn = false;
                pauseMenuPanel.gameObject.SetActive(false);
            }
        }


        // Horizontal input을 받아서 좌우 이동
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // 기본적으로 moveVec.x는 0으로 초기화 (위치 이동을 위해)
        moveVec.x = 0;

        if (horizontalInput != 0)
        {
            // 왼쪽으로 이동 시 Raycast 확인
            if (horizontalInput < 0 && dirChKR.CanMoveLeft())
            {
                moveVec.x = -speed;
            }
            // 오른쪽으로 이동 시 Raycast 확인
            else if (horizontalInput > 0 && dirChKR.CanMoveRight())
            {
                moveVec.x = speed;
            }
        }

        if (dirChKR.CanTurnMoveForword()/* || dirChKR.CanTurnMoveBackword()*/)
        {
            moveVec.z = speed;
        }
        else
        {
            moveVec.z = 0f;
        }


        // Z 방향 (전방) 이동
        //moveVec.z = speed;


        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Roll();
        }
        //Y = Up and Down
        //dir.y = verticalVelocity;

        if(rb.linearVelocity.y < -20f)
        {
            //떨어져서 죽음
            Debug.Log("fall death");
            DeathType = death.fall;
            Death(DeathType);
        }

        //Z = Forward and Backward
        dir.z = speed;


        if (Time.time - startTime < CamS.animationDur)
        {
            // 실제 이동
            rb.MovePosition(rb.position + transform.TransformDirection(moveVec) * Time.deltaTime);
            return;
        }

        if(!tutorialOn)
        {
            //Debug.Log("Stop");
            if(!TutorialPanel.gameObject.activeSelf)
            {
                TutorialPanel.gameObject.SetActive(true);
            }

            if(Input.anyKey)
            {
                tutorialOn = true;
            }

            Time.timeScale = 0f;
            return;
        }
        else
        {
            if (TutorialPanel.gameObject.activeSelf)
            {
                TutorialPanel.gameObject.SetActive(false);
            }

            if (!PauseMenuOn)
                Time.timeScale = 1f;
        }

        if(!PauseMenuOn)
        {
            // 실제 이동
            rb.MovePosition(rb.position + transform.TransformDirection(moveVec) * Time.deltaTime);

            if (!turnInput)  //rigidbody rotate Y는 고정해서 이 외에 물리 값 받지 않도록
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (turn != null)
                    {
                        StopCoroutine(turn);
                        turn = StartCoroutine(turnleftCo());
                    }
                    else
                    {
                        turn = StartCoroutine(turnleftCo());
                    }

                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (turn != null)
                    {
                        StopCoroutine(turn);
                        turn = StartCoroutine(turnrightCo());
                    }
                    else
                    {
                        turn = StartCoroutine(turnrightCo());
                    }

                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    //점프로 간다 이건
                    //rb.AddForce((Vector3.up * 150f),ForceMode.Impulse);

                    Debug.Log("추가 아이디어 구현 필요");
                }
            }
        }
      
        LayerMask mask = LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Wall");
        Vector3 look = transform.TransformDirection(Vector3.forward); // Local -> World
        Debug.DrawRay(transform.position + Vector3.up, look * dirChKR.rayZLength, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.forward, out hit, dirChKR.rayZLength, mask))
        {
            //기타 오브젝트에 닿아서 죽었을 때
            // Ray에 충돌이 감지되면 충돌된 오브젝트의 이름을 출력
            //Debug.Log($"Raycast {hit.collider.gameObject.name}!");
            //Debug.Log("impact death");
            DeathType = death.crash;
            Death(DeathType);
        }
    }



#region run and turn

    public void Roll()
    {
        AudioManager.AM.PlaySfx(AudioManager.Sfx.roll);
        animator.Play("roll");
        animator.SetFloat("rollSpeed", (speed * 0.4f));
        //Debug.Log("Roll");
    }


    private Vector3 currentDirection = Vector3.forward; // 초기 이동 방향 설정 (기본적으로 z 방향)

    //왼쪽 회전
    public IEnumerator turnleftCo()
    {
        //Debug.Log("왼쪽 회전");
        turnInput = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, -90, 0);

        float elapsedTime = 0f;
        float duration = 1f;  // 회전 시간 (초 단위)

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration); // 부드러운 회전
            elapsedTime += Time.deltaTime * (0.5f *speed);
            yield return null;
        }
        transform.rotation = endRotation;  // 정확한 90도 회전으로 마무리
        currentDirection = transform.forward; // 회전 후 진행 방향을 forward로 설정
        turnInput = false;
    }


    //오른쪽 회전
    public IEnumerator turnrightCo()
    {
        //Debug.Log("오른쪽 회전");
        turnInput = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 90, 0);  // 오른쪽 90도 회전

        float elapsedTime = 0f;
        float duration = 1f;  // 회전 시간 (초 단위)

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration); // 부드러운 회전
            elapsedTime += Time.deltaTime * (0.5f * speed);
            yield return null;
        }

        transform.rotation = endRotation;  // 정확한 90도 회전으로 마무리
        currentDirection = transform.forward; // 회전 후 진행 방향을 forward로 설정
        turnInput = false;
    }


    public void SetSpeed(float modifier)
    {
        speed += modifier;
    }
#endregion


#region die

    public enum death
    {
        crash,
        crash_crane,
        water,
        fall
    }
    [Header("Die")]
    public death DeathType;

    void Death(death type)
    {
        AudioManager.AM.PlayBGM(false);
        AudioManager.AM.PlayAmbience(false);


        if (die != null)
        {
            StopCoroutine(die);
            die = StartCoroutine(deathCo(type));
        }
        else
        {
            die = StartCoroutine(deathCo(type));
        }
    }

    Coroutine die;
    IEnumerator deathCo(death type)
    {
        isDead = true;
        switch (type)
        {
            case death.crash:
                animator.Play("crash");
                AudioManager.AM.PlaySfx(AudioManager.Sfx.crash);
                yield return new WaitForSeconds(0.5f);
                break;
            case death.crash_crane:
                animator.Play("crash_crane");
                AudioManager.AM.PlaySfx(AudioManager.Sfx.impactCrane);
                yield return new WaitForSeconds(0.5f);
                break;
            case death.water:
                animator.Play("slip");
                AudioManager.AM.PlaySfx(AudioManager.Sfx.enterWater);
                yield return new WaitForSeconds(0.5f);
                break;
            case death.fall:
                AudioManager.AM.PlaySfx(AudioManager.Sfx.falling);
                animator.Play("fall");
                break;
            default:
                break;
        }
        scoreS.OnDeath();
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle_Ground"))
        {
            //물에 닿아서 죽었을 때
            DeathType = death.water;
            Death(DeathType);

            Debug.Log("Enter water");
        }
        else if (collision.gameObject.CompareTag("Obstacle_Crane"))
        {
            //크레인에 닿아서 죽었을 때
            DeathType = death.crash_crane;
            Death(DeathType);

            Debug.Log("impact Crane");
        }
    }

#endregion
}
