using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public CardData cardData;
    public Sprite backSprite;
    private Sprite frontSprite;

    public Image artworkImage;
    public Transform playArea;

    private bool isInHand = true;
    public Player owner;

    private void Start()
    {
        if (cardData != null && artworkImage != null)
        {
            frontSprite = cardData.artwork;
            artworkImage.sprite = frontSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInHand) return;

        if (!IsPlayerTurn())
        {
            return;
        }

        Debug.Log($"Played: {cardData.cardName}");
        PlayCard();
    }

    private bool IsPlayerTurn()
    {
        return GameManager.Instance.players[GameManager.Instance.activePlayerIndex] == owner;
    }

    private void PlayCard()
    {
        if (playArea == null) return;

        transform.SetParent(playArea);
        transform.position = playArea.position;
        isInHand = false;
        ShowFront();

        bool collected = TryCollectCards();

        if (!collected)
        {
            TableManager.Instance.AddCard(this);
        }

        owner.RemoveCard(this);
        GameManager.Instance.NextPlayer();
    }

    private bool TryCollectCards()
    {
        Card topCard = TableManager.Instance.GetTopCard();

        if (topCard == null) return false;

        if (cardData.value == CardValue.Jack)
        {
            Debug.Log("Vale ile tüm kartlar toplandı");
            CollectTableAndSelf();
            return true;
        }

        if (cardData.value == topCard.cardData.value)
        {
            if (TableManager.Instance.TableCardCount() == 1)
            {
                Debug.Log("Pişti yapıldı");
                owner.pistiCount++;
            }
            else
            {
                Debug.Log("Normal toplama yapıldı");
            }

            CollectTableAndSelf();
            return true;
        }

        return false;
    }

    private void CollectTableAndSelf()
    {
        List<Card> collectedCards = TableManager.Instance.CollectTableCards();
        collectedCards.Add(this);

        foreach (var card in collectedCards)
        {
            owner.collectedCards.Add(card.cardData);
            Destroy(card.gameObject);
        }
    }

    public void ShowFront()
    {
        if (artworkImage != null && frontSprite != null)
            artworkImage.sprite = frontSprite;
    }

    public void ShowBack()
    {
        if (artworkImage != null && backSprite != null)
            artworkImage.sprite = backSprite;
    }

    public void SetInteractable(bool interactable)
    {
        GetComponent<Button>().interactable = interactable;
    }
}
