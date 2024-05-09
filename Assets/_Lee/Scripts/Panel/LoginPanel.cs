using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField NickNameField;
    [SerializeField] Button loginButton;
    private void Start()
    {
        NickNameField.text = $"Palyer {Random.Range(1000, 10000)}"; // 일단 랜덤으로 지정해줌(구현할때 귀찮아서)
        loginButton.onClick.AddListener(() => Login()); // 로그인버튼 누르면 로그인 되도록
    }

    // 로그인해서 네트워크한테 메인상태로 만들어줌
    private void Login()
    {
        /*        if ( NickNameField.text == "" )
                    NickNameField.text = string.Format("Player {0}", Random.Range(1000, 10000));
        */
        PhotonNetwork.LocalPlayer.NickName = NickNameField.text;
        PhotonNetwork.ConnectUsingSettings();
    }

}
