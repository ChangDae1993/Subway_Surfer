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
            // Ray�� �浹�� �����Ǹ� �浹�� ������Ʈ�� �̸��� ���
            Debug.Log($"Raycast {hit.collider.gameObject.name}!");
            Death();
        }
    }
    public float rayLength;



    private Vector3 currentDirection = Vector3.forward; // �ʱ� �̵� ���� ���� (�⺻������ z ����)

    public IEnumerator turnleftCo()
    {
        //Debug.Log("���� ȸ��");
        turnInput = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, -90, 0);

        float elapsedTime = 0f;
        float duration = 1f;  // ȸ�� �ð� (�� ����)

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration); // �ε巯�� ȸ��
            elapsedTime += Time.deltaTime * (speed);
            yield return null;
        }
        transform.rotation = endRotation;  // ��Ȯ�� 90�� ȸ������ ������
        currentDirection = transform.forward; // ȸ�� �� ���� ������ forward�� ����
        turnInput = false;
    }

    public IEnumerator turnrightCo()
    {
        //Debug.Log("������ ȸ��");
        turnInput = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 90, 0);  // ������ 90�� ȸ��

        float elapsedTime = 0f;
        float duration = 1f;  // ȸ�� �ð� (�� ����)

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration); // �ε巯�� ȸ��
            elapsedTime += Time.deltaTime * (speed);
            yield return null;  // �� ������ ���
        }

        transform.rotation = endRotation;  // ��Ȯ�� 90�� ȸ������ ������
        currentDirection = transform.forward; // ȸ�� �� ���� ������ forward�� ����
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
