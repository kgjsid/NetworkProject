using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModPanel : MonoBehaviour
{
    [SerializeField] Button baseGameButton;
    [SerializeField] Button missionGameButon;
    [SerializeField] Button itemGameButton;

    [SerializeField] RoomPanel roomPanel;

    private void Start()
    {
        roomPanel = GetComponentInParent<RoomPanel>();
        baseGameButton.onClick.AddListener(BaseGame);
        missionGameButon.onClick.AddListener(MissionGame);
        itemGameButton.onClick.AddListener(ItemGame);
    }

    private void BaseGame()
    {
        roomPanel.curentGameModname = roomPanel.GameModName [0];
        gameObject.SetActive (false);
    }
    private void MissionGame()
    {
        roomPanel.curentGameModname = roomPanel.GameModName [1];
        gameObject.SetActive(false);
    }
    private void ItemGame()
    {
        roomPanel.curentGameModname = roomPanel.GameModName [2];
        gameObject.SetActive(false);
    }
}
