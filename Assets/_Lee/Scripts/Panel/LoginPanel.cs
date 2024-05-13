using Firebase.Database;
using Firebase.Extensions;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField nickNameField;
    [SerializeField] TMP_InputField emailField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] Button loginButton;
    [SerializeField] Button editButton;
    [SerializeField] Button endButton;

    [SerializeField] LobbyManager manager;
    private void Awake()
    {
        manager = FindObjectOfType<LobbyManager>();
    }
    private void Start()
    {
        loginButton.onClick.AddListener(() => Login()); // 로그인버튼 누르면 로그인 되도록
        editButton.onClick.AddListener(() => SingUp());
        endButton.onClick.AddListener(EndGame); 
    }

    // 로그인해서 네트워크한테 메인상태로 만들어줌
    private void Login()
    {
        string email = emailField.text;
        string pass = passwordField.text;

        FirebaseManager.Auth.SignInWithEmailAndPasswordAsync(email, pass).ContinueWithOnMainThread(task =>
        {
            if ( task.IsCanceled )
            {
                manager.ShowInfo("로그인이 취소됨");
                SetInteractable(true);
                return;
            }
            else if ( task.IsFaulted )
            {
                manager.ShowInfo($"로그인 실패 : {task.Exception.Message}");
                SetInteractable(true);
                return;
            }
            FirebaseManager.DB.GetReference($"UserDate/{email}/nickName").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                DataSnapshot snapshot = task.Result;
                if ( snapshot.Exists )
                {
                    string json = snapshot.GetValue(true).ToString();

                    PhotonNetwork.LocalPlayer.NickName = json;// 이부분은 DB에 닉네임 저장하고 불러오기로 할 예정
                }
                else
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                }
            });
            PhotonNetwork.ConnectUsingSettings();
            SetInteractable(true);
        });
    }
    private void SingUp()
    {
        manager.SetActivePanel(LobbyManager.Panel.SignUp);
    }
    private void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit(); // 어플리케이션 종료
#endif
    }
    private void SetInteractable( bool interactable )
    {
        emailField.interactable = interactable;
        nickNameField.interactable = interactable;
        passwordField.interactable = interactable;
        loginButton.interactable = interactable;
        editButton.interactable = interactable;
        endButton.interactable = interactable;
    }
}