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

    private bool init = true;
    private bool first = true;

    public void setPaused(bool state)
    {
        started = !state;
    }

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

    void setToZero(int which)
    {
        if (which == -1)
        {
            foreach (GameObject tile in tiles)
            {
                tile.transform.position = Vector3.zero;
            }
        } else
        {
            tiles.ToArray()[which].transform.position = Vector3.zero;
        }
    }

    void Start()
    {
        if (auto)
        {
            spawnInitialTiles();
            started = true;
        }
    }

    void Update()
    {

        if (init && first)
        {
            init = false;
            first = false;
            setToZero(-1);
        }
        if (init)
        {
            init = false;
            setToZero(numberOfTiles - 1);
        }
        if (!started)
            return;
        float timeD = Time.deltaTime * speed;
        internPos += timeD;
        totalDistance += timeD;
        foreach (GameObject tile in tiles)
        {
            tile.transform.position += new Vector3(0, 0, timeD);
        }
        if (internPos > tileSize)
        {
            internPos = 0;
            Destroy(tiles[0].gameObject);
            tiles.RemoveAt(0);
            GameObject tile = Instantiate(prefabTile, gameObject.transform, true);
            tile.transform.position = new Vector3(0, 0, tileSize * (numberOfTiles - 1) * direction);
            tiles.Add(tile);
            init = true;
        }
    }
}
