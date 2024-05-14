using Firebase.Extensions;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class SignUpPanel : MonoBehaviour
{
    [SerializeField] LobbyManager lobbymanager;

    [SerializeField] TMP_InputField nickNameInput;
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_InputField confirmInputInput;

    [SerializeField] Button cancel;
    [SerializeField] Button singUp;

    private void Awake()
    {
        lobbymanager = FindObjectOfType<LobbyManager>();
        singUp.onClick.AddListener(SingUp);
        cancel.onClick.AddListener(Cancel);
    }

    public void SingUp()
    {
        SetInteractable(false);

        string nickName = nickNameInput.text;
        string email = emailInput.text;
        string pass = passwordInput.text;
        string confirm = confirmInputInput.text;
        if ( pass != confirm )
        {
            lobbymanager.ShowInfo("비밀번호가 같지 않음");
            SetInteractable(true);
            return;
        }
        FirebaseManager.Auth.CreateUserWithEmailAndPasswordAsync(email, pass).ContinueWithOnMainThread(task =>
        {
            if ( task.IsCanceled )
            {
                lobbymanager.ShowInfo("회원가입 취소됨");
                SetInteractable(true);
                return;
            }
            else if ( task.IsFaulted )
            {
                lobbymanager.ShowInfo($"회원가입 실패함 : {task.Exception.Message}");
                SetInteractable(true);
                return;
            }
            lobbymanager.ShowInfo("회원가입 성공");
            lobbymanager.SetActivePanel(LobbyManager.Panel.Login);
            SetInteractable(true);
        });
        // 로그인
        FirebaseManager.Auth.SignInWithEmailAndPasswordAsync(email, pass).ContinueWithOnMainThread(task =>
        {
            if ( task.IsCanceled )
            {
                lobbymanager.ShowInfo("로그인이 취소됨");
                SetInteractable(true);
                return;
            }
            else if ( task.IsFaulted )
            {
                lobbymanager.ShowInfo($"로그인 실패 : {task.Exception.Message}");
                SetInteractable(true);
                return;
            }

            /*PhotonNetwork.LocalPlayer.NickName = nickNameInput.text; // 이부분은 DB에 닉네임 저장하고 불러오기로 할 예정
            PhotonNetwork.ConnectUsingSettings();
            FirebaseManager.DB.GetReference($"UserDate/{email}/nickName").SetValueAsync(nickNameInput.text);*/
            NicknameAppyly();
            SetInteractable(true);
        });


    }

    private void NicknameAppyly()
    {
        SetInteractable(false);
        UserProfile userProfile = new UserProfile();
        userProfile.DisplayName = nickNameInput.text;

        FirebaseManager.Auth.CurrentUser.UpdateUserProfileAsync(userProfile).ContinueWithOnMainThread(task =>
        {
            if ( task.IsCanceled )
            {
                lobbymanager.ShowInfo("유저 프로필 갱신 취소");
                SetInteractable(true);
                return;
            }
            if ( task.IsFaulted )
            {
                lobbymanager.ShowInfo($"유저 프로필 갱신 실패: {task.Exception.Message}");
                SetInteractable(true);
                return;
            }
            PhotonNetwork.LocalPlayer.NickName =userProfile.DisplayName;
            PhotonNetwork.ConnectUsingSettings();
            lobbymanager.ShowInfo("유저 프로필 갱신 성공");
            SetInteractable(true);
        });
    }
    private void Cancel()
    {
        lobbymanager.SetActivePanel(LobbyManager.Panel.Login);
    }

    private void SetInteractable( bool interactable )
    {
        emailInput.interactable = interactable;
        passwordInput.interactable = interactable;
        confirmInputInput.interactable = interactable;
        cancel.interactable = interactable;
        singUp.interactable = interactable;
    }
}
