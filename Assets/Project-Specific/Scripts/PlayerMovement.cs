using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private RoadGenerator Ground;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Transform mainWheel;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float laneDistance = 2f; // Distance between lanes
    //[SerializeField] private float wheelRotationSpeed = 100f; // Wheel rotation speed
    //[SerializeField] private float rotateDegree = 20f;
    [SerializeField] private float secondsWaitingTurn = 0.3f;

    private SwipeDetection swipeDetection;
    private Quaternion initialRotation;
    private Rigidbody rb;
    private bool isJumping = false;
    private int jumpCount = 0;
    private bool canDoubleJump = false;
    private Vector3 targetPosition;

    private int previousLane = 0;
    private int currentLane = 0; // Starts in the middle lane (-1 = left, 0 = middle, 1 = right)

    private Animator animationController;
    public float PositionInLane()
    {
        return currentLane * laneDistance;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        swipeDetection = GetComponent<SwipeDetection>();
        targetPosition = transform.position;
        initialRotation = transform.rotation;
        animationController = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        swipeDetection.OnSwipeUp += Jump;
        swipeDetection.OnSwipeLeft += MoveLeft;
        swipeDetection.OnSwipeRight += MoveRight;
        //swipeDetection.OnSwipeLeft += ManageRotateWhenTurningLeft;
       // swipeDetection.OnSwipeRight += ManageRotateWhenTurningRight;
        GetComponent<PlayerCollision>().OnSideObstacleHit += ReturnToPreviousLane;
    }

    private void OnDisable()
    {
        swipeDetection.OnSwipeUp -= Jump;
        swipeDetection.OnSwipeLeft -= MoveLeft;
        swipeDetection.OnSwipeRight -= MoveRight;
        //swipeDetection.OnSwipeLeft -= ManageRotateWhenTurningLeft;
        //swipeDetection.OnSwipeRight -= ManageRotateWhenTurningRight;
        GetComponent<PlayerCollision>().OnSideObstacleHit -= ReturnToPreviousLane;

    }

    private void Update()
    {
        if (Ground.move && !isJumping)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
            rb.MovePosition(newPosition);
            animationController.SetBool("Running", true);
            animationController.SetBool("Jumping", false);

            //RotateTheWheel();
        }
        else if(Ground.move && isJumping)
        {
            animationController.SetBool("Running", false);
            animationController.SetBool("Jumping", true);

        }
        else
        {
            animationController.SetBool("Running", false);
            animationController.SetBool("Jumping", false);
        }
        //Debug.Log(animationController.GetCurrentAnimatorStateInfo(0));
    }
    private void Jump()
    {
        if (!canDoubleJump && jumpCount >= 1) return; // Only allow one jump if double jump is disabled
        if (Ground.move)
        {
            isJumping = true;
            jumpCount++;
            audioManager.PlayEffect("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (jumpCount == 1)
            {
                StartCoroutine(EndJump());
            }
        }
    }

    private IEnumerator EndJump()
    {
        yield return new WaitForSeconds(0.75f);
        isJumping = false;
        jumpCount = 0;
    }

    private void ReturnToPreviousLane()
    {
        if (previousLane > currentLane)
        {
            MoveRight();
        }
        else if (previousLane < currentLane)
        {
            MoveLeft();
        }
    }
 
    private void MoveLeft()
    {
        if (currentLane > -1)
        {
            previousLane = currentLane;
            currentLane--;
            targetPosition = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);
        }
    }

    private void MoveRight()
    {
        if (currentLane < 1)
        {
            previousLane = currentLane;
            currentLane++;
            targetPosition = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);
        }
    }

    public void PlayerFall()
    {
        gameObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    public void PlayerStandUp()
    {
        gameObject.transform.rotation = initialRotation;
        Vector3 centralPosition = new Vector3(0, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.position = centralPosition;
    }
    public void EnableDoubleJump()
    {
        canDoubleJump = true;
    }

    public void DisableDoubleJump()
    {
        canDoubleJump = false;
    }
/*
    #region Decorative Movement

    private void RotateTheWheel()
    {
        if (mainWheel != null)
        {
            mainWheel.Rotate(Vector3.forward * wheelRotationSpeed * Time.deltaTime);
        }
    }

    private void ManageRotateWhenTurningRight()
    {
        StartCoroutine(RotateWhenTurningRight());
    }

    private IEnumerator RotateWhenTurningRight()
    {
        gameObject.transform.rotation = Quaternion.Euler(rotateDegree, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        yield return new WaitForSeconds(secondsWaitingTurn);
        gameObject.transform.rotation = initialRotation;
    }

    private void ManageRotateWhenTurningLeft()
    {
        StartCoroutine(RotateWhenTurningLeft());
    }

    private IEnumerator RotateWhenTurningLeft()
    {
        gameObject.transform.rotation = Quaternion.Euler(-rotateDegree, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        yield return new WaitForSeconds(secondsWaitingTurn);
        gameObject.transform.rotation = initialRotation;
    }

    #endregion
*/
}