using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public GameObject[] Tiles;

    public List<GameObject> activeTiles = new List<GameObject>();

    private Transform playerTr;
    [SerializeField] private float spawnZ = -20f;


    private Vector3 currentDirection = Vector3.forward; // 기본 방향
    private Vector3 currentPosition = Vector3.zero;     // 타일 생성 기준점
    private float tileLength = 16f;                    // 타일의 길이

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
            Debug.LogError("Player 태그가 있는 오브젝트를 찾을 수 없습니다.");
        }

        for (int i = 0; i < PretileObjNum; i++)
        {
            if (i < 3)
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
    // Update is called once per frame
    void Update()
    {
        //Debug.Log((playerTr.transform.position.z - saveZone) + " : " + (spawnZ - PretileObjNum * tileLength));
        // 플레이어가 기준 위치를 넘어가면 새 타일 생성
        //if (playerTr.transform.position.z - saveZone > (spawnZ - PretileObjNum * tileLength))
        //{
        //    SpawnTile();
        //    DeletTile();
        //}

        // 타일 생성 로직
        // (예시로 플레이어가 이동할 때마다 타일을 생성하는 로직을 추가)
        if (Vector3.Distance(playerTr.transform.position, lastTilePosition) > tileLength)
        {
            SpawnTile();
            DeletTile();
            lastTilePosition = playerTr.transform.position;  // 타일 생성 후, 마지막 생성 위치 업데이트
        }

        //Debug.Log((Vector3.Distance(playerTr.transform.position, lastTilePosition)) + " : " + (tileLength));

        // 플레이어가 기준 위치를 넘어가면 새 타일 생성
        // 여기서는 그냥 테스트로 계속 생성
        //if (Input.GetKeyDown(KeyCode.Space)) // 테스트용: Space키로 타일 생성
        //{
        //    SpawnTile();
        //    DeletTile();
        //}
    }


    void SpawnTile(int prefabIndex = -1)
    {
        GameObject tile;

        if (prefabIndex == -1)
        {
            tile = Instantiate(Tiles[RandomPrefabIndex()]) as GameObject;


            // 타일 타입에 따라 방향 업데이트
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

        // 타일의 위치와 방향 설정
        tile.transform.SetParent(transform);
        tile.transform.position = currentPosition;
        tile.transform.rotation = Quaternion.LookRotation(currentDirection); // 방향에 따라 회전
        activeTiles.Add(tile);

        spawnZ += tileLength;
        // 다음 타일의 기준점 업데이트
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


    void DeletTile()
    {
        if (activeTiles.Count > 0)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }
        else
        {
            Debug.LogWarning("activeTiles 리스트가 비어 있습니다!");
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
