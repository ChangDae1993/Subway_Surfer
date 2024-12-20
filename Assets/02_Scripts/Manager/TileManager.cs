using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public GameObject[] Tiles; // Ÿ�� ������ �迭

    public List<GameObject> activeTiles = new List<GameObject>(); // ���� Ȱ��ȭ�� Ÿ�� ����Ʈ

    private Transform playerTr;
    public Score_script playerLevel; // �÷��̾� ���� ��ũ��Ʈ
    public Player_Move playermove;   // �÷��̾� �̵� ��ũ��Ʈ
    [SerializeField] private float spawnZ = -20f;


    private Vector3 currentDirection = Vector3.forward; // �⺻ ����
    private Vector3 currentPosition = Vector3.zero;     // Ÿ�� ���� ������
    private float tileLength = 16f;                    // Ÿ���� ����

    private float tileMaker = 0f;  // Ÿ�� ���� Ÿ�̸�

    private int PretileObjNum = 10; // ������ Ÿ�� ����

    private int lastPrefabIndex = 0; // ���� ������ �ε���

    public bool lightOn = false;    //Ÿ�ϵ� ���ѱ�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �÷��̾� ���� ����
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
                SpawnTile(0); // ù 5���� �⺻ Ÿ��
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

        // Ÿ�� ���� �ֱ� ������Ʈ
        tileMaker += Time.deltaTime;

        if (activeTiles.Count > 0)
        {
            // �÷��̾� ��ġ ��� ���� ������ Ÿ�� ����
            if (Vector3.Distance(playerTr.position, activeTiles[0].transform.position) > tileLength)
            {
                playerLevel.score += 1f;
                RemoveOldTile();
            }
        }
    }

    // Ÿ�� �ı� ����
    void RemoveOldTile()
    {
        if (activeTiles.Count > 0)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
            //���ָ鼭 �տ� ���� ����
            SpawnTile();
        }
        else
        {
            Debug.LogWarning("activeTiles ����Ʈ�� ��� �ֽ��ϴ�!");
        }
    }



    private Queue<SpawnTiles.TileType> tileSequence = new Queue<SpawnTiles.TileType>(); // ��Ʈ ���� ť


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
                //Debug.Log("��ħ");
                // ��ħ�� �߻��ϸ� downY Ÿ�� ���� ����
                prefabIndex = GetPrefabIndexForTileType(SpawnTiles.TileType.down_Down);
                tile = Instantiate(Tiles[prefabIndex]);
                //Debug.Break();
            }
            else if (tileSequence.Count > 0 && !IsOverlappingWithPreviousTile())
            {
                // ť���� ���� Ÿ�� Ÿ�� ��������
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

            // Ÿ�� Ÿ�Կ� ���� ���� ������Ʈ �� ��Ʈ ���� ����
            if (tile.TryGetComponent(out SpawnTiles tileComponent))
            {
                currentDirection = GetNextDirection(tileComponent.tileType);

                // upŸ���� ������ ��Ʈ ť�� �߰�
                if (tileComponent.tileType == SpawnTiles.TileType.up_Up)
                {
                    UPEnqueueTileSequenceForSet(); // ť�� ���ο� ��Ʈ �߰�
                }
                else if(tileComponent.tileType == SpawnTiles.TileType.down_Down)
                {
                    DOWNEnqueueTileSequenceForSet();
                }
            }

        }

        // Ÿ���� ��ġ�� ���� ����
        tile.transform.SetParent(transform);
        tile.transform.position = currentPosition;
        tile.transform.rotation = Quaternion.LookRotation(currentDirection);
        activeTiles.Add(tile);

        // ���� Ÿ���� ������ ������Ʈ
        currentPosition += currentDirection * tileLength;
    }

    // Ư�� Ÿ�� ��Ʈ�� ť�� �߰�
    void UPEnqueueTileSequenceForSet()
    {
        tileSequence.Enqueue(SpawnTiles.TileType.up_flat);  // ù ��° ��Ʈ Ÿ��
        //tileSequence.Enqueue(SpawnTiles.TileType.upY);  // �� ��° ��Ʈ Ÿ��
        tileSequence.Enqueue(SpawnTiles.TileType.up_Down); // ������ ��Ʈ Ÿ��
    }

    public bool downStart = false;
    void DOWNEnqueueTileSequenceForSet()
    {
        if (!downStart)
        {
            downStart = true;
            tileSequence.Enqueue(SpawnTiles.TileType.down_flat);  // ù ��° ��Ʈ Ÿ��
            //tileSequence.Enqueue(SpawnTiles.TileType.upY);  // �� ��° ��Ʈ Ÿ��
            tileSequence.Enqueue(SpawnTiles.TileType.down_Up); // ������ ��Ʈ Ÿ��
        }

    }

    // Ÿ�� Ÿ�Կ� �´� ������ �ε��� ã��
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

        // upY�� down�� ������ ������ �ε����� �߰�
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

        // ��ȿ�� �ε��� �߿��� ������ ���� �ε����� ����
        validIndices.Remove(lastPrefabIndex);

        // ��ȿ�� �ε��� �߿��� ���� ����
        int randomIndex = validIndices[Random.Range(0, validIndices.Count)];

        lastPrefabIndex = randomIndex;  // ���� ���õ� �ε����� lastPrefabIndex�� ����
        return randomIndex;
    }


    public bool downPass = false;
    // ��ħ ���� Ȯ�� (��� ���� Ÿ�ϵ�� ��)
    bool IsOverlappingWithPreviousTile()
    {
        if (activeTiles.Count > 0)
        {
            // �����Ϸ��� Ÿ���� ��ġ
            Vector3 nextTilePosition = currentPosition;

            // activeTiles�� ��� Ÿ�ϵ�� ��
            foreach (var tile in activeTiles)
            {
                Vector3 lastTilePosition = tile.transform.position;

                // Ÿ�� �� �Ÿ��� �ʹ� ����� ��� ��ģ�ٰ� �Ǵ�
                if (Vector3.Distance(nextTilePosition, lastTilePosition) <= 0.5f)
                {
                    if (tile.gameObject.TryGetComponent(out SpawnTiles st))
                    {
                        //��ġ�� Ÿ���� downŸ���̸� �׳� ������
                        if (st.gameObject.name.Contains("down"))
                        {
                            downPass = true;
                            Debug.Log("�����?");
                            return false;
                        }
                        else
                        {
                            Debug.Log("���� �ȵ���?");
                            return true;
                        }
                    }
                }

            }
        }
        return false;
    }
}
