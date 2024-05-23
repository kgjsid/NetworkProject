using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameUI : MonoBehaviour
{
    private static BaseGameUI instance;
    public static BaseGameUI Instance { get { return instance; } }

    [SerializeField] protected GameObject GameUI;

    [SerializeField] protected KillLogPanel killLogUI;
    [SerializeField] protected GameTime gameTime;
    [SerializeField] protected PlayerList playerList;
    public PlayerList PlayerList { get {  return playerList; } }
    private void Awake()
    {
        
        if ( instance == null )
        {
            instance = this;
        }
        GameUI.SetActive(true);

    }
    private void Start()
    {
        killLogUI = GetComponentInChildren<KillLogPanel>();
        gameTime = GetComponentInChildren<GameTime>();
        playerList = GetComponentInChildren<PlayerList>();
    }
    private void EndGame()
    {
        GameUI.SetActive( false );
    }
}
