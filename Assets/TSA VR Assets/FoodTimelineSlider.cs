using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodTimelineSlider : MonoBehaviour
{
    public Slider slider;
    public GameObject[] stageVariants;
    public TMP_Text captionText;
    [TextArea] public string[] captions;

    void Start()
    {
        if (slider == null) slider = GetComponent<Slider>();

        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderChanged);
        else
            Debug.LogWarning("FoodTimelineSlider: No Slider assigned or found.");
    }

    void OnSliderChanged(float value)
    {
        int index = Mathf.RoundToInt(value);

        if (stageVariants != null)
        {
            for (int i = 0; i < stageVariants.Length; i++)
            {
                if (stageVariants[i] != null)
                    stageVariants[i].SetActive(i == index);
            }
        }

        if (captionText != null && captions != null && index >= 0 && index < captions.Length)
            captionText.text = captions[index];
    }
}
