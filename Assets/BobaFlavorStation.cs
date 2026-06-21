using UnityEngine;
using TMPro;

public class BobaFlavorStation : MonoBehaviour
{
    [Header("Flavor Objects")]
    public GameObject[] flavorObjects;

    [Header("Flavor Text")]
    public TMP_Text flavorTitleText;
    public TMP_Text flavorDescriptionText;

    [Header("Optional Visuals")]
    public Renderer accentRenderer;
    public Color[] accentColors;

    [Header("Optional Audio")]
    public AudioSource clickSound;

    private int currentIndex = 0;

    void Start()
    {
        ShowFlavor(0);
    }

    public void NextFlavor()
    {
        if (flavorObjects == null || flavorObjects.Length == 0)
        {
            Debug.LogWarning("BobaFlavorStation: No flavor objects assigned.");
            return;
        }

        currentIndex++;

        if (currentIndex >= flavorObjects.Length)
        {
            currentIndex = 0;
        }

        ShowFlavor(currentIndex);

        if (clickSound != null)
        {
            clickSound.Play();
        }
    }

    void ShowFlavor(int index)
    {
        for (int i = 0; i < flavorObjects.Length; i++)
        {
            if (flavorObjects[i] != null)
            {
                flavorObjects[i].SetActive(i == index);
            }
        }

        if (flavorTitleText != null)
        {
            flavorTitleText.text = GetFlavorTitle(index);
        }

        if (flavorDescriptionText != null)
        {
            flavorDescriptionText.text = GetFlavorDescription(index);
        }

        if (accentRenderer != null && accentColors != null && index < accentColors.Length)
        {
            accentRenderer.material.color = accentColors[index];
        }
    }

    string GetFlavorTitle(int index)
    {
        switch (index)
        {
            case 0:
                return "Brown Sugar Boba";
            case 1:
                return "Taro Boba";
            case 2:
                return "Mango Boba";
            case 3:
                return "Classic Milk Tea";
            default:
                return "Boba";
        }
    }

    string GetFlavorDescription(int index)
    {
        switch (index)
        {
            case 0:
                return "Brown sugar boba became famous for its caramel-like syrup and striped cup design.";
            case 1:
                return "Taro boba uses a purple root flavor popular across many Asian dessert traditions.";
            case 2:
                return "Mango boba connects tropical fruit flavors with modern bubble tea culture.";
            case 3:
                return "Classic milk tea is the original base that helped bubble tea spread globally.";
            default:
                return "";
        }
    }
}