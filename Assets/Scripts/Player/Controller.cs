using System.Collections;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float Accleration;
    [SerializeField] float Declaration;
    [SerializeField] float MaxSpeed;

    [Header("Jump")]
    [SerializeField] float Jumpforce;
    [SerializeField] LayerMask Ground;
    [SerializeField] Transform GroundCheck;

    [Header("Dash")]
    [SerializeField] Transform RaycastOrigin;
    [SerializeField] float DashDistance;
    [SerializeField] LayerMask Object;

    bool isGrounded;
    bool Jumped;
    bool LetMove;
    bool Attack1;
    bool AirAttack;
    
    Rigidbody2D rb;
    Animator animator;



    void Start()
    {
        isGrounded = false;
        LetMove = true;
        Attack1 = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        Animation();
    }

    void FixedUpdate()
    {
        CheckGrounded();
        if (LetMove)
        {
            Movement();
        }
        
    }

    void HandleInput()
    {
        Vector2 Velocity = rb.velocity;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0, Jumpforce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            Jumped = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && Jumped && !isGrounded)
        {
            rb.velocity = new Vector2(Velocity.x, Velocity.y - (Velocity.y / 1.5f));
            Jumped = false;
        }


        float Direction = Input.GetAxisRaw("Horizontal");
        Vector3 scale = transform.localScale;
        if (Direction != 0)
        {
            transform.localScale = new Vector3(Direction, scale.y, scale.z);
        }
    }

    void CheckGrounded()
    {
        Collider2D circle = Physics2D.OverlapCircle(GroundCheck.position, 0.15f, Ground);
        isGrounded = (circle != null);
    }

    void Movement()
    {
        float Direction = Input.GetAxisRaw("Horizontal");
        Vector2 Velocity = rb.velocity;
        float TargetSpeed = (MaxSpeed * Direction);
        float Diff = TargetSpeed - Velocity.x;
        float acc = (TargetSpeed != 0) ? Accleration : Declaration;

        rb.AddForce(acc * Diff * Vector2.right);
    }

    public void Dash(float num = -1)
    {
        num = (num != -1) ? num : DashDistance;
        Vector3 Transform = transform.position;
        RaycastHit2D RayCast = Physics2D.Raycast(RaycastOrigin.position, Vector2.right * transform.localScale.x, num, Object);

        if (RayCast)
        {
            float Distance = RayCast.distance;
            Distance = (Distance >= 0.25) ? Distance :  0;

            transform.position = new Vector3(Transform.x + Distance * transform.localScale.x, Transform.y, 0);
            return;
        }

        transform.position = new Vector3( Transform.x+num * transform.localScale.x, Transform.y, 0);
        return;
    }

    public void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    void Animation()
    {
        Vector2 Velocity = rb.velocity;
        if (Velocity.y < 0 && !isGrounded)
        {
            animator.SetBool("Falling", true);
        }
        else
        {
            animator.SetBool("Falling", false);
        }

        float Direction = Input.GetAxisRaw("Horizontal");
        if (Direction != 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && isGrounded && !Attack1) 
        {
            animator.SetTrigger("Attack1");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isGrounded && !Attack1 && AirAttack)
        {
            AirAttack = false;
            animator.SetTrigger("Attack1-Air");
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && isGrounded)
        {
            animator.SetTrigger("Attack2");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Dash");   
        }

        if (isGrounded)
        {
            AirAttack = true;
        }

    }

    public void Attack(float t) 
    {
        StartCoroutine(Attacking(t));
    }

    IEnumerator Attacking(float time)
    {
        LetMove = false;
        Attack1 = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        yield return new WaitForSeconds(time);
        LetMove = true;
        rb.isKinematic = false;
        Attack1 = false;
    }
}