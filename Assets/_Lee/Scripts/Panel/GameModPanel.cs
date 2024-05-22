using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModPanel : MonoBehaviour
{
    [SerializeField] Button baseGameButton;
    [SerializeField] Button missionGameButon;
    [SerializeField] Button itemGameButton;

    [SerializeField]RoomPanel roomPanel;

    private void Start()
    {
        roomPanel = GetComponentInParent<RoomPanel>();
        baseGameButton.onClick.AddListener(BaseGame);
        missionGameButon.onClick.AddListener(MissionGame);
        itemGameButton.onClick.AddListener(ItemGame);
    }

    private void BaseGame()
    {
        LobbyManager.Instance.SetActivePanel(LobbyManager.Panel.Room);
    }
    private void MissionGame()
    {
        LobbyManager.Instance.SetActivePanel(LobbyManager.Panel.Room);
    }
    private void ItemGame()
    {
        LobbyManager.Instance.SetActivePanel(LobbyManager.Panel.Room);
    }
}
