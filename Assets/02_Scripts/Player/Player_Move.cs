using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 moveVec;

    public float speed = 5f;
    public float verticalVelocity = 0f;
    [SerializeField] private float gravity = 12.0f;

    public Camera_Script CamS;
    public Score_script scoreS;

    public float startTime;

    //private float animationDur = 3.0f;

    public bool isDead = false;

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (scoreS == null)
            scoreS = GetComponent<Score_script>();
        

        if (CamS == null)
            CamS = GetComponent<Camera_Script>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Time.time - startTime < CamS.animationDur)
        {
            controller.Move(Vector3.forward * Time.deltaTime * speed);
            return;
        }

        moveVec = Vector3.zero;


        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        //X = left and right
        moveVec.x = Input.GetAxisRaw("Horizontal") * speed;
        //Y = Up and Down

        moveVec.y = verticalVelocity;

        //Z = Forward and Backward
        moveVec.z = speed;


        controller.Move(moveVec * Time.deltaTime * speed);
    }


    public void SetSpeed(float modifier)
    {
        speed = speed + modifier;
    }

    //It is being called everytime our capsule hit something
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.point.z > transform.position.z + controller.radius)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        scoreS.OnDeath();
        Debug.Log("Death");
    }
}
