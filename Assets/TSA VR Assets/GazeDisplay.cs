using UnityEngine;

// Attach to EACH scannable food object. Give that object a Collider (not trigger).
public class GazeDisplay : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;          // drag Main Camera here
    public CanvasGroup scrollPanel;      // drag the scroll's CanvasGroup here

    [Header("Tuning")]
    public float dwellTime = 0.5f;       // look this long to open
    public float maxDistance = 8f;
    public float fadeSpeed = 4f;         // higher = snappier
    public GameObject scanReticleGlow;   // optional: a glow/outline object to enable while dwelling

    float dwell = 0f;
    bool open = false;

    void Start()
    {
        if (playerCamera == null && Camera.main != null) playerCamera = Camera.main;
        if (scrollPanel != null) { scrollPanel.alpha = 0f; scrollPanel.gameObject.SetActive(true); }
        if (scanReticleGlow != null) scanReticleGlow.SetActive(false);
    }

    void Update()
    {
        if (playerCamera == null) return;

        bool lookingAtMe = false;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            lookingAtMe = (hit.transform == transform || hit.transform.IsChildOf(transform));

        if (lookingAtMe && !open)
        {
            dwell += Time.deltaTime;
            if (scanReticleGlow != null) scanReticleGlow.SetActive(true);
            if (dwell >= dwellTime) open = true;
        }
        else if (!lookingAtMe)
        {
            dwell = 0f;
            if (scanReticleGlow != null) scanReticleGlow.SetActive(false);
            open = false;
        }

        // Smooth fade toward target — no flicker
        if (scrollPanel != null)
        {
            float target = open ? 1f : 0f;
            scrollPanel.alpha = Mathf.MoveTowards(scrollPanel.alpha, target, fadeSpeed * Time.deltaTime);
        }
    }
}