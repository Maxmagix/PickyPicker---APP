using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    //Level Parameters
    [SerializeField]
    public int level;
    [SerializeField]
    private float density;
    [SerializeField]
    private float scatter;
    [SerializeField]
    private float durationDistance;

    //UI and Flower
    [SerializeField]
    private RawImage[] flowerImages;
    [SerializeField]
    private Text displayedText;
    public int selectedFlower;
    private List<FileInfo> fileList;
    [SerializeField]
    private GameObject flowerPrefab;
    [SerializeField]
    private string textToShow = "Find this flower";
    [SerializeField]
    private string flowerFolder;
    [SerializeField]
    private GameObject previewOverlay;

    private List<HexagonMap> populatedFloors = new List<HexagonMap>();
    private bool started = false;
    private MovingFloor movingFloor = null;
    private ScoreManager scoreManager = null;

    //Time stuff
    private Clock timer = new Clock();
    int secondsToCount = 3;
    int textShowTime = 2;
    int numberSeconds = 0;
    bool initTimer = false;
    bool timerRunning = false;

    int spawnedFlowers;

    // Start is called before the first frame update
    void Start()
    {
        LoadFlowers();
        ChooseFlower();
        movingFloor = gameObject.transform.GetComponent<MovingFloor>();
        movingFloor.spawnInitialTiles();
        started = true;
        initTimer = true;
        GameObject scorer = GameObject.FindGameObjectWithTag("ScoreManager");
        scoreManager = scorer.transform.GetComponent<ScoreManager>();
    }

    public bool checkGameRunning()
    {
        return (!timerRunning && started);
    }

    void setLevelDifficulty(float chosenSpeed, float chosenDist)
    {
        movingFloor.speed = chosenSpeed;
        durationDistance = chosenDist;
    }

    void LoadFlowers()
    {
        var info = new DirectoryInfo(Application.dataPath + flowerFolder);
        var fileArray = info.GetFiles();
        fileList = new List<FileInfo>(fileArray);
        fileList.RemoveAll(r => r.ToString().EndsWith(".meta"));
    }

    Texture2D LoadTextureFlower(string filePath)
    {
        Texture2D texture = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
        }
        return texture;
    }

    void ChooseFlower()
    {
        selectedFlower = Random.Range(0, fileList.Count);
        Texture textureFlower = LoadTextureFlower(fileList[selectedFlower].FullName);
        flowerImages[0].texture = textureFlower;
        flowerImages[1].texture = textureFlower;
    }

    void PopulateHexagonFloors()
    {
        int nbHexaToLast = 0;
        //Get all the hexagonMap
        GameObject[] hexagons = GameObject.FindGameObjectsWithTag("HexagonFloor");
        foreach (GameObject hexagon in hexagons)
        {
            if (nbHexaToLast == hexagons.Length - 1)
                break;
            nbHexaToLast++;
            HexagonMap hexaMap = hexagon.transform.GetComponent<HexagonMap>();
            //Check if I already populated the area
            if (!hexaMap.spawned && hexagon.transform.childCount > 0)
            {
                hexaMap.spawned = true;
                int nbHexa = hexagon.transform.childCount;
                int nbFlowers = (int)(density);
                int spawnFlowerAt = Random.Range(0, nbFlowers);
                //For each flower to spawn, I choose a random tile in the area
                for (int nb = 0; nb < nbFlowers; nb++)
                {
                    int selectHexa = Random.Range(0, nbHexa);
                    GameObject dirtTile = hexagon.transform.GetChild(selectHexa).gameObject;
                    Vector3 posTile = dirtTile.transform.position;
                    //I make sure not to spawn a flower on top of another and outside the map
                    while (posTile.x < -2.5f || posTile.x > 2.5f || posTile.z > -7 || 
                        dirtTile.transform.childCount > 0)
                    {
                        selectHexa = Random.Range(0, nbHexa);
                        dirtTile = hexagon.transform.GetChild(selectHexa).gameObject;
                        posTile = dirtTile.transform.position;
                    }
                    dirtTile = hexagon.transform.GetChild(selectHexa).gameObject;
                    GameObject flower = Instantiate(flowerPrefab, dirtTile.transform, true);
                    Transform parentTr = flower.transform.parent;
                    flower.transform.localScale = new Vector3(0.008f / parentTr.localScale.x, 0.008f / parentTr.localScale.z, 1);
                    flower.transform.position = new Vector3(
                        parentTr.position.x + Random.Range(scatter * -1, scatter),
                        1,
                        parentTr.position.z + Random.Range(scatter * -1, scatter));
                    int tempFlower = Random.Range(0, fileList.Count);
                    //If the unique flower already spawned I choose another flower
                    while (tempFlower == selectedFlower)
                    {
                        tempFlower = Random.Range(0, fileList.Count);
                    }
                    if (nb == spawnFlowerAt)
                    {
                        tempFlower = selectedFlower;
                    }
                    if (tempFlower == selectedFlower)
                    {
                        flower.transform.position += new Vector3(0, 0.01f, 0);
                        spawnedFlowers++;
                    }
                    GameObject image = flower.transform.GetChild(0).gameObject;
                    image.transform.GetChild(0).GetComponent<FlowerButton>().correct = (tempFlower == selectedFlower);
                    image.transform.GetComponent<RawImage>().texture = LoadTextureFlower(fileList[tempFlower].FullName);
                }
            }
        }
        scoreManager.spawnedFlowers = spawnedFlowers;
    }

    void initTimerIntro()
    {
        previewOverlay.SetActive(true);
        numberSeconds = 0;
        movingFloor.setPaused(true);
        timer.Reset();
        displayedText.text = textToShow;
        displayedText.transform.GetChild(0).GetComponent<Text>().text = textToShow;
        timerRunning = true;
        initTimer = false;
    }

    void Replay()
    {

    }

    void EndLevel()
    {
        movingFloor.setPaused(true);
        scoreManager.showResult();
        started = false;
    }

    void showFlower()
    {
        if (numberSeconds > secondsToCount + textShowTime)
        {
            previewOverlay.SetActive(false);
            timerRunning = false;
            movingFloor.setPaused(false);
        }
        if (timer.ElapsedTime() >= 1)
        {
            if (numberSeconds >= textShowTime)
            {
                displayedText.text = (secondsToCount + textShowTime - numberSeconds).ToString();
                displayedText.transform.GetChild(0).GetComponent<Text>().text = (secondsToCount + textShowTime - numberSeconds).ToString();
            }
            numberSeconds++;
            timer.Reset();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            showFlower();
            return;
        }
        if (!started)
            return;
        PopulateHexagonFloors();
        if (initTimer)
            initTimerIntro();
        if (movingFloor.totalDistance > durationDistance)
        {
            EndLevel();
        }
    }
}
