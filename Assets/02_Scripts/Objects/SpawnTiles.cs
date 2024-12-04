using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public enum TileType
    {
        left,
        right, 
        up, 
        down,
        upY,
        flat,
    }

    public Transform[] objSpawnPoint;

    public GameObject[] obstacleList;

    public TileType tileType;

    public Vector3 generateVec;

    //���� �ɶ�, Ȥ�� ObjPool���� ���� ��
    private void OnEnable()
    {
        GameObject obstacle;
        if(objSpawnPoint.Length != 0 && obstacleList.Length != 0)
        {
            for(int i = 0; i < objSpawnPoint.Length; i++)
            {
                obstacle = Instantiate(obstacleList[Random.Range(0, obstacleList.Length)]) as GameObject;
                obstacle.transform.SetParent(objSpawnPoint[i]);
                obstacle.transform.localPosition = new Vector3(0f, 0.3f, 0f);
                //Debug.Log(obstacle.transform.localPosition);
            }
        }
    }


    //Destroy �� ��, Ȥ�� ObjPool�� ���ư� ��
    private void OnDisable()
    {
        if (objSpawnPoint != null)
        {

        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateVec = new Vector3(0f, 0.3f, 0f);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
