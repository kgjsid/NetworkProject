using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerEmote : MonoBehaviour
{
    [SerializeField] Animator ani;
    [SerializeField] InputAction action;
    [SerializeField] GameObject emoteUI;
    public bool emoteUiCheck;
    public PlayerAttack playerAttack;
    private void Update()
    {
        EmoteCancel();
    }
    private void OnEmote()
    {
        playerAttack.canAttack = false; // PlayerAttack의 canAttack을 false로 설정
        emoteUiCheck = true;
        emoteUI.SetActive(true);
    }

    public void Cancel()
    {
        playerAttack.canAttack = true;
        ani.SetBool("EmoteMode", false);
        emoteUI.SetActive(false);
    }

    public void EmoteCancel()
    {
        if (Input.anyKeyDown)
        {
            ani.SetBool("EmoteMode", false);
            ani.SetBool("Emote01", false);
            ani.SetBool("Emote02", false);
            ani.SetBool("Emote03", false);
            ani.SetBool("Emote04", false);
        }
    }

    public void Emote01()
    {
        ani.SetBool("Emote01", true);
        ani.SetBool("EmoteMode", false);
        emoteUI.SetActive(false);
        playerAttack.canAttack = true;
    }
    public void Emote02()
    {
        ani.SetBool("Emote02", true);
        ani.SetBool("EmoteMode", false);
        emoteUI.SetActive(false);
        playerAttack.canAttack = true;
    }
    public void Emote03()
    {
        ani.SetBool("Emote03", true);
        ani.SetBool("EmoteMode", false);
        emoteUI.SetActive(false);
        playerAttack.canAttack = true;
    }
    public void Emote04()
    {
        ani.SetBool("Emote04", true);
        ani.SetBool("EmoteMode", false);
        emoteUI.SetActive(false);
        playerAttack.canAttack = true;
    }
}