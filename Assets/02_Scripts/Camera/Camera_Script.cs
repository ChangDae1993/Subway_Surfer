using UnityEngine;
using System.Collections;

public class Camera_Script : MonoBehaviour
{

    public Transform lookAt;
    private Vector3 camOffset;

    private Vector3 moveVec;

    [Header("Camera Y min/max")]
    public float CamMinY;
    public float CamMaxY;


    [SerializeField] private float transition = 0f;

    [HideInInspector] public float animationDur = 3.0f;

    private Vector3 animationOffSet = new Vector3(0f, 6.5f, 5f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transition = 0f;

        if (lookAt == null)
        {
            lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        }
        camOffset = transform.position - lookAt.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveVec = lookAt.position + camOffset;

        // Y축 높이 제한 적용
        //moveVec.y = Mathf.Clamp(moveVec.y, CamMinY, CamMaxY);

        if (transition <= 1.0f)
        {
            transform.position = Vector3.Lerp(moveVec + animationOffSet, moveVec, transition);
            transition += Time.deltaTime * 1 / animationDur;
            transform.LookAt(lookAt.position + Vector3.up);
        }
    }
}
