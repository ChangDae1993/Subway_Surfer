using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public GameObject[] Tiles;

    public List<GameObject> activeTiles = new List<GameObject>();

    private Transform playerTr;
    private float spawnZ = -20f;
    private float tileLength = 10f;
    private int PretileObjNum = 7;

    private int lastPrefabIndex = 0;

    private float saveZone = 15f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        //SpawnTile(0);

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

    // Update is called once per frame
    void Update()
    {
        if (playerTr.transform.position.z - saveZone > (spawnZ - PretileObjNum * tileLength))
        {
            SpawnTile();
            DeletTile();
        }
    }

    void SpawnTile(int prefabIndex = -1)
    {
        GameObject go;
        if(prefabIndex == -1)
        {
            go = Instantiate(Tiles[RandomPrefabIndex()]) as GameObject;
        }
        else
        {
            go = Instantiate(Tiles[prefabIndex]) as GameObject;
        }
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(go);
    }

    void DeletTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    int RandomPrefabIndex()
    {
        if(Tiles.Length <= 1)
        {
            return 0;
        }
        int randomIndex = lastPrefabIndex;

        while (randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, Tiles.Length);
        }

        if(randomIndex == 4)
        {
            Debug.Log("다리 만들기 시작");
            Debug.Break();
        }    

        lastPrefabIndex = randomIndex;
        return randomIndex;
    }
}
