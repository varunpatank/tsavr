using UnityEngine;
using UnityEngine.InputSystem;

public class BobaSelector : MonoBehaviour
{
    public GameObject[] bobaVariants;
    private int currentIndex = -1;

    void Start()
    {
        foreach (GameObject boba in bobaVariants)
            boba.SetActive(false);
    }

    public void NextBoba()
    {
        currentIndex++;

        if (currentIndex >= bobaVariants.Length)
        {
            foreach (GameObject boba in bobaVariants)
                boba.SetActive(false);
            currentIndex = -1;
            return;
        }

        bobaVariants[currentIndex].SetActive(true);
    }
}