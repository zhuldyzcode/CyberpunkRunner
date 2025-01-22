using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;
    [SerializeField] private RoadGenerator roadGenerator;
    int itemSpace = 5;
    int itemCountInMap = 4;
    public int mapSize;

    int coinsCountInItem = 4;
    float coinsHeight = 0.4f;
    enum TrackPos { Left = -1, Center = 0, Right = 1};
    enum CoinStyle { Line, Jump};
    public float laneOffset = 2f;

    public GameObject CarBlackPrefab;
    public GameObject LaserStopPrefab;
    public GameObject CoinPrefab;
    public GameObject CyberTankPrefab;

    

    public List<GameObject> maps = new List<GameObject>();
    public List<GameObject> activeMaps = new List<GameObject>();

    struct MapItem
    {
        public void SetValues(GameObject obstacle, TrackPos trackPos, CoinStyle coinStyle)
        {
            this.obstacle = obstacle; this.trackPos = trackPos;this.coinStyle = coinStyle;
        }
        public GameObject obstacle;
        public TrackPos trackPos;
        public CoinStyle coinStyle;
    }
    private void Awake()
    {
        instance = this;
        mapSize = itemCountInMap * itemSpace;
        maps.Add(MakeMap1());
        maps.Add(MakeMap2());
        maps.Add(MakeMap3());
        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }
    }

    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        if (!RoadGenerator.instance.move) return;
        foreach (GameObject map in activeMaps)
        {
            map.transform.position -= new Vector3(0,0,roadGenerator.speed* Time.deltaTime);
        }
        if(activeMaps[0].transform.position.z < -itemCountInMap * itemSpace*1.3)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
    }
    public void ResetMaps()
    {
        while (activeMaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }
        AddActiveMap();
        AddActiveMap();
    }
    void RemoveFirstActiveMap()
    {
        activeMaps[0].SetActive(false);
        maps.Add(activeMaps[0]);
        activeMaps.RemoveAt(0);
    }    
    void AddActiveMap()
    {
        int r = Random.Range(0, maps.Count);
        GameObject go = maps[r];
        go.SetActive(true);
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetActive(true);

        }
        go.transform.position = activeMaps.Count > 0 ?
                                activeMaps[activeMaps.Count - 1].transform.position + Vector3.forward * mapSize :
                                new Vector3(0, 0, 5);
        maps.RemoveAt(r);
        activeMaps.Add(go);
    }

    GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        for (int i =0; i< itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            CoinStyle coinStyle = CoinStyle.Line;
            
            if (i == 1){trackPos = TrackPos.Left; obstacle = CarBlackPrefab; coinStyle = CoinStyle.Jump; }
            else if (i == 3) { trackPos = TrackPos.Right; obstacle = LaserStopPrefab; coinStyle = CoinStyle.Jump; }      
            //else if (i == 4) { trackPos = TrackPos.Right; obstacle = LaserStopPrefab; }
            Vector3 obstaclePos = new Vector3((int)trackPos*laneOffset, 0.72f, i*itemSpace);
            CreateCoins(coinStyle, obstaclePos,result);
            if(obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }

        return result;
    }
    //Change it
    GameObject MakeMap2()
    {
        GameObject result = new GameObject("Map2");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            CoinStyle coinStyle = CoinStyle.Line;

            if (i == 2) { trackPos = TrackPos.Left; obstacle = LaserStopPrefab; coinStyle = CoinStyle.Jump; }
            else if (i == 4) { trackPos = TrackPos.Center; obstacle = CarBlackPrefab; coinStyle = CoinStyle.Jump; }
            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 0.72f, i * itemSpace);
            CreateCoins(coinStyle, obstaclePos, result);
            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }

        return result;
    }
    GameObject MakeMap3()
    {
        GameObject result = new GameObject("Map3");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            CoinStyle coinStyle = CoinStyle.Line;

            if (i == 3) { trackPos = TrackPos.Center; obstacle = CyberTankPrefab; coinStyle = CoinStyle.Jump; }
            else if (i == 4) { trackPos = TrackPos.Right; obstacle = CarBlackPrefab; coinStyle = CoinStyle.Jump; }
            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 0.2f, i * itemSpace);
            CreateCoins(coinStyle, obstaclePos, result);
            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }

        return result;
    }
    void CreateCoins(CoinStyle style, Vector3 pos, GameObject parentObj)
    {
        Vector3 coinPos = Vector3.zero;
        if (style == CoinStyle.Line)
        {
            for(int i = 0 ; i <= coinsCountInItem / 2; i++)
            {
                coinPos.y = coinsHeight;
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(CoinPrefab, coinPos + pos, Quaternion.Euler(90, 180, 0));
                go.transform.SetParent (parentObj.transform);

            }
        }
        else if(style == CoinStyle.Jump)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Max(-1/2f * Mathf.Pow(i,2)+3, coinsHeight);
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(CoinPrefab, coinPos + pos, Quaternion.Euler(90, 180, 0));
                go.transform.SetParent(parentObj.transform);

            }
        }
    }
}
