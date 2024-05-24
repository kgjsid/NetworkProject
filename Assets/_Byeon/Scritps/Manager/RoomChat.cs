using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomChat : MonoBehaviourPun
{
    [Header("chat")]
    [SerializeField] TMP_Text chatTextPrefab;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] RectTransform content;
    //채팅 삭제용
    Queue<TMP_Text> textQueue = new Queue<TMP_Text>();
    //채팅 색깔용
    [SerializeField] List<Color> colorList = new List<Color>();
    int number;

    private void Awake()
    {
        inputField.onSubmit.AddListener(Send);

    }

    private void Start()
    {
        inputField.text = "";
        inputField.ActivateInputField();
    }

    private void Send(string a)
    {
        if (inputField.text != "")
        {
            photonView.RPC("RoomSendRpc", RpcTarget.All, inputField.text);
            inputField.text = "";
            inputField.ActivateInputField(); //채팅창 재활성화
        }

        
    }

    [PunRPC]
    private void RoomSendRpc(string inputField, PhotonMessageInfo info) // PhotonMessageInfo info 보낸사람의 정보
    {
        //int playerNumber = info.photonView.OwnerActorNr;

        TMP_Text textPrefab = Instantiate(chatTextPrefab, content);

        textPrefab.text = $"{info.Sender.NickName} : {inputField}";

        if (textQueue.Count < 15)
        {
            textQueue.Enqueue(textPrefab);
        }
        else if (textQueue.Count >= 15)
        {
            //오래된 채팅 삭제
            TMP_Text destroyText = textQueue.Dequeue();
            Destroy(destroyText.gameObject);
            textQueue.Enqueue(textPrefab);
        }
    }
}