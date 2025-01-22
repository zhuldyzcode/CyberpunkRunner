using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private float minimumDistance = 0.2f;
    [SerializeField] private float maximumTime = 0.5f;
    [SerializeField,Range (0f,1f)] private float directionThreshold = .9f;


    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private Coroutine coroutine;

    public event System.Action OnSwipeUp;
    public event System.Action OnSwipeDown;
    public event System.Action OnSwipeLeft;
    public event System.Action OnSwipeRight;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;


    }
    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
       
        coroutine = StartCoroutine(Trail());
    }
    private IEnumerator Trail()
    {
        while (true)
        {
            yield return null;
        }
    }
    private void SwipeEnd(Vector2 position, float time)
    {
        StopCoroutine(coroutine);
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }
    private void DetectSwipe()
    {
      if(Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
            (endTime-startTime)<=maximumTime)
        {
            Vector2 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }
    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            OnSwipeUp?.Invoke();
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            OnSwipeDown?.Invoke();
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            OnSwipeLeft?.Invoke();
        }
        else if(Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            OnSwipeRight?.Invoke();
        }
    }
}
