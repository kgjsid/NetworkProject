using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] TMP_Text infoTxT;
    [SerializeField] Button closeButton;
    [SerializeField] LobbyManager manager;

    private void Awake()
    {
        closeButton.onClick.AddListener(Close);
    }
    public void ShowInfo(string message )
    {
        manager.SetActivePanel(LobbyManager.Panel.Info);
        infoTxT.text = message;
    }
    private void Close()
    {
        manager.SetActivePanel(LobbyManager.Panel.Login);
    }
}
