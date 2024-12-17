using UnityEngine;

public class Left_Right_CHKR : MonoBehaviour
{
    [Header("Raycast Parameters")]
    public float rayLength;  // Ray ���� ����
    public LayerMask obstacleLayer;  // ��ֹ� ���̾�
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
        Vector3 rayDirection = transform.forward;  // ���� ���� (ȸ���� ���� �������� ����)
        if (Physics.Raycast(transform.position + Vector3.up, rayDirection, out hit, rayLength, obstacleLayer))
        {
            Debug.Log("���浹");
            Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.blue);
            // Ray�� ��ֹ��� �浹���� ��
            return false;
        }

        // �浹���� ������ Ray ���̸� ������� ǥ��
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
        return true; // �浹���� ������ �̵� ����
    }

    // �������� �̵��� �� �ִ��� Ȯ���ϴ� �Լ�
    public bool CanMoveLeft()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up;
        Vector3 rayDirection = -transform.right;  // ���� ���� (ȸ���� ���� �������� ����)
        if (Physics.Raycast(transform.position + Vector3.up, rayDirection, out hit, rayLength, obstacleLayer))
        {
            Debug.Log("���浹");
            Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);
            // Ray�� ��ֹ��� �浹���� ��
            return false;
        }

        // �浹���� ������ Ray ���̸� ������� ǥ��
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
        return true; // �浹���� ������ �̵� ����
    }

    // ���������� �̵��� �� �ִ��� Ȯ���ϴ� �Լ�
    public bool CanMoveRight()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up;  // Ray ���� ����
        Vector3 rayDirection = transform.right;  // ������ ���� (ȸ���� ���� �������� ������)
        if (Physics.Raycast(transform.position + Vector3.up, rayDirection, out hit, rayLength, obstacleLayer))
        {
            Debug.Log("���浹");
            Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);
            return false;
        }
        // �浹���� ������ Ray ���̸� ������� ǥ��
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
        return true; // �浹���� ������ �̵� ����
    }
}
