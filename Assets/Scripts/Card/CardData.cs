using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType type;
    public CardValue value;
    public int score;
    public Sprite artwork;
}
