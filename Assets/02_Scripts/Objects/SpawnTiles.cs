using System;
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

    public TileType tileType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
