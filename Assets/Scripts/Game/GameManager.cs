using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Players")]
    public List<Player> players = new List<Player>();
    public int activePlayerIndex = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetActivePlayer(int index)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetActive(i == index);

            if (players[i] is AIPlayer)
            {
                players[i].RevealHand(false); // AI kartları her zaman kapalı
            }
            else
            {
                players[i].RevealHand(true); // İnsan kartları her zaman açık
            }
        }

        activePlayerIndex = index;
    }

    public void NextPlayer()
    {
        activePlayerIndex = (activePlayerIndex + 1) % players.Count;
        SetActivePlayer(activePlayerIndex);

        Debug.Log($"Sıra değişti: {players[activePlayerIndex].gameObject.name}");

        if (players[activePlayerIndex] is AIPlayer aiPlayer)
        {
            aiPlayer.PlayTurn();
        }

        if (AreAllHandsEmpty())
        {
            if (IsDeckEmpty())
                FinishGame();
            else
                DealNewHands();
        }
    }

    private void UpdatePlayerInteractability()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetActive(i == activePlayerIndex);
        }
    }

    private void UpdateHandVisibility()
    {
        foreach (var player in players)
        {
            if (player is AIPlayer)
                player.RevealHand(false);
            else
                player.RevealHand(player == players[activePlayerIndex]);
        }
    }

    public bool AreAllHandsEmpty()
    {
        foreach (var player in players)
        {
            if (player.handCards.Count > 0)
                return false;
        }
        return true;
    }

    public void DealNewHands()
    {
        StartCoroutine(DealNewHandsCoroutine());
    }

    private IEnumerator DealNewHandsCoroutine()
    {
        Debug.Log("Yeni kartlar dağıtılıyor...");

        for (int i = 0; i < 4; i++)
        {
            foreach (var player in players)
            {
                if (!IsDeckEmpty())
                    DeckManager.Instance.DealCardToPlayer(player);
            }
        }

        yield return null; // Bir frame bekle

        UpdateHandVisibility();
    }

    public void FinishGame()
    {
        Debug.Log("Oyun bitiyor! Kalan masa kartları toplanıyor...");

        Player lastPlayer = players[(activePlayerIndex - 1 + players.Count) % players.Count];
        List<Card> remainingCards = TableManager.Instance.CollectTableCards();

        foreach (var card in remainingCards)
        {
            lastPlayer.collectedCards.Add(card.cardData);
            Destroy(card.gameObject);
        }

        Debug.Log($"{lastPlayer.gameObject.name} kalan masa kartlarını aldı!");

        ScoreManager.Instance.CalculateScores(players);
    }

    public bool IsDeckEmpty()
    {
        return DeckManager.Instance.IsDeckEmpty();
    }
}
