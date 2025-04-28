using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Dropdown playerCountDropdown;
    public TMP_Dropdown aiDifficultyDropdown;

    public GameObject mainMenuPanel;
    public GameObject gameUIPanel;

    public GameObject playerPrefab;
    public GameObject aiPlayerPrefab;

    public Transform playersParent;
    public List<Transform> handAreas;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        gameUIPanel.SetActive(false);
    }

    public void StartGame()
    {
        ApplySettings();
        SetupPlayers();
        SetupUI();

        DeckManager.Instance.DealInitialHands();

        SetInitialHandVisibility();

        StartCoroutine(DelayedStart());
    }

    private void ApplySettings()
    {
        int selectedPlayerCount = playerCountDropdown.value == 0 ? 2 : 4;
        AIDifficulty selectedDifficulty = aiDifficultyDropdown.value == 0 ? AIDifficulty.Easy : AIDifficulty.Hard;

        MatchSettings.Instance.playerCount = selectedPlayerCount;
        MatchSettings.Instance.selectedDifficulty = selectedDifficulty;
    }

    private void SetupPlayers()
    {
        GameManager.Instance.players.Clear();

        for (int i = 0; i < MatchSettings.Instance.playerCount; i++)
        {
            GameObject playerObj = Instantiate(
                i == 0 ? playerPrefab : aiPlayerPrefab,
                playersParent
            );

            if (i != 0)
            {
                playerObj.GetComponent<AIPlayer>().difficulty = MatchSettings.Instance.selectedDifficulty;
            }

            Player player = playerObj.GetComponent<Player>();
            player.handArea = handAreas[i];
            GameManager.Instance.players.Add(player);
        }
    }

    private void SetupUI()
    {
        mainMenuPanel.SetActive(false);
        gameUIPanel.SetActive(true);

        GameUIManager.Instance.SetActivePlayerTexts(MatchSettings.Instance.playerCount);
    }

    private void SetInitialHandVisibility()
    {
        foreach (var player in GameManager.Instance.players)
        {
            if (player is AIPlayer)
                player.RevealHand(false);
            else
                player.RevealHand(true);
        }
    }

    private IEnumerator DelayedStart()
    {
        yield return null;
        GameManager.Instance.SetActivePlayer(0);
    }
}
