using UnityEngine;

public class GazeDisplay : MonoBehaviour
{
    [Header("Settings")]
    public float gazeTimeRequired = 2f;
    public GameObject uiPanel;
    public Camera playerCamera;
    public float hideDelay = 30f;
    public float fadeDuration = 1f;

    private float gazeTimer = 0f;
    private float hideTimer = 0f;
    private bool playerInZone = false;
    private bool panelVisible = false;
    private float fadeTimer = 0f;
    private bool fadingIn = false;
    private bool fadingOut = false;
    private Renderer panelRenderer;

    void Start()
    {
        panelRenderer = uiPanel.GetComponent<Renderer>();
        // Start fully invisible
        SetAlpha(0f);
        uiPanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInZone)
        {
            if (panelVisible) StartFadeOut();
            HandleFade();
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position,
                         playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
            {
                gazeTimer += Time.deltaTime;
                hideTimer = 0f;
                fadingOut = false;

                if (gazeTimer >= gazeTimeRequired && !panelVisible)
                    StartFadeIn();
            }
            else
            {
                gazeTimer = 0f;
                if (panelVisible)
                {
                    hideTimer += Time.deltaTime;
                    if (hideTimer >= hideDelay)
                        StartFadeOut();
                }
            }
        }
        else
        {
            gazeTimer = 0f;
            if (panelVisible)
            {
                hideTimer += Time.deltaTime;
                if (hideTimer >= hideDelay)
                    StartFadeOut();
            }
        }

        HandleFade();
    }

    void HandleFade()
    {
        if (fadingIn)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            SetAlpha(alpha);
            if (alpha >= 1f)
                fadingIn = false;
        }
        else if (fadingOut)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (fadeTimer / fadeDuration));
            SetAlpha(alpha);
            if (alpha <= 0f)
            {
                fadingOut = false;
                panelVisible = false;
                uiPanel.SetActive(false);
            }
        }
    }

    void SetAlpha(float alpha)
    {
        if (panelRenderer != null)
        {
            Color color = panelRenderer.material.color;
            color.a = alpha;
            panelRenderer.material.color = color;
        }
    }

    void StartFadeIn()
    {
        panelVisible = true;
        fadingIn = true;
        fadingOut = false;
        fadeTimer = 0f;
        hideTimer = 0f;
        uiPanel.SetActive(true);
        SetAlpha(0f);
    }

    void StartFadeOut()
    {
        fadingOut = true;
        fadingIn = false;
        fadeTimer = 0f;
        hideTimer = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered zone!");
            playerInZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            gazeTimer = 0f;
            StartFadeOut();
        }
    }
}