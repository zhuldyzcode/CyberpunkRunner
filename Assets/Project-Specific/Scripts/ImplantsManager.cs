using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImplantsManager : MonoBehaviour
{
    public float jetpackDuration = 5f; // Duration for the jetpack effect
    public float riseSpeed = 2f; // Speed at which the player rises
    public float rotationSpeed = 2f; // Speed at which the player rotates

    private bool hasQuant = false;
    private bool hasJetpack = false;
    private bool hasSpeedRun = false;
    private bool isUsingJetpack = true;
    #region Properties
    public bool HasQuant
    {
        get { return hasQuant; }
    }
    public bool HasJetpack
    {
        get { return hasJetpack; }
    }
    public bool HasSpeedRun
    {
        get { return hasSpeedRun; }
    }
    #endregion
    public void AddQuant()
    {
        hasQuant = true;
    }
    public void AddJetpack()
    {
        hasQuant = hasJetpack;
    }
    public void AddSpeedRun()
    {
        hasQuant = hasSpeedRun;
    }

    public void UseQuant()
    {
    }
    public void UseSpeedRun()
    {
    }

    public void UseJetpack()
    {
        if (!isUsingJetpack)
        {
            StartCoroutine(JetpackRoutine());
        }
    }

    private IEnumerator JetpackRoutine()
    {
        isUsingJetpack = true;
        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(90f, initialRotation.eulerAngles.y, initialRotation.eulerAngles.z);

        while (elapsedTime < jetpackDuration)
        {
            // Smoothly move the player up
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            // Smoothly rotate the player to the target rotation
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / jetpackDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset player rotation after jetpack duration
        transform.rotation = initialRotation;
        isUsingJetpack = false;
    }
}