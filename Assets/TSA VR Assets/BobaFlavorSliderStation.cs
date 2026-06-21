using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class BobaFlavorSliderStation : MonoBehaviour
{
    [Header("── Slider ──────────────────────")]
    [Tooltip("The UI Slider controlling flavor selection. Auto-detected if left empty.")]
    public Slider flavorSlider;

    [Space]
    [Header("── Flavor GameObjects ───────────")]
    [Tooltip("One GameObject per flavor. Only the selected index will be active.")]
    public GameObject[] flavorObjects;

    [Space]
    [Header("── UI Text ─────────────────────")]
    [Tooltip("Large title text that displays the current flavor name.")]
    public TMP_Text flavorTitleText;
    [Tooltip("Smaller text describing the selected flavor's cultural context.")]
    public TMP_Text flavorDescriptionText;

    [Space]
    [Header("── Panel Fade ──────────────────")]
    [Tooltip("Assign the CanvasGroup of your info panel to get a smooth fade when switching flavors.")]
    public CanvasGroup infoPanel;
    [Tooltip("How fast the panel fades in and out (units of alpha per second).")]
    [Range(0.5f, 12f)]
    public float fadeSpeed = 6f;

    [Space]
    [Header("── Progress Dots ────────────────")]
    [Tooltip("One Image per flavor step — will be tinted to show which step is active.")]
    public Image[] progressDots;
    [Tooltip("Color applied to the currently active dot.")]
    public Color activeDotColor = Color.white;
    [Tooltip("Color applied to all inactive dots.")]
    public Color inactiveDotColor = new Color(1f, 1f, 1f, 0.25f);

    [Space]
    [Header("── Accent Color ─────────────────")]
    [Tooltip("Optional renderer whose material color changes per flavor.")]
    public Renderer accentRenderer;
    [Tooltip("One color per flavor index (should match the number of flavor objects).")]
    public Color[] accentColors;

    [Space]
    [Header("── Audio ────────────────────────")]
    [Tooltip("Sound played each time the slider moves to a new flavor.")]
    public AudioSource changeSound;

    [Space]
    [Header("── PC / Simulator Keyboard ───────")]
    [Tooltip("Allows Left/Right arrow keys and A/D to control the slider in the XR Device Simulator.")]
    public bool enableKeyboardControl = true;

    // ── private state ──────────────────────────────────────────────────────────
    private int currentIndex = -1;
    private float targetAlpha = 1f;
    private Coroutine pulseRoutine;

    // ──────────────────────────────────────────────────────────────────────────
    void Start()
    {
        if (flavorSlider == null)
            flavorSlider = GetComponent<Slider>();

        if (flavorSlider != null)
        {
            flavorSlider.minValue = 0;
            // auto-size max so you never have to hand-type it again
            if (flavorObjects != null && flavorObjects.Length > 0)
                flavorSlider.maxValue = flavorObjects.Length - 1;
            else
                flavorSlider.maxValue = 3;

            flavorSlider.wholeNumbers = true;
            flavorSlider.onValueChanged.AddListener(OnSliderChanged);
            OnSliderChanged(flavorSlider.value);
        }
        else
        {
            Debug.LogWarning("BobaFlavorSliderStation: No Slider assigned or found on this GameObject.");
        }

        if (infoPanel != null)
            infoPanel.alpha = 1f;
    }

    void Update()
    {
        // Smooth panel alpha toward target each frame
        if (infoPanel != null)
            infoPanel.alpha = Mathf.MoveTowards(infoPanel.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

        // Keyboard fallback for PC / XR Device Simulator
        if (enableKeyboardControl && flavorSlider != null)
        {
            var kb = Keyboard.current;
            if (kb != null)
            {
                if (kb.rightArrowKey.wasPressedThisFrame || kb.dKey.wasPressedThisFrame)
                    flavorSlider.value = Mathf.Min(flavorSlider.value + 1, flavorSlider.maxValue);
                else if (kb.leftArrowKey.wasPressedThisFrame || kb.aKey.wasPressedThisFrame)
                    flavorSlider.value = Mathf.Max(flavorSlider.value - 1, flavorSlider.minValue);
            }
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    public void OnSliderChanged(float value)
    {
        int index = Mathf.RoundToInt(value);
        if (index == currentIndex) return;
        currentIndex = index;

        UpdateProgressDots(index);

        if (changeSound != null) changeSound.Play();

        if (infoPanel != null)
            StartCoroutine(FadeAndUpdate(index));
        else
            ShowFlavor(index);
    }

    // Fade the panel out, swap content, fade back in with a title pulse
    IEnumerator FadeAndUpdate(int index)
    {
        targetAlpha = 0f;
        yield return new WaitUntil(() => infoPanel == null || infoPanel.alpha <= 0.05f);

        ShowFlavor(index);
        targetAlpha = 1f;

        if (flavorTitleText != null)
        {
            if (pulseRoutine != null) StopCoroutine(pulseRoutine);
            pulseRoutine = StartCoroutine(PulseScale(flavorTitleText.transform));
        }
    }

    // Quick scale pop on the title text when a new flavor is selected
    IEnumerator PulseScale(Transform t)
    {
        float duration = 0.22f;
        float elapsed = 0f;
        Vector3 original = Vector3.one;

        while (elapsed < duration)
        {
            float s = 1f + 0.13f * Mathf.Sin(Mathf.PI * elapsed / duration);
            t.localScale = new Vector3(s, s, 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        t.localScale = original;
    }

    void ShowFlavor(int index)
    {
        if (flavorObjects != null)
        {
            for (int i = 0; i < flavorObjects.Length; i++)
            {
                if (flavorObjects[i] != null)
                    flavorObjects[i].SetActive(i == index);
            }
        }

        if (flavorTitleText != null)       flavorTitleText.text       = GetFlavorTitle(index);
        if (flavorDescriptionText != null) flavorDescriptionText.text = GetFlavorDescription(index);

        if (accentRenderer != null && accentColors != null && index < accentColors.Length)
            accentRenderer.material.color = accentColors[index];
    }

    void UpdateProgressDots(int index)
    {
        if (progressDots == null) return;
        for (int i = 0; i < progressDots.Length; i++)
        {
            if (progressDots[i] != null)
                progressDots[i].color = (i == index) ? activeDotColor : inactiveDotColor;
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    string GetFlavorTitle(int index)
    {
        switch (index)
        {
            case 0:  return "Brown Sugar Boba";
            case 1:  return "Taro Boba";
            case 2:  return "Mango Boba";
            case 3:  return "Classic Milk Tea";
            default: return "Boba Tea";
        }
    }

    string GetFlavorDescription(int index)
    {
        switch (index)
        {
            case 0:
                return "Brown sugar boba is known for its caramel syrup and striped cup design. It shows how visual presentation became part of modern boba culture.";
            case 1:
                return "Taro boba uses a purple root flavor common in many Asian desserts. Its color and sweetness helped make boba feel playful and recognizable.";
            case 2:
                return "Mango boba connects tropical fruit flavors with bubble tea culture, showing how the drink adapts to local tastes around the world.";
            case 3:
                return "Classic milk tea is the foundation of boba. Tea, milk, and tapioca pearls helped turn a local Taiwanese drink into a global café experience.";
            default:
                return "";
        }
    }
}