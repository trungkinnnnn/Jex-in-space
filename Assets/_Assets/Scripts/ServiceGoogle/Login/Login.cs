using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class Login : MonoBehaviour
{
    private void Start()
    {
        PlayGamesPlatform.Activate();
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
            Debug.Log("Login Success with name : " + name);
        }
        else
        {
            Debug.Log("Login failed : " + status.ToString());

        }    
    }    

}
