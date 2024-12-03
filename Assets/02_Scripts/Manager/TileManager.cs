using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public GameObject[] Tiles;

    public List<GameObject> activeTiles = new List<GameObject>();

    private Transform playerTr;
    [SerializeField] private float spawnZ = -20f;


    private Vector3 currentDirection = Vector3.forward; // �⺻ ����
    private Vector3 currentPosition = Vector3.zero;     // Ÿ�� ���� ������
    private float tileLength = 16f;                    // Ÿ���� ����

    private int PretileObjNum = 7;

    private int lastPrefabIndex = 0;

    private float saveZone = 15f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTr = player.transform;
        }
        else
        {
            Debug.LogError("Player �±װ� �ִ� ������Ʈ�� ã�� �� �����ϴ�.");
        }
        //SpawnTile(0);

        for (int i = 0; i < PretileObjNum; i++)
        {
            if (i < 5)
            {
                SpawnTile(0);
            }
            else
            {
                SpawnTile();
            }
        }
    }


    public bool leftOn = false;
    public int leftCnt = 0;

    // Update is called once per frame
    void Update()
    {
        //{
        //    if (leftOn)
        //    {
        //        if (playerTr.transform.position.x - saveZone > (spawnZ - PretileObjNum * tileLength))
        //        {
        //            SpawnTile();
        //            //SpawnTile();
        //            DeletTile();
        //        }
        //    }
        //    else
        //    {
        //        if (playerTr.transform.position.z - saveZone > (spawnZ - PretileObjNum * tileLength))
        //        {
        //            SpawnTile();
        //            //SpawnTile();
        //            DeletTile();
        //        }
        //    }



        // �÷��̾ ���� ��ġ�� �Ѿ�� �� Ÿ�� ����
        if (playerTr.transform.position.x - saveZone > (spawnZ - PretileObjNum * tileLength))
        {
            SpawnTile();
            //SpawnTile();
            DeletTile();
        }


        // �÷��̾ ���� ��ġ�� �Ѿ�� �� Ÿ�� ����
        // ���⼭�� �׳� �׽�Ʈ�� ��� ����
        if (Input.GetKeyDown(KeyCode.Space)) // �׽�Ʈ��: SpaceŰ�� Ÿ�� ����
        {
            SpawnTile();
            DeletTile();
        }

        //Debug.Log(activeTiles[activeTiles.Count - 1].gameObject.transform.position.z);
    }


    void SpawnTile(int prefabIndex = -1)
    {
        GameObject tile;

        if (prefabIndex == -1)
        {
            tile = Instantiate(Tiles[RandomPrefabIndex()]) as GameObject;


            // Ÿ�� Ÿ�Կ� ���� ���� ������Ʈ
            if (tile.TryGetComponent(out SpawnTiles tileComponent))
            {
                currentDirection = GetNextDirection(tileComponent.tileType, tile);
            }
        }
        else
        {
            tile = Instantiate(Tiles[prefabIndex]);
        }

        // Ÿ���� ��ġ�� ���� ����
        tile.transform.SetParent(transform);
        tile.transform.position = currentPosition;
        tile.transform.rotation = Quaternion.LookRotation(currentDirection); // ���⿡ ���� ȸ��
        activeTiles.Add(tile);

        // ���� Ÿ���� ������ ������Ʈ
        currentPosition += currentDirection * tileLength;
    }

    private Vector3 GetNextDirection(SpawnTiles.TileType tileType, GameObject st)
    {
        switch (tileType)
        {
            case SpawnTiles.TileType.left:
                //st.gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
                return Quaternion.Euler(0, -90, 0) * currentDirection;
            case SpawnTiles.TileType.right:
                return Quaternion.Euler(0, 90, 0) * currentDirection;
            case SpawnTiles.TileType.up:
                return Vector3.up;
            case SpawnTiles.TileType.down:
                return Vector3.down;
            case SpawnTiles.TileType.flat:
                return currentDirection;
            default:
                Debug.LogWarning($"Unknown TileType: {tileType}");
                return currentDirection;
        }
    }


    void DeletTile()
    {
        if (activeTiles.Count > 0)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }
        else
        {
            Debug.LogWarning("activeTiles ����Ʈ�� ��� �ֽ��ϴ�!");
        }
    }

    int RandomPrefabIndex()
    {
        if (Tiles.Length <= 1)
            return 0;

        int randomIndex = (lastPrefabIndex + Random.Range(1, Tiles.Length)) % Tiles.Length;
        while (randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, Tiles.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;
    }
}
