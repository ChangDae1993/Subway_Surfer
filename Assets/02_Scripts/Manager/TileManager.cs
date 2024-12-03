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


    private Vector3 currentDirection = Vector3.forward; // 기본 방향
    private Vector3 currentPosition = Vector3.zero;     // 타일 생성 기준점
    private float tileLength = 16f;                    // 타일의 길이

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
            Debug.LogError("Player 태그가 있는 오브젝트를 찾을 수 없습니다.");
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

    private float tileMaker = 0f;  // 타일 생성 타이머

    // Update is called once per frame
    void Update()
    {
        if (playermove.isDead)
        {
            return;
        }

        // 타일 생성 주기 업데이트
        tileMaker += Time.deltaTime;

        if (activeTiles.Count > 0)
        {
            // 플레이어 위치 기반 가장 오래된 타일 제거
            if (Vector3.Distance(playerTr.position, activeTiles[0].transform.position) > tileLength)
            {
                RemoveOldTile();
            }
        }
        // 플레이어가 기준 위치를 넘어가면 새 타일 생성
        // 여기서는 그냥 테스트로 계속 생성
        //if (Input.GetKeyDown(KeyCode.Space)) // 테스트용: Space키로 타일 생성
        //{
        //    SpawnTile();
        //    DeletTile();
        //}
    }

    // 타일 파괴 로직
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
            Debug.LogWarning("activeTiles 리스트가 비어 있습니다!");
        }
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
