using UnityEngine;

[CreateAssetMenu(fileName = "NewCardSet", menuName = "Cards/Card Set")]
public class CardSet : ScriptableObject
{
    public CardData[] cards;
}
