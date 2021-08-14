using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonMap : MonoBehaviour
{
    enum typeMap
    {
        GRASS,
        HIVE
    };

    public bool spawned = false;

    [SerializeField]
    private typeMap type;
    [SerializeField]
    private GameObject prefabHexagon;
    [SerializeField]
    private GameObject prefabHoney;
    [SerializeField]
    private Vector2 mapSize;
    [SerializeField]
    private Vector2 heightVariation;

    // Start is called before the first frame update
    void Start()
    {
        generateMap();
    }

    public void generateMap()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                GameObject hexa = Instantiate(prefabHexagon);
                hexa.transform.parent = gameObject.transform;
                int Xodd = x % 2;
                hexa.transform.position = new Vector3((x - (mapSize.x / 2)) * 0.75f , 0, y - (Xodd * 0.5f) - (mapSize.y / 2));
                Vector3 scale = hexa.transform.localScale;
                float height = Random.Range(heightVariation.x, heightVariation.y);
                hexa.transform.localScale = new Vector3(scale.x, height, scale.z);
                if (Random.Range(0, 10) < 6 && type == typeMap.HIVE) {
                    GameObject honey = Instantiate(prefabHoney);
                    honey.transform.localScale = new Vector3(scale.x, Random.Range(heightVariation.x + (heightVariation.y / 10), height), scale.z);
                    honey.transform.parent = hexa.transform;
                    honey.transform.position = new Vector3((x - (mapSize.x / 2)) * 0.75f, 0, y - (Xodd * 0.5f) - (mapSize.y / 2));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
