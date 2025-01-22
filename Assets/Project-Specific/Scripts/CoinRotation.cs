using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    private void Update()
    {
        // Вращение монеты вокруг оси Y
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}