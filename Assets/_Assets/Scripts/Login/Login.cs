using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class Login : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _infoLogin;

    private void Start()
    {
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }    

    internal void ProcessAuthentication(SignInStatus status)
    {
        if(status == SignInStatus.Success)
        {
          
            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string imageUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

            _infoLogin.text = $"Name : {name} + id : {id}";

        }
        else
        {
            _infoLogin.text = $"❌ SignIn Failed!!\nStatus: {status.ToString()}";
        }    
    }    

}
