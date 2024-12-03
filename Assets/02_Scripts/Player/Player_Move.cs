using UnityEngine;

public class Player_Move : MonoBehaviour
{
    //public CharacterController controller;
    public Vector3 moveVec;

    public float speed = 5f;
    public float verticalVelocity = 0f;
    [SerializeField] private float gravity = 12.0f;

    public Camera_Script CamS;
    public Score_script scoreS;

    public float startTime;

    //private float animationDur = 3.0f;

    public bool isDead = false;

    public Rigidbody rb;
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
            //var dir = Vector3.forward;

            //rb.MovePosition(rb.position + transform.TransformDirection(-dir) * (speed * Time.deltaTime));

            //controller.Move(Vector3.forward * Time.deltaTime * speed);
            return;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("왼쪽 회전");
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("오른쪽 회전");
        }

        rb.MovePosition(rb.position + transform.TransformDirection(dir) * (speed * Time.deltaTime));

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.TryGetComponent(out SpawnTiles tile))
    //    {
    //        if(tile.tileType == SpawnTiles.TileType.flat)
    //        {
    //            Debug.Log("return");
    //            Debug.Break();
    //            return;
    //        }
    //        else
    //        {
    //            Debug.Log(tile.tileType);
    //            Debug.Break();
    //        }
    //    }
    //}



    public void SetSpeed(float modifier)
    {
        speed = speed + modifier;
    }

    //It is being called everytime our capsule hit something
    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (!hit.gameObject.CompareTag("Obstacle"))
    //        return;

    //    if (Time.time - startTime > CamS.animationDur && hit.point.z > transform.position.z + controller.radius)
    //    {
    //        Debug.Log(hit.gameObject.name);
    //        Death();
    //    }
    //}

    void Death()
    {
        isDead = true;
        scoreS.OnDeath();
        Debug.Log("Death");
    }
}
