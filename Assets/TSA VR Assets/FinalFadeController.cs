using UnityEngine;

public class FinalFadeController : MonoBehaviour
{
    public Renderer fadePanel;
    public float fadeInSpeed = 0.5f;

    private bool fading = false;
    private float alpha = 0f;

    void Start()
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(false);
        }
    }

    public void BeginFinalFade()
    {
        if (fadePanel == null)
        {
            Debug.LogWarning("FinalFadeController: Fade Panel is not assigned.");
            return;
        }

        fadePanel.gameObject.SetActive(true);
        fading = true;
    }

    void Update()
    {
        if (!fading || fadePanel == null)
        {
            return;
        }

        alpha = Mathf.MoveTowards(alpha, 1f, fadeInSpeed * Time.deltaTime);

        Color color = fadePanel.material.color;
        color.a = alpha;
        fadePanel.material.color = color;
    }
}