using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField nicknameInput;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField passInput;
    [SerializeField] TMP_InputField confirmInput;

    [SerializeField] Button nameApply;
    [SerializeField] Button passApply;
    [SerializeField] Button edit;
    [SerializeField] Button delete;

    [SerializeField] LobbyManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<LobbyManager>();
        delete.onClick.AddListener(() => Delete());
        edit.onClick.AddListener(() => Edit());
        nameApply.onClick.AddListener(() => NameApply());
        passApply.onClick.AddListener(() => PassApply());
    }

    private void Delete()
    {
        manager.SetActivePanel(LobbyManager.Panel.Login);
    }

    private void Edit()
    {
        SetInteractable(false);
        FirebaseManager.Auth.CurrentUser.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if ( task.IsCanceled )
            {
                manager.ShowInfo("회원수정이 취소됨");
                SetInteractable(true);
                return;
            }
            else if(task.IsFaulted )
            {
                manager.ShowInfo($"회원수정에 실패함 : {task.Exception.Message}");
                SetInteractable(true ); 
                return;
            }

            manager.ShowInfo("회원수정 성공");
            SetInteractable(true);
        });
    }
    private void NameApply()
    {
        SetInteractable(false);

        UserProfile userProfile = new UserProfile();
        userProfile.DisplayName = nameInput.text;

        FirebaseManager.Auth.CurrentUser.UpdateUserProfileAsync(userProfile).ContinueWithOnMainThread(task =>
        {
            if ( task.IsCanceled )
            {
                manager.ShowInfo("이메일 입력이 취소됨 ");
                SetInteractable(true);
                return;
            }
            else if ( task.IsFaulted )
            {
                manager.ShowInfo($"이메일 입력에 실패함 : {task.Exception.Message}");
                SetInteractable(true);
                return;
            }

            manager.ShowInfo("이메일 입력 성공");
            SetInteractable(true);
        });

    }
    private void PassApply()
    {
        SetInteractable(false);
        string pass = confirmInput.text;
        FirebaseManager.Auth.CurrentUser.UpdatePasswordAsync(pass).ContinueWithOnMainThread(task =>
        {
            if ( task.IsCanceled )
            {
                manager.ShowInfo("비밀번호 수정이 취소됨 ");
                SetInteractable(true);
                return;
            }
            else if ( task.IsFaulted )
            {
                manager.ShowInfo($"비밀번호 수정 실패 : {task.Exception.Message}");
                SetInteractable(true);
                return;
            }

            manager.ShowInfo("비밀번호 수정 성공");
            SetInteractable(true);
        });
    }

    private void SetInteractable( bool interactable )
    {
        nameInput.interactable = interactable;
        passInput.interactable = interactable;
        confirmInput.interactable = interactable;
        nameApply.interactable = interactable;
        passApply.interactable = interactable;
        edit.interactable = interactable;
        delete.interactable = interactable;
    }
}
