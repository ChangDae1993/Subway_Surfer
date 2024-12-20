using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public GameObject[] Tiles; // 타일 프리팹 배열

    public List<GameObject> activeTiles = new List<GameObject>(); // 현재 활성화된 타일 리스트

    private Transform playerTr;
    public Score_script playerLevel; // 플레이어 레벨 스크립트
    public Player_Move playermove;   // 플레이어 이동 스크립트
    [SerializeField] private float spawnZ = -20f;


    private Vector3 currentDirection = Vector3.forward; // 기본 방향
    private Vector3 currentPosition = Vector3.zero;     // 타일 생성 기준점
    private float tileLength = 16f;                    // 타일의 길이

    private float tileMaker = 0f;  // 타일 생성 타이머

    private int PretileObjNum = 10; // 유지할 타일 개수

    private int lastPrefabIndex = 0; // 이전 프리팹 인덱스

    public bool lightOn = false;    //타일들 불켜기

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 플레이어 참조 설정
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
                SpawnTile(0); // 첫 5개는 기본 타일
            }
            else
            {
                SpawnTile();
            }
        }

        lightOn = false;
    }


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
                playerLevel.score += 1f;
                RemoveOldTile();
            }
        }
    }

    // 타일 파괴 로직
    void RemoveOldTile()
    {
        if (activeTiles.Count > 0)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
            //없애면서 앞에 새로 생성
            SpawnTile();
        }
        else
        {
            Debug.LogWarning("activeTiles 리스트가 비어 있습니다!");
        }
    }



    private Queue<SpawnTiles.TileType> tileSequence = new Queue<SpawnTiles.TileType>(); // 세트 관리 큐


    void SpawnTile(int prefabIndex = -1)
    {
        GameObject tile;

        if (prefabIndex != -1)
        {
            tile = Instantiate(Tiles[prefabIndex]);
        }
        else
        {
            if (IsOverlappingWithPreviousTile())
            {
                //Debug.Log("겹침");
                // 겹침이 발생하면 downY 타입 강제 생성
                prefabIndex = GetPrefabIndexForTileType(SpawnTiles.TileType.down_Down);
                tile = Instantiate(Tiles[prefabIndex]);
                //Debug.Break();
            }
            else if (tileSequence.Count > 0 && !IsOverlappingWithPreviousTile())
            {
                // 큐에서 다음 타일 타입 가져오기
                SpawnTiles.TileType nextTileType = tileSequence.Dequeue();

                prefabIndex = GetPrefabIndexForTileType(nextTileType);
                tile = Instantiate(Tiles[prefabIndex]);
            }
            else
            {
                downStart = false;
                if (downPass)
                {
                    tile = Instantiate(Tiles[0]);
                    downPass = false;
                }
                else
                {
                    prefabIndex = RandomPrefabIndex();
                    tile = Instantiate(Tiles[prefabIndex]);
                }

            }

            // 타일 타입에 따라 방향 업데이트 및 세트 생성 시작
            if (tile.TryGetComponent(out SpawnTiles tileComponent))
            {
                currentDirection = GetNextDirection(tileComponent.tileType);

                // up타일을 만나면 세트 큐에 추가
                if (tileComponent.tileType == SpawnTiles.TileType.up_Up)
                {
                    UPEnqueueTileSequenceForSet(); // 큐에 새로운 세트 추가
                }
                else if(tileComponent.tileType == SpawnTiles.TileType.down_Down)
                {
                    DOWNEnqueueTileSequenceForSet();
                }
            }

        }

        // 타일의 위치와 방향 설정
        tile.transform.SetParent(transform);
        tile.transform.position = currentPosition;
        tile.transform.rotation = Quaternion.LookRotation(currentDirection);
        activeTiles.Add(tile);

        // 다음 타일의 기준점 업데이트
        currentPosition += currentDirection * tileLength;
    }

    // 특정 타일 세트를 큐에 추가
    void UPEnqueueTileSequenceForSet()
    {
        tileSequence.Enqueue(SpawnTiles.TileType.up_flat);  // 첫 번째 세트 타일
        //tileSequence.Enqueue(SpawnTiles.TileType.upY);  // 두 번째 세트 타일
        tileSequence.Enqueue(SpawnTiles.TileType.up_Down); // 마지막 세트 타일
    }

    public bool downStart = false;
    void DOWNEnqueueTileSequenceForSet()
    {
        if (!downStart)
        {
            downStart = true;
            tileSequence.Enqueue(SpawnTiles.TileType.down_flat);  // 첫 번째 세트 타일
            //tileSequence.Enqueue(SpawnTiles.TileType.upY);  // 두 번째 세트 타일
            tileSequence.Enqueue(SpawnTiles.TileType.down_Up); // 마지막 세트 타일
        }

    }

    // 타일 타입에 맞는 프리팹 인덱스 찾기
    int GetPrefabIndexForTileType(SpawnTiles.TileType tileType)
    {
        for (int i = 0; i < Tiles.Length; i++)
        {
            if (Tiles[i].GetComponent<SpawnTiles>().tileType == tileType)
            {
                return i;
            }
        }
        return -1;
    }


    private Vector3 GetNextDirection(SpawnTiles.TileType tileType)
    {
        switch (tileType)
        {
            case SpawnTiles.TileType.left:
                return Quaternion.Euler(0, -90, 0) * currentDirection;
            case SpawnTiles.TileType.right:
                return Quaternion.Euler(0, 90, 0) * currentDirection;
            case SpawnTiles.TileType.up_Up:
                return currentDirection;
            case SpawnTiles.TileType.up_flat:
                return currentDirection;
            case SpawnTiles.TileType.up_Down:
                return currentDirection;
            case SpawnTiles.TileType.down_Down:
                return currentDirection;
            case SpawnTiles.TileType.down_flat:
                return currentDirection;
            case SpawnTiles.TileType.down_Up:
                return currentDirection;
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

        List<int> validIndices = new List<int>();

        // upY와 down을 제외한 프리팹 인덱스만 추가
        for (int i = 0; i < Tiles.Length; i++)
        {
            SpawnTiles tileComponent = Tiles[i].GetComponent<SpawnTiles>();
            if (tileComponent != null && 
                tileComponent.tileType != SpawnTiles.TileType.up_flat && 
                tileComponent.tileType != SpawnTiles.TileType.up_Down &&
                tileComponent.tileType != SpawnTiles.TileType.down_Down &&
                tileComponent.tileType != SpawnTiles.TileType.down_flat &&
                tileComponent.tileType != SpawnTiles.TileType.down_Up)
            {
                validIndices.Add(i);
            }
        }

        // 유효한 인덱스 중에서 이전에 나온 인덱스를 제외
        validIndices.Remove(lastPrefabIndex);

        // 유효한 인덱스 중에서 랜덤 선택
        int randomIndex = validIndices[Random.Range(0, validIndices.Count)];

        lastPrefabIndex = randomIndex;  // 현재 선택된 인덱스를 lastPrefabIndex로 저장
        return randomIndex;
    }


    public bool downPass = false;
    // 겹침 여부 확인 (모든 이전 타일들과 비교)
    bool IsOverlappingWithPreviousTile()
    {
        if (activeTiles.Count > 0)
        {
            // 생성하려는 타일의 위치
            Vector3 nextTilePosition = currentPosition;

            // activeTiles의 모든 타일들과 비교
            foreach (var tile in activeTiles)
            {
                Vector3 lastTilePosition = tile.transform.position;

                // 타일 간 거리가 너무 가까운 경우 겹친다고 판단
                if (Vector3.Distance(nextTilePosition, lastTilePosition) <= 0.5f)
                {
                    if (tile.gameObject.TryGetComponent(out SpawnTiles st))
                    {
                        //겹치는 타일이 down타입이면 그냥 지나감
                        if (st.gameObject.name.Contains("down"))
                        {
                            downPass = true;
                            Debug.Log("여기는?");
                            return false;
                        }
                        else
                        {
                            Debug.Log("여기 안들어옴?");
                            return true;
                        }
                    }
                }

            }
        }
        return false;
    }
}
