using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public static TableManager Instance;

    [Header("Table Cards")]
    public List<Card> tableCards = new List<Card>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCard(Card card)
    {
        tableCards.Add(card);
    }

    public void ClearTable()
    {
        foreach (var card in tableCards)
        {
            if (card != null)
            {
                Destroy(card.gameObject);
            }
        }

        tableCards.Clear();
    }

    public Card GetTopCard()
    {
        return tableCards.Count > 0 ? tableCards[^1] : null;
    }

    public int TableCardCount()
    {
        return tableCards.Count;
    }

    public List<Card> CollectTableCards()
    {
        List<Card> collected = new List<Card>(tableCards);
        tableCards.Clear();
        return collected;
    }
}
