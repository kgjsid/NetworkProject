using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun, IDamageable
{
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;

    [SerializeField] int hp;

    [Header("Move")]
    private float moveSpeed = 8;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    private Vector3 moveDir;
    private float ySpeed;
    private bool isAlive = true;

    [SerializeField] LayerMask damageLayer;
    private bool isDamaged;

    private KillLogUI killLogUI;
    private void Awake()
    {
        killLogUI = FindObjectOfType<KillLogUI>();
    }
    private void Update()
    {
        if ( isAlive ) // Only allow movement if the player is alive
        {
            Move();
        }
        Jump();
    }

    private void Move()
    {
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
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;
        /*if (controller.isGrounded)
        {
            ySpeed = 0f;
        }
        */
        controller.Move(Vector3.up * ySpeed * Time.deltaTime);

    }

    private void OnMove( InputValue value )
    {
        Vector2 inputDir = value.Get<Vector2>();
        moveDir.x = inputDir.x;
        moveDir.z = inputDir.y;
    }

    private void OnRun( InputValue value )
    {
        if ( value.isPressed )
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

    private void OnAttack( InputValue value )
    {
        animator.SetBool("Attack02", true);
    }

    public void AniAttack()
    {
        animator.SetBool("Attack02", false);
    }

    private void Die()
    {
        isDamaged = false;
        animator.SetTrigger("Die");
        photonView.RPC("RequestKillLog", RpcTarget.MasterClient);
    }
    [PunRPC]
    public void RequestKillLog()
    {
        killLogUI.KillDieLog(PhotonNetwork.LocalPlayer.NickName);

    }
    public void TakeDamage( int damage )
    {
        if ( !isDamaged )
        {
            isDamaged = true;
            hp -= damage;
            if ( hp <= 0 )
            {
                Die();

            }
        }
    }

    private int damageCount;
    private void OnTriggerEnter( Collider collision )
    {
        if ( damageLayer.Contain(collision.gameObject.layer) )
        {
            TakeDamage(2);

        }
    }

    public void PlayerMove()
    {
        isAlive = true;
        hp += 2;         // 테스트용 hp        
    }
    public void PlayerNotMove()
    {
        isAlive = false;
    }
}
