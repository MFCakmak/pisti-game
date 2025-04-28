using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Properties")]
    public List<Card> handCards = new List<Card>();
    public List<CardData> collectedCards = new List<CardData>();
    public Transform handArea;
    public int pistiCount = 0;

    public void SetActive(bool active)
    {
        foreach (var card in handCards)
        {
            card.SetInteractable(active);
        }
    }

    public void AddCard(Card card)
    {
        handCards.Add(card);
    }

    public void RemoveCard(Card card)
    {
        handCards.Remove(card);
    }

    public void CollectCards(List<Card> cards)
    {
        foreach (var card in cards)
        {
            collectedCards.Add(card.cardData);
        }
    }

    public void RevealHand(bool reveal)
    {
        foreach (var card in handCards)
        {
            if (reveal)
                card.ShowFront();
            else
                card.ShowBack();
        }
    }
}
