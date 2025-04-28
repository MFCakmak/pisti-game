using UnityEngine;

public class MatchSettings : MonoBehaviour
{
    public static MatchSettings Instance;

    [Header("Match Settings")]
    public int playerCount = 2;
    public AIDifficulty selectedDifficulty = AIDifficulty.Easy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
