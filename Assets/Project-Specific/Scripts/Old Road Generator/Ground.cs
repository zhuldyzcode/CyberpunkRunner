using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Ground : MonoBehaviour
{
    public List<GameObject> levelBlocks; // List of prefabs to use for track generation
    public float distanceBlock; // Length of each block in Unity units
    public int segmentsCount; // Number of segments visible on screen at once
    public float speed; // Speed at which the track scrolls
    public float initialSpeed = 2f;
    public float changeInSpeed = 0.05f;

    public bool move = false; // Toggle for track movement
    public bool useMoveDelay = true; // Whether to use a delay before starting movement
    public float moveDelay; // Delay before starting movement
    public TextMeshProUGUI countdownText;

    private List<GameObject> activeBlocks;
    public event Action OnSegmentDestroyedEvent;


    private void Start()
    {
        activeBlocks = new List<GameObject>();
        InitializeTrack();
        OnSegmentDestroyedEvent += HandleSegmentDestroyed;
        speed = initialSpeed;
    }
    public void DestroyTrack()
    {
        for (int i = 0; i < activeBlocks.Count; i++)
        {
            Destroy(activeBlocks[i]);
        }
    }
    public void CountDownBeforeStart()
    {
        if (useMoveDelay)
        {
            move = false;
            StartCoroutine(MoveDelay());
        }
    }
    private void IncreaseSpeed()
    {
        speed += changeInSpeed;
    }
    public void SetSpeedToInitial()
    {
        speed = initialSpeed;
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
    public void InitializeTrack()
    {
        for (int i = 0; i < segmentsCount; i++)
        {
            CreateSegment(i);
        }
    }

    private void CreateSegment(int index)
    {
        int randomIndex = UnityEngine.Random.Range(0, levelBlocks.Count);
        GameObject segment = Instantiate(levelBlocks[randomIndex]);
        segment.transform.parent = transform;
        segment.transform.localPosition = new Vector3(0, 0, distanceBlock * (index));
        activeBlocks.Add(segment);
    }
    private void CreateSegmentFurther(Vector3 position)
    {
        int randomIndex = UnityEngine.Random.Range(0, levelBlocks.Count);
        GameObject segment = Instantiate(levelBlocks[randomIndex]);
        segment.transform.parent = transform;
        segment.transform.localPosition = position;
        activeBlocks.Add((segment));
    }

    public void OnSegmentDestroyed()
    {
        OnSegmentDestroyedEvent?.Invoke();
    }

    private void HandleSegmentDestroyed()
    {
        CreateSegmentFurther(FindLastActiveBlockPosition());
        IncreaseSpeed();
    }
    private Vector3 FindLastActiveBlockPosition()
    {
        if (activeBlocks == null || activeBlocks.Count == 0)
        {
            return Vector3.zero;
        }
        Vector3 lastActiveBlockPosition = activeBlocks[^1].transform.position;
        lastActiveBlockPosition.z += distanceBlock;
        int targetIndex = 0;
        for (int i = 0; i < activeBlocks.Count; i++)
        {
            if (activeBlocks[i] != null)
            {
                activeBlocks[targetIndex] = activeBlocks[i];
                targetIndex++;
            }
        }
        for (int i = activeBlocks.Count - 1; i >= targetIndex; i--)
        {
            activeBlocks.RemoveAt(i);
        }
        return lastActiveBlockPosition;
    }
    public void Pause()
    {
        move = false;
    }
    public void Resume()
    {
        move = true;
    }
}