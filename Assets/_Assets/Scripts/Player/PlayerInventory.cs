using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // HUD Controller
    public static System.Action<int> OnActionCoin;
    public static System.Action<int> OnActionScore;

    public int coinTotal = 0;
    public int scoreTotal = 0;
   
    public void OnEnable()
    {
        Ast.AddScoreOnDie += HandleAddScore;
    }

    public void OnDisable()
    {
        Ast.AddScoreOnDie -= HandleAddScore;
    }

    private void HandleAddScore(int score)
    {
        scoreTotal += score;
        OnActionScore?.Invoke(scoreTotal);  
    }

    public void HandleAddCoin(int amount)
    {
        coinTotal += amount;
        OnActionCoin?.Invoke(coinTotal);
    }
}
