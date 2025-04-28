using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    public List<GameObject> playerNameTexts = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetActivePlayerTexts(int playerCount)
    {
        for (int i = 0; i < playerNameTexts.Count; i++)
        {
            if (i < playerCount)
            {
                playerNameTexts[i].SetActive(true);
            }
            else
            {
                playerNameTexts[i].SetActive(false);
            }
        }
    }
}
