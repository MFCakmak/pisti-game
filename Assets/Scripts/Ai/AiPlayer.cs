using System.Collections;
using UnityEngine;

public class AIPlayer : Player
{
    public AIDifficulty difficulty = AIDifficulty.Easy;

    public void PlayTurn()
    {
        StartCoroutine(PlayAfterDelay());
    }

    private IEnumerator PlayAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        switch (difficulty)
        {
            case AIDifficulty.Easy:
                EasyPlay();
                break;
            case AIDifficulty.Hard:
                HardPlay();
                break;
        }
    }

    private void EasyPlay()
    {
        Card topCard = TableManager.Instance.GetTopCard();
        int tableCardCount = TableManager.Instance.TableCardCount();

        Card matchingCard = FindMatchingCard(topCard);
        if (matchingCard != null)
        {
            matchingCard.OnPointerClick(null);
            return;
        }

        Card jackCard = FindJackCard();
        if (jackCard != null && (tableCardCount >= 2 || TableHasValuableCard()))
        {
            jackCard.OnPointerClick(null);
            return;
        }
        PlayRandomCard();
    }


    private void HardPlay()
    {
        Card topCard = TableManager.Instance.GetTopCard();
        int tableCardCount = TableManager.Instance.TableCardCount();

        bool hasValuable = TableHasValuableCard();
        bool deckIsLow = DeckManager.Instance.GetRemainingCardCount() <= 10;

        Card matchingCard = FindMatchingCard(topCard);
        if (matchingCard != null)
        {
            matchingCard.OnPointerClick(null);
            return;
        }

        Card jackCard = FindJackCard();
        if (jackCard != null && (tableCardCount >= 4 || hasValuable || deckIsLow))
        {
            jackCard.OnPointerClick(null);
            return;
        }

        Card lowestCard = FindLowestScoreCard();
        if (lowestCard != null)
        {
            lowestCard.OnPointerClick(null);
        }
    }

    private Card FindMatchingCard(Card topCard)
    {
        if (topCard == null) return null;

        foreach (var card in handCards)
        {
            if (card.cardData.value == topCard.cardData.value)
                return card;
        }
        return null;
    }

    private Card FindJackCard()
    {
        foreach (var card in handCards)
        {
            if (card.cardData.value == CardValue.Jack)
                return card;
        }
        return null;
    }

    private Card FindLowestScoreCard()
    {
        Card lowestCard = null;
        int lowestScore = int.MaxValue;

        foreach (var card in handCards)
        {
            if (card.cardData.score < lowestScore)
            {
                lowestScore = card.cardData.score;
                lowestCard = card;
            }
        }
        return lowestCard;
    }

    private bool TableHasValuableCard()
    {
        foreach (var card in TableManager.Instance.tableCards)
        {
            if (card.cardData.score > 0)
                return true;
        }
        return false;
    }

    private void PlayRandomCard()
    {
        if (handCards.Count > 0)
        {
            handCards[Random.Range(0, handCards.Count)].OnPointerClick(null);
        }
    }

}
