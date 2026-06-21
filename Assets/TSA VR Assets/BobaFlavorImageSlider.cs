using UnityEngine;
using UnityEngine.UI;

public class BobaFlavorImageSlider : MonoBehaviour
{
    [Header("Slider")]
    public Slider slider;

    [Header("Changing Menu Image")]
    public Image menuImage;

    [Header("Flavor Sprites")]
    public Sprite[] flavorSprites;

    private int currentIndex = -1;

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = 3;
            slider.wholeNumbers = true;
            slider.onValueChanged.AddListener(ChangeImage);
            ChangeImage(slider.value);
        }
        else
        {
            Debug.LogWarning("BobaFlavorImageSlider: Slider is not assigned.");
        }
    }

    public void ChangeImage(float value)
    {
        int index = Mathf.Clamp(Mathf.RoundToInt(value), 0, 3);

        if (index == currentIndex)
        {
            return;
        }

        currentIndex = index;

        if (menuImage != null && flavorSprites != null && index < flavorSprites.Length && flavorSprites[index] != null)
        {
            menuImage.sprite = flavorSprites[index];
            menuImage.preserveAspect = true;
        }
    }
}