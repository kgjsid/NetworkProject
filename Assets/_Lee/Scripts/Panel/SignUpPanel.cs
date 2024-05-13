using Firebase.Extensions;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanel : MonoBehaviour
{
    [SerializeField] LobbyManager manager;

    [SerializeField] TMP_InputField nickNameInput;
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_InputField confirmInputInput;

    [SerializeField] Button cancel;
    [SerializeField] Button singUp;

    private void Awake()
    {
        manager = FindObjectOfType<LobbyManager>();
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
            manager.ShowInfo("비밀번호가 같지 않음");
            SetInteractable(true);
            return;
        }
        FirebaseManager.Auth.CreateUserWithEmailAndPasswordAsync(email, pass).ContinueWithOnMainThread(task =>
        {
            if ( task.IsCanceled )
            {
                manager.ShowInfo("회원가입 취소됨");
                SetInteractable(true);
                return;
            }
            else if ( task.IsFaulted )
            {
                manager.ShowInfo($"회원가입 실패함 : {task.Exception.Message}");
                SetInteractable(true);
                return;
            }
            manager.ShowInfo("회원가입 성공");
            manager.SetActivePanel(LobbyManager.Panel.Login);
            SetInteractable(true);
        });
        // 나중에 구조체로 가져와야 할듯? ㅈㄴ 대충 만듬
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

            PhotonNetwork.LocalPlayer.NickName = nickNameInput.text; // 이부분은 DB에 닉네임 저장하고 불러오기로 할 예정
            PhotonNetwork.ConnectUsingSettings();
            FirebaseManager.DB.GetReference($"UserDate/{email}/nickName").SetValueAsync(nickNameInput.text);
            SetInteractable(true);
        });
    }
    private void Cancel()
    {
        manager.SetActivePanel(LobbyManager.Panel.Login);
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
