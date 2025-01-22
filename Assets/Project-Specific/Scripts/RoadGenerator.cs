using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RoadGenerator : MonoBehaviour
{
    public static RoadGenerator instance;
    public GameObject RoadPrefab;
    private List<GameObject> roads = new List<GameObject> ();
    public float maxSpeed = 15f;
    public float speed = 7f;
    private float initialSpeed = 7f;
    public float changeInSpeed = 0.08f;
    public float roadLength = 20;
    public int maxRoadCount = 5;

    // Delay
    public bool move = false; // Toggle for track movement
    public bool useMoveDelay = true; // Whether to use a delay before starting movement
    public float moveDelay; // Delay before starting movement
    public TextMeshProUGUI countdownText;

    //Manage each block passed
    public event Action OnSegmentDestroyedEvent;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PoolManager.Instance.Preload(RoadPrefab, 5);
        ResetLevel();
        OnSegmentDestroyedEvent += IncreaseSpeed;

    }
    void OnDisable()
    {
        OnSegmentDestroyedEvent -= IncreaseSpeed;
    }

    void Update()
    {
        if (move)
        {
            foreach (GameObject road in roads)
            {
                road.transform.position -= new Vector3(0,0,speed*Time.deltaTime);
            }
            if (roads[0].transform.position.z < -(2*roadLength))
            {
                OnSegmentDestroyed();
                PoolManager.Instance.Despawn(roads[0]);
                roads.RemoveAt(0);
                CreateNextRoad();
            }
        }
    }
    void CreateNextRoad()
    {
        Vector3 pos = Vector3.zero;
        if(roads.Count > 0) { pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, roadLength); }
        GameObject go = PoolManager.Instance.Spawn(RoadPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        roads.Add(go);
    }
    public void ResetLevel()
    {
        speed = initialSpeed;
        while(roads.Count > 0)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);
        }
        for(int i = 0; i < maxRoadCount; i++)
        {
            CreateNextRoad();
        }
        MapGenerator.instance.ResetMaps();
    }
    private void IncreaseSpeed()
    {
        speed += changeInSpeed;
    }

    #region Delay
    public void CountDownBeforeStart()
    {
        if (useMoveDelay)
        {
            move = false;
            StartCoroutine(MoveDelay());
        }
    }
    IEnumerator MoveDelay()
    {
        countdownText.gameObject.SetActive(true);
        int countdown = (int)moveDelay;

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1);
            countdown--;
        }

        countdownText.text = "Go!";
        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false);
        move = true;
    }
    #endregion

    public void Pause()
    {
        move = false;
    }
    public void Resume()
    {
        move = true;
    }
    public void OnSegmentDestroyed()
    {
        OnSegmentDestroyedEvent?.Invoke();
    }


}
