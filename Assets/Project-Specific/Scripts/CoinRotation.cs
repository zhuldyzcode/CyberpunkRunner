using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    private void Update()
    {
        // �������� ������ ������ ��� Y
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}