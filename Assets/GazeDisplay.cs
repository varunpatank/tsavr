using UnityEngine;

public class GazeDisplay : MonoBehaviour
{
    [Header("Settings")]
    public float gazeTimeRequired = 2f;
    public GameObject uiPanel;
    public Camera playerCamera;

    private float gazeTimer = 0f;
    private bool playerInZone = false;
    private bool panelVisible = false;

    // This tells the raycast to ignore the DisplayCase layer
    private int ignoreLayer;

    void Start()
    {
        ignoreLayer = ~LayerMask.GetMask("DisplayCase");
    }

    void Update()
    {
        if (!playerInZone) return;

        Ray ray = new Ray(playerCamera.transform.position,
                         playerCamera.transform.forward);
        RaycastHit hit;

        // Raycast now ignores anything on DisplayCase layer
        if (Physics.Raycast(ray, out hit, 10f, ignoreLayer))
        {
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
            {
                gazeTimer += Time.deltaTime;
                if (gazeTimer >= gazeTimeRequired && !panelVisible)
                    ShowPanel();
            }
            else
            {
                gazeTimer = 0f;
                if (panelVisible) HidePanel();
            }
        }
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
            HidePanel();
        }
    }

    void ShowPanel()
    {
        panelVisible = true;
        uiPanel.SetActive(true);
    }

    void HidePanel()
    {
        panelVisible = false;
        uiPanel.SetActive(false);
    }
}