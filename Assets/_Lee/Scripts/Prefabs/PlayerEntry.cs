using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text nickName;
    [SerializeField] Image readyIMG;
    [SerializeField] TMP_Text readyTxT;
    public TMP_Text ReadyTxT { get { return readyTxT; } set { readyTxT = value; } }
    [SerializeField] Image masterIcon;

    private Player player;
    public Player Player { get { return player; } }
    private void OnDisable()
    {
        Destroy(gameObject);
    }
    public void SetPlayer( Player player )
    {
        this.player = player;

        nickName.text = player.NickName;
        if ( player.IsMasterClient )
        {
            readyIMG.gameObject.SetActive(false);
            masterIcon.gameObject.SetActive(true);
        }
        else
        {
            readyIMG.gameObject.SetActive(true);
            masterIcon.gameObject.SetActive(false);
        }
    }


    /* public void ReadyAndStart()
     {
         Debug.Log(player.NickName);
         bool ready = player.GetReady();
         player.SetReady(!ready);
         if(!player.IsMasterClient)
         {
             readyTxT.text = "준비완료";
         }

         if(player.IsMasterClient )
         {
             // 마스터일때는 준비와 게임 시작을하게 해줄거임
             PhotonNetwork.CurrentRoom.IsVisible = false; // 방 닫기
             PhotonNetwork.LoadLevel("BaseGameScene");
         }
     }*/
    public void ChangeCustomProperty( PhotonHashtable property )
    {
        bool ready = player.GetReady();
        readyTxT.text = ready ? "준비 완료" : "준비 미완료";
    }
}
