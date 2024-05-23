using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
//using static System.Net.Mime.MediaTypeNames;

public class ChattingManager : MonoBehaviourPun
{
    [Header("chat")]
    [SerializeField] GameObject window;
    [SerializeField] TMP_Text chatTextPrefab;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] RectTransform content;

    [Header("playerInput")]
    //자기자신의 인풋액션 찾기 (채팅중 움직임 정지용)
    [SerializeField] PlayerInput playerInput;


    //채팅 삭제용
    Queue<TMP_Text> textQueue = new Queue<TMP_Text>();
    //채팅 색깔용
    [SerializeField] List<Color> colorList = new List<Color>();

    private void Awake()
    {
        inputField.onSubmit.AddListener(Send);
    }

    private void Start()
    {
        window.SetActive(false);  // d
    }


    private void Update()
    {
        if (!photonView.IsMine)
            return;

        // 채팅치다 그냥 아무곳이나 클릭했을때
        if (!inputField.isFocused && playerInput != null)
            playerInput.enabled = true;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //채팅창 처음열때 플레이어인풋 찾기
            if (playerInput == null)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PhotonView>().IsMine)
                    {
                        playerInput = player.GetComponent<PlayerInput>();
                        break;
                    }
                }
            }

            //창이 닫혀있을때
            if (!window.active)
            {
                Debug.Log(1);
                window.SetActive(true);
                inputField.interactable = true;
                inputField.text = "";
                inputField.ActivateInputField();
                playerInput.enabled = false;

                
                //채팅창 꺼지는 코르틴 ( 조건 : 채팅을 안치고있거나, 인풋필드 포커스가 꺼져있을때)
                if (chatCoroutine != null)
                {
                    StopCoroutine(chatCoroutine);
                    chatCoroutine = null;
                }
                
                chatCoroutine = StartCoroutine(ChatActiveTime());
            }

            //창이 열려있을때 다시 채팅치기
            else if (window.active && !inputField.interactable)
            {
                Debug.Log(2);
                inputField.interactable = true;
                inputField.text = "";
                inputField.ActivateInputField();
                playerInput.enabled = false;

                if (chatCoroutine != null)
                {
                    StopCoroutine(chatCoroutine);
                    chatCoroutine = null;
                }

                chatCoroutine = StartCoroutine(ChatActiveTime());
            }

            //창이 열려있을때 채팅을 입력할때
            else if(window.active && inputField.interactable)
            {
                Debug.Log(5);
                playerInput.enabled = true;
                inputField.DeactivateInputField();
                inputField.text = "";
                inputField.interactable = false;
            }
        }
    }


    //채팅 네트워크 공유
    private void Send(string a)
    {
        if(inputField.text == "")
            Debug.Log(3);

        if (inputField.text != "")
        {
            Debug.Log(4);
            //보낼 채팅이있을때
            photonView.RPC("SendRpc", RpcTarget.All, inputField.text);  //d
        }
        
    }

    [PunRPC]
    private void SendRpc(string inputField, PhotonMessageInfo info) // PhotonMessageInfo info 보낸사람의 정보
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


    Coroutine chatCoroutine;
    IEnumerator ChatActiveTime()
    {
        //채팅 켜기
        float start = Time.time;
        
        while (Time.time - start <= 5)
        {
            Debug.Log(6);
            yield return new WaitForSeconds(0.1f);
            if (inputField.text != "" && inputField.isFocused) //채팅치는 중이고 인풋필드가 활성화 되어있을때{
            {
                start += 0.1f;
            }
        }
        Debug.Log(7);
        playerInput.enabled = true;
        inputField.DeactivateInputField();
        inputField.text = "";
        inputField.interactable = false;
        window.SetActive(false);
    }

    /*private void AddScroll()
    {
        textSize = 0;
        
        foreach (TMP_Text text in textQueue)
        {
            textSize += text.preferredHeight;
            Debug.Log(text.preferredHeight);
        }
        
        Debug.Log($"textHeight : {textSize}");

        if(textSize > scrollRect.content.sizeDelta.y)
        {
            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, textSize);
        }

        scrollRect.verticalNormalizedPosition = 0f;

    }*/


}
