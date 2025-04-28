using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Result UI")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    private const int PistiPoints = 10;
    private const int MostCardsBonus = 3;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CalculateScores(List<Player> players)
    {
        Dictionary<Player, int> scores = new Dictionary<Player, int>();

        foreach (var player in players)
        {
            int totalScore = CalculatePlayerScore(player);
            scores[player] = totalScore;
        }

        AwardMostCardsBonus(players, scores);

        Player winner = FindWinner(scores);

        ShowResults(scores, winner);
    }

    private int CalculatePlayerScore(Player player)
    {
        int totalScore = 0;

        foreach (var cardData in player.collectedCards)
        {
            totalScore += cardData.score;
        }

        totalScore += player.pistiCount * PistiPoints;

        return totalScore;
    }

    private void AwardMostCardsBonus(List<Player> players, Dictionary<Player, int> scores)
    {
        Player mostCardsPlayer = null;
        int mostCards = -1;

        foreach (var player in players)
        {
            if (player.collectedCards.Count > mostCards)
            {
                mostCards = player.collectedCards.Count;
                mostCardsPlayer = player;
            }
        }

        if (mostCardsPlayer != null)
        {
            scores[mostCardsPlayer] += MostCardsBonus;
            Debug.Log($"{mostCardsPlayer.name} en çok kart topladığı için +{MostCardsBonus} puan aldı.");
        }
    }

    private Player FindWinner(Dictionary<Player, int> scores)
    {
        Player winner = null;
        int highestScore = -1;

        foreach (var kvp in scores)
        {
            Debug.Log($"{kvp.Key.name} Toplam Puan: {kvp.Value}");

            if (kvp.Value > highestScore)
            {
                highestScore = kvp.Value;
                winner = kvp.Key;
            }
        }

        if (winner != null)
        {
            Debug.Log($"Kazanan: {winner.name} {highestScore} puanla!");
        }

        return winner;
    }

    private void ShowResults(Dictionary<Player, int> scores, Player winner)
    {
        resultPanel.SetActive(true);

        string resultMessage = "";

        foreach (var kvp in scores)
        {
            Player player = kvp.Key;
            int score = kvp.Value;
            int totalCards = player.collectedCards.Count;

            int playerIndex = GameManager.Instance.players.IndexOf(player);
            string displayName = $"Player {playerIndex + 1}";

            int aceCount = 0;
            int jackCount = 0;
            int clubTwoCount = 0;
            int diamondTenCount = 0;

            foreach (var cardData in player.collectedCards)
            {
                if (cardData.value == CardValue.Ace)
                    aceCount++;
                else if (cardData.value == CardValue.Jack)
                    jackCount++;
                else if (cardData.value == CardValue.Two && cardData.type == CardType.Clubs)
                    clubTwoCount++;
                else if (cardData.value == CardValue.Ten && cardData.type == CardType.Diamonds)
                    diamondTenCount++;
            }

            resultMessage += $"{displayName}  -  Skor: {score}  |  Pişti: {player.pistiCount}  |  Kartlar: {totalCards}\n";
            resultMessage += $"(As: {aceCount}, Vale: {jackCount}, 2♣: {clubTwoCount}, 10♦: {diamondTenCount})\n\n";
        }

        int winnerIndex = GameManager.Instance.players.IndexOf(winner);
        string winnerDisplayName = $"Player {winnerIndex + 1}";

        resultMessage += $"🏆 Kazanan: {winnerDisplayName}";

        resultText.text = resultMessage;

    }
}
