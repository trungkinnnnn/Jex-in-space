using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    private PlayerAudio _audio;
    // HUD Controller
    public static System.Action<int> OnActionCoin;
    public static System.Action<int , AsteroidType> OnActionScore;

    public int coinTotal = 0;
    public int scoreTotal = 0;

    private void Awake()
    {
        _audio = GetComponent<PlayerAudio>();
    }


    public void OnEnable()
    {
        Ast.AddScoreOnDie += HandleAddScore;
    }

    public void OnDisable()
    {
        Ast.AddScoreOnDie -= HandleAddScore;
    }

    private void HandleAddScore(int score, AsteroidType type)
    {
        scoreTotal += score;
        OnActionScore?.Invoke(scoreTotal, type);  
    }

    public void HandleAddCoin(int amount)
    {
        _audio.PlayClipTakeCoin();
        coinTotal += amount;
        OnActionCoin?.Invoke(coinTotal);
    }
}
