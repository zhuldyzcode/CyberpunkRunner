using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public event Action OnCoinCollected = delegate { };
    public event Action OnHeadOnObstacleHit = delegate { };
    public event Action OnSideObstacleHit = delegate { };
    public event Action OnJumpedOnCar = delegate { }; // Event for jumping on the car

    [SerializeField] private float carHeight = 0.1f;
    [SerializeField] private float distanceIfSameLane = 0.1f;
    [SerializeField] private AudioManager audioManager;

    private bool isCollisionEnabled = true;
    private void OnTriggerEnter(Collider other)
    {
        Vector3 playerPosition = transform.position;

        if (other.CompareTag("Coin"))
        {
            OnCoinCollected.Invoke();
            audioManager.PlayEffect("Coin");
            Destroy(other.gameObject);
        }

        else if (other.CompareTag("Obstacle") && isCollisionEnabled)
        {
            Vector3 obstaclePosition = other.transform.position;
            if (Mathf.Abs(obstaclePosition.x - playerPosition.x) < distanceIfSameLane)
            {
                OnHeadOnObstacleHit.Invoke();
            }
            else
            {
                
                OnSideObstacleHit.Invoke();
                audioManager.PlayEffect("Hit");
            }
        }

        else if (other.CompareTag("Car") && isCollisionEnabled)
        {
            Vector3 obstaclePosition = other.transform.position;
            if (Mathf.Abs(obstaclePosition.x - playerPosition.x) < distanceIfSameLane)
            {
                
                if (playerPosition.y > obstaclePosition.y + carHeight)
                {
                    audioManager.PlayEffect("Horn");
                    OnJumpedOnCar.Invoke();
                }
                else
                {
                    OnHeadOnObstacleHit.Invoke();
                }
            }
            else
            {
                audioManager.PlayEffect("Horn");
                OnSideObstacleHit.Invoke();          

            }
        }
    }
    public void ChangeIsCOllisionEnables()
    {
        isCollisionEnabled = !isCollisionEnabled;
        Debug.Log("isCollisionEnabled " + isCollisionEnabled);
    }
}