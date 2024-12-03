using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileManager : MonoBehaviour
{
    public GameObject[] Tiles;

    public List<GameObject> activeTiles = new List<GameObject>();

    private Transform playerTr;
    public Score_script playerLevel;
    public Player_Move playermove;
    [SerializeField] private float spawnZ = -20f;


    private Vector3 currentDirection = Vector3.forward; // �⺻ ����
    private Vector3 currentPosition = Vector3.zero;     // Ÿ�� ���� ������
    private float tileLength = 16f;                    // Ÿ���� ����

    private int PretileObjNum = 10;

    private int lastPrefabIndex = 0;

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

        if(playerLevel == null)
        {
            playerLevel = playerTr.gameObject.GetComponent<Score_script>();
        }

        if(playermove == null)
        {
            playermove = player.gameObject.GetComponent<Player_Move>();
        }

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

    Vector3 lastTilePosition = Vector3.zero;

    private float tileMaker = 0f;  // Ÿ�� ���� Ÿ�̸�

    // Update is called once per frame
    void Update()
    {
        if (playermove.isDead)
        {
            return;
        }

        // Ÿ�� ���� �ֱ� ������Ʈ
        tileMaker += Time.deltaTime;

        if (activeTiles.Count > 0)
        {
            // �÷��̾� ��ġ ��� ���� ������ Ÿ�� ����
            if (Vector3.Distance(playerTr.position, activeTiles[0].transform.position) > tileLength)
            {
                RemoveOldTile();
            }
        }
        // �÷��̾ ���� ��ġ�� �Ѿ�� �� Ÿ�� ����
        // ���⼭�� �׳� �׽�Ʈ�� ��� ����
        //if (Input.GetKeyDown(KeyCode.Space)) // �׽�Ʈ��: SpaceŰ�� Ÿ�� ����
        //{
        //    SpawnTile();
        //    DeletTile();
        //}
    }

    // Ÿ�� �ı� ����
    void RemoveOldTile()
    {
        if (activeTiles.Count > 0)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
            SpawnTile();
        }
        else
        {
            Debug.LogWarning("activeTiles ����Ʈ�� ��� �ֽ��ϴ�!");
        }
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
                currentDirection = GetNextDirection(tileComponent.tileType);
            }
            spawnZ += tileLength;
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

        spawnZ += tileLength;
        // ���� Ÿ���� ������ ������Ʈ
        currentPosition += currentDirection * tileLength;
    }

    private Vector3 GetNextDirection(SpawnTiles.TileType tileType)
    {
        switch (tileType)
        {
            case SpawnTiles.TileType.left:
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
