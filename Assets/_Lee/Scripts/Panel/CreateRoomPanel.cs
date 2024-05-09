using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : MonoBehaviour
{
    [SerializeField] Button cancel;
    [SerializeField] TMP_InputField creatRoomName;
    [SerializeField] TMP_Dropdown roomNumber;
    [SerializeField] TMP_InputField password; // 비밀번호는 나중에
    [SerializeField] Button creatRoomButton;

    private void Start()
    {
        cancel.onClick.AddListener(() => Cancel());
        creatRoomButton.onClick.AddListener(() => CreateRoomConfirm());
    }

    // 방만들기
    private void CreateRoomConfirm()
    {
        string roomName = creatRoomName.text; // 방이름 가져오기
        if ( roomName == "" )
        {
            Debug.Log("이름 만들어줘!");
            return;
        }
        int maxPlayer =int.Parse(roomNumber.captionText.text); // 인원수 제한
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;
        PhotonNetwork.CreateRoom(roomName, options); //설정한 이름과 옵션으로 생성
    }

    private void Cancel()
    {
        gameObject.SetActive(false);
    }
}
