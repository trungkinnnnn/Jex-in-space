using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] Button _showLeaderboardBtn;

    public static LeaderboardManager Instance;
    private void Start()
    {
        if (Instance == null) Instance = this;

        if (_showLeaderboardBtn != null)
            _showLeaderboardBtn.onClick.AddListener(() => ShowLeaderboard());
    }

    private void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void PushHighScore(int highScore)
    {
        Social.ReportScore(highScore, GPGSIds.leaderboard_high_score, success => { Debug.Log("Push Score"); });
    }

}
