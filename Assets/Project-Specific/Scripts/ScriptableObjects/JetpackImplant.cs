using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Implants/JetPack")]
public class JetpackImplant : Implant
{
    public float changeInPosition = 5f; // Desired height change
    public float targetYPosition = 5f; // The final Y position
    public float changeInRotationInX = 90f;
    public float smoothTime = 1f; // Time to complete the movement

    public override void ApplyEffect(GameObject target)
    {
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.StartCoroutine(SmoothMove(playerMovement));
        }
    }

    public override void RemoveEffect(GameObject target)
    {
        // Optionally, you can revert the player to the original state
        // This example does not revert the player after the effect duration
    }

    private IEnumerator SmoothMove(PlayerMovement playerMovement)
    {
        playerMovement.enabled = false; // Pause normal movement

        Transform targetTransform = playerMovement.transform;
        Vector3 startPosition = targetTransform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, targetYPosition, startPosition.z);
        Quaternion startRotation = targetTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(changeInRotationInX, 0, 0);

        float elapsedTime = 0f;

        while (elapsedTime < smoothTime)
        {
            targetTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / smoothTime);
            targetTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / smoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and rotation are set
        targetTransform.position = targetPosition;
        targetTransform.rotation = targetRotation;

        yield return new WaitForSeconds(duration - smoothTime);

        // Resume normal movement after the duration
        playerMovement.enabled = true;
    }
}