using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// Attach to a Canvas or sign GameObject near the museum exit.
/// The sign fades in when the player walks within triggerRadius.
/// If triggerRadius is 0, the sign is always fully visible.
public class ThankYouSign : MonoBehaviour
{
    [Header("── Sign Text ─────────────────────")]
    [Tooltip("TMP_Text component that will display the exit message.")]
    public TMP_Text signText;

    [TextArea(2, 5)]
    [Tooltip("The message shown on the exit sign.")]
    public string message = "Thank You for Visiting!";

    [Space]
    [Header("── Panel / Canvas Group ────────────")]
    [Tooltip("CanvasGroup of the sign panel — used for fading. Leave empty to skip fading.")]
    public CanvasGroup signPanel;

    [Range(1f, 10f)]
    [Tooltip("How fast the sign fades in when the player is nearby.")]
    public float fadeSpeed = 3f;

    [Space]
    [Header("── Proximity Trigger ────────────────")]
    [Range(0f, 25f)]
    [Tooltip("The sign fades in when the player is within this distance. Set to 0 to always show.")]
    public float triggerRadius = 6f;

    [Tooltip("Player camera used to measure proximity. Auto-fills with Camera.main.")]
    public Camera playerCamera;

    [Space]
    [Header("── Optional Entrance Animation ──────")]
    [Tooltip("Optional extra object to pulse/animate when the sign becomes visible (e.g. a glow).")]
    public GameObject glowObject;

    // ── private state ─────────────────────────────────────────────────────────
    private float targetAlpha = 1f;
    private bool wasVisible = false;

    // ─────────────────────────────────────────────────────────────────────────
    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (signText != null)
            signText.text = message;

        bool alwaysVisible = triggerRadius <= 0f;

        if (signPanel != null)
        {
            signPanel.gameObject.SetActive(true);
            signPanel.alpha = alwaysVisible ? 1f : 0f;
        }

        targetAlpha = alwaysVisible ? 1f : 0f;

        if (glowObject != null)
            glowObject.SetActive(alwaysVisible);
    }

    void Update()
    {
        if (triggerRadius > 0f && playerCamera != null)
        {
            float dist = Vector3.Distance(playerCamera.transform.position, transform.position);
            bool inRange = dist <= triggerRadius;

            targetAlpha = inRange ? 1f : 0f;

            // Trigger glow + pulse only on the moment we enter range
            if (inRange && !wasVisible)
            {
                if (glowObject != null) glowObject.SetActive(true);
                if (signText != null) StartCoroutine(PulseScale(signText.transform));
            }
            else if (!inRange && wasVisible)
            {
                if (glowObject != null) glowObject.SetActive(false);
            }

            wasVisible = inRange;
        }

        if (signPanel != null)
            signPanel.alpha = Mathf.MoveTowards(signPanel.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
    }

    // Small scale pop animation when the sign first appears
    IEnumerator PulseScale(Transform t)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 original = Vector3.one;

        while (elapsed < duration)
        {
            float s = 1f + 0.15f * Mathf.Sin(Mathf.PI * elapsed / duration);
            t.localScale = new Vector3(s, s, 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        t.localScale = original;
    }
}
