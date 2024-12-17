using UnityEngine;

public class Left_Right_CHKR : MonoBehaviour
{
    [Header("Raycast Parameters")]
    public float rayLength;  // Ray 길이 설정
    public LayerMask obstacleLayer;  // 장애물 레이어
    public float rayZLength;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    public bool CanTurnMoveForword()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up;
        Vector3 rayDirection = transform.forward;  // 왼쪽 방향 (회전된 방향 기준으로 왼쪽)
        if (Physics.Raycast(transform.position + Vector3.up, rayDirection, out hit, rayLength, obstacleLayer))
        {
            Debug.Log("앞충돌");
            Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.blue);
            // Ray가 장애물과 충돌했을 때
            return false;
        }

        // 충돌하지 않으면 Ray 길이를 녹색으로 표시
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
        return true; // 충돌하지 않으면 이동 가능
    }

    // 왼쪽으로 이동할 수 있는지 확인하는 함수
    public bool CanMoveLeft()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up;
        Vector3 rayDirection = -transform.right;  // 왼쪽 방향 (회전된 방향 기준으로 왼쪽)
        if (Physics.Raycast(transform.position + Vector3.up, rayDirection, out hit, rayLength, obstacleLayer))
        {
            Debug.Log("왼충돌");
            Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);
            // Ray가 장애물과 충돌했을 때
            return false;
        }

        // 충돌하지 않으면 Ray 길이를 녹색으로 표시
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
        return true; // 충돌하지 않으면 이동 가능
    }

    // 오른쪽으로 이동할 수 있는지 확인하는 함수
    public bool CanMoveRight()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up;  // Ray 시작 지점
        Vector3 rayDirection = transform.right;  // 오른쪽 방향 (회전된 방향 기준으로 오른쪽)
        if (Physics.Raycast(transform.position + Vector3.up, rayDirection, out hit, rayLength, obstacleLayer))
        {
            Debug.Log("오충돌");
            Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);
            return false;
        }
        // 충돌하지 않으면 Ray 길이를 녹색으로 표시
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
        return true; // 충돌하지 않으면 이동 가능
    }
}
