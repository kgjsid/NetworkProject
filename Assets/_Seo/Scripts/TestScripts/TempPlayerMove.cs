using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Cinemachine;

public class TempPlayerMove : MonoBehaviourPun
{
    // 네트워크 테스트 플레이어 코드

    Vector3 moveDir;
    [SerializeField] PlayerInput input;
    [SerializeField] CinemachineFreeLook virtualCamera;
    [SerializeField] CharacterController controller;

    private void Start()
    {
        if(photonView.IsMine == false)
        {
            input.enabled = false;
            controller.enabled = false;
            virtualCamera.gameObject.SetActive(false);
        }
    }

    /*
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //transform.Translate(moveDir * Time.fixedDeltaTime * 5f);
        if (controller == null)
            return;

        controller.Move(moveDir * Time.fixedDeltaTime * 5f);
    }

    private void OnMove(InputValue value)
    {
        Vector2 inputDir = value.Get<Vector2>();

        moveDir.x = inputDir.x;
        moveDir.z = inputDir.y;
    }
    */
}
