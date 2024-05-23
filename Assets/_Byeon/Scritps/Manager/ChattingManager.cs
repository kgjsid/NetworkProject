using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    //채팅창 자동으로 꺼지는 코르틴
    Coroutine chatCoroutine;
    float startTime;
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

        //인풋플레이어 찾기
        StartCoroutine(GetInputPlayer());
    }


    private void Update()
    {
        // 채팅치다 그냥 아무곳이나 클릭했을때
        if (!inputField.isFocused && playerInput != null)
            playerInput.enabled = true;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //창이 닫혀있을때
            if (!window.active)
            {
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
            else if (window.active && inputField.interactable)
            {
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
        if (inputField.text != "")
        {
            //보낼 채팅이있을때
            photonView.RPC("SendRpc", RpcTarget.All, inputField.text);  //d
        }
    }

    [PunRPC]
    private void SendRpc(string inputField, PhotonMessageInfo info) // PhotonMessageInfo info 보낸사람의 정보
    {
        //채팅 생성 및 내용 입력
        TMP_Text textPrefab = Instantiate(chatTextPrefab, content);
        textPrefab.text = $"{info.Sender.NickName} : {inputField}";

        //상대가 보냈을때 창 활성화
        window.SetActive(true);
        chatCoroutine = StartCoroutine(ChatActiveTime());

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



    IEnumerator ChatActiveTime()
    {
        Debug.Log(1);
        //채팅 켜기
        startTime = Time.time;

        while (Time.time - startTime <= 5)
        {
            Debug.Log(Time.time - startTime);
            yield return new WaitForSeconds(0.1f);
            if (inputField.text != "" && inputField.isFocused) //채팅치는 중이고 인풋필드가 활성화 되어있을때{
            {
                startTime += 0.1f;
            }
        }
        playerInput.enabled = true;
        inputField.DeactivateInputField();
        inputField.text = "";
        inputField.interactable = false;
        window.SetActive(false);
    }

    //플레이어들 생성후 찾기
    IEnumerator GetInputPlayer()
    {
        yield return new WaitForSeconds(0.5f);

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
    }
}
