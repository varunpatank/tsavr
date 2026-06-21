using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInteractionFeedback : MonoBehaviour
{
    public AudioSource clickSound;
    public GameObject hoverGlow;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        if (interactable == null)
        {
            Debug.LogWarning("VRInteractionFeedback: No XRBaseInteractable found on this GameObject.");
            return;
        }

        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
        interactable.selectEntered.AddListener(OnSelectEntered);
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (hoverGlow != null) hoverGlow.SetActive(true);
    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        if (hoverGlow != null) hoverGlow.SetActive(false);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (clickSound != null) clickSound.Play();
    }

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
            interactable.selectEntered.RemoveListener(OnSelectEntered);
        }
    }
}
