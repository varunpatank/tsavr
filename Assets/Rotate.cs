using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 30f;

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}