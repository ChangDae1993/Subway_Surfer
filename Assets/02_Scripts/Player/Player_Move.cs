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
    [HideInInspector] public bool PauseMenuOn = false;
    public Image pauseMenuPanel;
    // Update is called once per frame
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
                pauseMenuPanel.gameObject.SetActive(PauseMenuOn);
            }
            else
            {
                PauseMenuOn = false;
                pauseMenuPanel.gameObject.SetActive(PauseMenuOn);
            }
        }

        //X = left and right
        dir.x = Input.GetAxisRaw("Horizontal") * speed;

        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Roll();
        }
        //Y = Up and Down
        //dir.y = verticalVelocity;

        if(rb.linearVelocity.y < -20f)
        {
            //�������� ����
            Debug.Log("fall death");
            DeathType = death.fall;
            Death(DeathType);
        }

        //Z = Forward and Backward
        dir.z = speed;


        if (Time.time - startTime < CamS.animationDur)
        {
            rb.MovePosition(rb.position + transform.TransformDirection(dir) * (speed * Time.deltaTime));
            return;
        }

        rb.MovePosition(rb.position + transform.TransformDirection(dir) * (speed * Time.deltaTime));

        if(!turnInput)  //rigidbody rotate Y�� �����ؼ� �� �ܿ� ���� �� ���� �ʵ���
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
                Debug.Log("�߰� ���̵�� ���� �ʿ�");
            }
        }


        LayerMask mask = LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Wall");
        Vector3 look = transform.TransformDirection(Vector3.forward); // Local -> World
        Debug.DrawRay(transform.position + Vector3.up, look * rayLength, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.forward, out hit, rayLength, mask))
        {
            //��Ÿ ������Ʈ�� ��Ƽ� �׾��� ��
            // Ray�� �浹�� �����Ǹ� �浹�� ������Ʈ�� �̸��� ���
            Debug.Log($"Raycast {hit.collider.gameObject.name}!");
            Debug.Log("impact death");
            DeathType = death.crash;
            Death(DeathType);
        }
    }

    public float rayLength;

#region run and turn
    public void Roll()
    {
        animator.Play("roll");
        animator.SetFloat("rollSpeed", speed);
        //Debug.Log("Roll");
    }


    private Vector3 currentDirection = Vector3.forward; // �ʱ� �̵� ���� ���� (�⺻������ z ����)

    //���� ȸ��
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


    //������ ȸ��
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
        if(die != null)
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
                yield return new WaitForSeconds(0.5f);
                break;
            case death.crash_crane:
                animator.Play("crash_crane");
                yield return new WaitForSeconds(0.5f);
                break;
            case death.water:
                animator.Play("slip");
                yield return new WaitForSeconds(0.5f);
                break;
            case death.fall:
                animator.Play("fall");
                //yield return new WaitForSeconds(0.5f);
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
            //���� ��Ƽ� �׾��� ��
            DeathType = death.water;
            Death(DeathType);
            Debug.Log("Enter water");
        }
        else if (collision.gameObject.CompareTag("Obstacle_Crane"))
        {
            //Debug.Log("impact Crane");
            DeathType = death.crash_crane;
            //ũ���ο� ��Ƽ� �׾��� ��
            Death(DeathType);
        }
    }

#endregion
}
