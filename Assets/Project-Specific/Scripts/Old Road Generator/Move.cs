using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Ground groundInitiator;
    void Start()
    {
        groundInitiator = gameObject.transform.GetComponentInParent<Ground>();
    }

    // Update is called once per frame

    private void LateUpdate()
    {
        if (groundInitiator.move)
        {
            float distance = Time.deltaTime * groundInitiator.speed;
            transform.position += Vector3.back * distance;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            Destroy(gameObject);
            groundInitiator.OnSegmentDestroyed();
        }
    }
}