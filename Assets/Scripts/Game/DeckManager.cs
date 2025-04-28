using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [Header("Deck Settings")]
    public CardSet cardSet;
    public GameObject cardPrefab;

    [Header("Areas")]
    public Transform deckArea;
    public Transform tableArea;

    private List<CardData> deck = new List<CardData>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InitializeDeck();
        SetupInitialTable();
        DealInitialHands();
        ShowDeckTopCard();
    }

    private void InitializeDeck()
    {
        deck = new List<CardData>(cardSet.cards);
        ShuffleDeck();
    }

    private void ShuffleDeck()
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }

    private void SetupInitialTable()
    {
        for (int i = 0; i < 3; i++)
        {
            Card hiddenCard = SpawnCard(tableArea, faceUp: false);
            TableManager.Instance.AddCard(hiddenCard);
        }

        Card openCard = SpawnCard(tableArea, faceUp: true);
        TableManager.Instance.AddCard(openCard);
    }

    public void DealInitialHands()
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (var player in GameManager.Instance.players)
            {
                DealCardToPlayer(player);
            }
        }
    }

    public void DealCardToPlayer(Player player)
    {
        if (IsDeckEmpty()) return;

        CardData cardData = deck[0];
        deck.RemoveAt(0);

        GameObject cardGO = Instantiate(cardPrefab, player.handArea);
        Card card = cardGO.GetComponent<Card>();
        card.cardData = cardData;
        card.playArea = tableArea;
        card.owner = player;

        player.AddCard(card);

        if (player is AIPlayer)
            card.ShowBack();
        else
            card.ShowFront();
    }

    private Card SpawnCard(Transform parent, bool faceUp)
    {
        if (IsDeckEmpty()) return null;

        CardData cardData = deck[0];
        deck.RemoveAt(0);

        GameObject cardGO = Instantiate(cardPrefab, parent);
        Card card = cardGO.GetComponent<Card>();
        card.cardData = cardData;
        card.playArea = tableArea;

        if (faceUp)
            card.ShowFront();
        else
            card.ShowBack();

        return card;
    }

    private void ShowDeckTopCard()
    {
        if (deck.Count == 0) return;

        GameObject deckCard = Instantiate(cardPrefab, deckArea);
        Card card = deckCard.GetComponent<Card>();
        card.cardData = deck[0];
        card.artworkImage.enabled = false;
    }

    public bool IsDeckEmpty()
    {
        return deck.Count == 0;
    }

    public int GetRemainingCardCount()
    {
        return deck.Count;
    }
}
