using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    [SerializeField]
    private bool auto = true;
    [SerializeField]
    public float speed;
    [SerializeField]
    private GameObject prefabTile;
    [SerializeField]
    private int numberOfTiles;
    private float internPos = 0;
    public float totalDistance = 0;

    public List<GameObject> tiles = new List<GameObject>();
    [SerializeField]
    private float tileSize;
    private int direction;

    private bool started = false;

    public void setPaused(bool state)
    {
        started = !state;
    }

    public void clear()
    {
        started = false;
        internPos = 0;
        totalDistance = 0;
        foreach (GameObject tile in tiles)
        {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
    }

    //Spawn the number of tiles that should be displayed
    public void spawnInitialTiles()
    {
        direction = (speed > 0 ? 1 : -1);
        if (tileSize == -1)
            tileSize = prefabTile.transform.lossyScale.z;
        for (int nb = 0; nb < numberOfTiles; nb++)
        {
            GameObject tile = Instantiate(prefabTile, gameObject.transform, true);
            tile.transform.position = new Vector3(0, 0, tileSize * nb * direction);
            tiles.Add(tile);
        }
        started = true;
    }

    //Move the tiles or one of the tiles to zero to reset it's relative position
    void setToZero()
    {
        foreach (GameObject tile in tiles)
        {
            if (!tile.transform.GetComponent<HexagonMap>().moved)
            {
                tile.transform.position = Vector3.zero;
                tile.transform.GetComponent<HexagonMap>().moved = true;
            }
        }
    }

    void Start()
    {
        if (auto)
        {
            spawnInitialTiles();
        }
    }

    void Update()
    {
        float timeD = Time.deltaTime * speed;
        if (!started)
            return;
        internPos += timeD;
        totalDistance += timeD;
        //move all the tiles
        foreach (GameObject tile in tiles)
        {
            tile.transform.position += new Vector3(0, 0, timeD);
        }
        //delete last tile and spawn a new
        if (internPos > tileSize)
        {
            internPos = 0;
            Destroy(tiles[0].gameObject);
            tiles.RemoveAt(0);
            GameObject tile = Instantiate(prefabTile, gameObject.transform, true);
            tile.transform.position = new Vector3(0, 0, tileSize * (numberOfTiles - 1) * direction);
            tiles.Add(tile);
        }
    }
}
