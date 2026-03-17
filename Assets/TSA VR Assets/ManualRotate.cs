using UnityEngine;
using UnityEngine.UI;

public class ManualRotate : MonoBehaviour
{
    public Slider rotationSlider;
    public GameObject foodObject;

    private Quaternion initialRotation;

    void Start()
    {
        // Store the original rotation
        initialRotation = foodObject.transform.rotation;
    }

    void Update()
    {
        if (foodObject != null && rotationSlider != null)
        {
            // Apply rotation ON TOP of the initial rotation
            float angle = rotationSlider.value;
            foodObject.transform.rotation = initialRotation * 
                Quaternion.Euler(0, angle, 0);
        }
    }
}