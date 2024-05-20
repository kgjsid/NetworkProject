using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerController : MonoBehaviourPun, IDamageable
{   // 플레이어 컨트롤러 스크립트
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;

    [SerializeField] int hp;

    public int Hp { get { return hp; } }

    [Header("Move")]
    private float moveSpeed = 8;            // 움직일 때의 속도
    [SerializeField] float walkSpeed;       // 걷는 속도(8)
    [SerializeField] float runSpeed;        // 뛰는 속도(12)
    private Vector3 moveDir;                // 움직임 벡터
    private float ySpeed;                   // 중력 및 점프력(수정 필요)
    private bool isAlive = true;            // 살아있는지 여부

    [SerializeField] LayerMask damageLayer; // 데미지 레이어
    private bool isDamaged;                 // 데미지를 받고 있는지에 대한 여부

    [SerializeField] PlayerInput input;
    [SerializeField] CinemachineFreeLook virtualCamera;
    [SerializeField] Canvas playerUI;

    KillLogUI killLogUI;
    [SerializeField] Damage damageCheck;

    private void Awake()
    {
        killLogUI = FindObjectOfType<KillLogUI>();
    }
    [PunRPC]
    public void KillLogNickName( int targetID )
    {
        // 모든 클라이언트에서 실행됨
        PhotonView targetView = PhotonView.Find(targetID);
        if ( targetView != null )
        {
            string targetName = targetView.Controller.NickName;
            if ( targetView.gameObject.layer == 3 )
            {
                killLogUI.KillLog(photonView.Controller.NickName, targetName);

            }
            else
            {
                killLogUI.KillLog(photonView.Controller.NickName, "AI");

            }
        }
    }
    public void KillLog( int targetID )
    {
        photonView.RPC("KillLogNickName", RpcTarget.AllViaServer, targetID);
    }
    private void Start()
    {   // 시작시 네트워크 작업
        if ( photonView.IsMine == false )
        {   // 로컬에서만 움직일 수 있도록 설정 수정
            input.enabled = false;
            playerUI.enabled = false;
            Destroy(controller);
            virtualCamera.gameObject.SetActive(false);
        }
    }



    private void FixedUpdate()
    {
        if ( controller == null )
            return;

        if ( isAlive ) // Only allow movement if the player is alive
        {
            Move();
        }
        Jump();
    }

    private void Move()
    {   // 실제 움직임 스크립트
        Vector3 forwardDir = Camera.main.transform.forward;
        forwardDir = new Vector3(forwardDir.x, 0, forwardDir.z).normalized;

        Vector3 rightDir = Camera.main.transform.right;
        rightDir = new Vector3(rightDir.x, 0, rightDir.z).normalized;

        controller.Move(forwardDir * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(rightDir * moveDir.x * moveSpeed * Time.deltaTime);

        Vector3 lookDir = forwardDir * moveDir.z + rightDir * moveDir.x;
        if ( lookDir.sqrMagnitude > 0 )
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 100);
        }

        animator.SetFloat("MoveSpeed", moveDir.magnitude * moveSpeed, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {   // 중력 및 점프 스크립트
        ySpeed += Physics.gravity.y * Time.deltaTime;
        /*if (controller.isGrounded)
        {
            ySpeed = 0f;
        }
        */
        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void OnMove( InputValue value )
    {   // 움직임 입력 함수(WASD)
        Vector2 inputDir = value.Get<Vector2>();
        moveDir.x = inputDir.x;
        moveDir.z = inputDir.y;
    }

    private void OnRun( InputValue value )
    {   // 달리는 것 설정함수(Shift)
        if ( value.isPressed )
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

    private void Die()
    {   // 사망 함수
        isDamaged = false;          // 데미지는 받지 않는 상태로
        animator.SetTrigger("Die"); // Die 애니메이션 재생
        // 사망시 플레이어 상태 변경
        // TODO... Die시 추가 작업 진행
        // 1. 공격 스크립트 제외
        // 2. 투명 처리
        // 3. 어택을 받을 수 없도록 처리 필요
    }

    public void TakeDamage( int damage )
    {   // 실제 데미지 함수
        if ( !isDamaged )         // 데미지를 받는 상태가 아니라면(isDamage가 false)
        {
            isDamaged = true;   // 데미지를 받고 있고
            hp -= damage;       // 체력 감소
            if ( hp <= 0 )        // hp가 0보다 작으면
            {
                Die();          // 사망처리
            }
        }
    }


    public void PlayerMove()
    {   // 애니메이션 이벤트에서 활용 중
        isAlive = true;
        hp += 2;         // 테스트용 hp        
    }
    public void PlayerNotMove()
    {   // 애니메이션 이벤트에서 활용 중
        isAlive = false;
    }
}
