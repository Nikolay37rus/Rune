using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnPos = 0;
    private float tileLenght = 100;

    [SerializeField] private Transform player;
    private int startTiles = 6;
    
    void Start()
    {
       for (int i = 0; i < startTiles; i++)
        {
            if (i == 0)
                SpawnTile(3);

            SpawnTile(Random.Range(0, tilePrefabs.Length));
        }
    }

    
    void Update()
    {
        if (player.position.z - 60 > spawnPos - (startTiles *tileLenght))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleliteTile();

        }
            
    }

    private void SpawnTile(int tileIndex)
    {
        GameObject nextTile = Instantiate(tilePrefabs[tileIndex], transform.forward * spawnPos, transform.rotation);
        activeTiles.Add(nextTile);
        spawnPos += tileLenght;
    }
    private void DeleliteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
