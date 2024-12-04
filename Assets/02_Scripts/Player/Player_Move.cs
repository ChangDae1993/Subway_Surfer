using System.Collections;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    //public CharacterController controller;
    public Vector3 moveVec;

    public float speed;

    public Camera_Script CamS;
    public Score_script scoreS;

    public float startTime;

    //private float animationDur = 3.0f;

    public bool isDead = false;

    public Rigidbody rb;
    public GameObject playerModel;
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

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        //X = left and right
        dir.x = Input.GetAxisRaw("Horizontal") * speed;

        //Y = Up and Down
        //dir.y = verticalVelocity;

        //Z = Forward and Backward
        dir.z = speed;


        if (Time.time - startTime < CamS.animationDur)
        {
            rb.MovePosition(rb.position + transform.TransformDirection(dir) * (speed * Time.deltaTime));
            return;
        }

        rb.MovePosition(rb.position + transform.TransformDirection(dir) * (speed * Time.deltaTime));

        if(!turnInput)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if(turn != null)
                {
                    StopCoroutine(turn);
                    turn = StartCoroutine(turnleftCo());
                }
                else
                {
                    turn = StartCoroutine(turnleftCo());
                }

            }

            if (Input.GetKeyDown(KeyCode.X))
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
        }


        LayerMask mask = LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Wall");
        Vector3 look = transform.TransformDirection(Vector3.forward); // Local -> World
        Debug.DrawRay(transform.position + Vector3.up, look * rayLength, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.forward, out hit, rayLength, mask))
        {
            // Ray에 충돌이 감지되면 충돌된 오브젝트의 이름을 출력
            Debug.Log($"Raycast {hit.collider.gameObject.name}!");
            Death();
        }
    }
    public float rayLength;



    private Vector3 currentDirection = Vector3.forward; // 초기 이동 방향 설정 (기본적으로 z 방향)

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
            elapsedTime += Time.deltaTime * (speed);
            yield return null;
        }
        transform.rotation = endRotation;  // 정확한 90도 회전으로 마무리
        currentDirection = transform.forward; // 회전 후 진행 방향을 forward로 설정
        turnInput = false;
    }

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
            elapsedTime += Time.deltaTime * (speed);
            yield return null;  // 한 프레임 대기
        }

        transform.rotation = endRotation;  // 정확한 90도 회전으로 마무리
        currentDirection = transform.forward; // 회전 후 진행 방향을 forward로 설정
        turnInput = false;
    }


    public void SetSpeed(float modifier)
    {
        speed = speed + modifier;
    }

    void Death()
    {
        isDead = true;
        scoreS.OnDeath();
        Debug.Log("Death");
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.CompareTag("Obstacle"))
    //    {
    //        Death();
    //    }
    //}
}
