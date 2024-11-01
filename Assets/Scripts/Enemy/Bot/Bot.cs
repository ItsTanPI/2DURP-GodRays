using UnityEngine;

public class Bot : MonoBehaviour
{
    public bool isDead;


    Animator animator;
    Rigidbody2D rb;


    //Player Deduction
    [SerializeField] Transform Orgin;
    [SerializeField] LayerMask Player;
    [SerializeField] Vector2 Size;
    [SerializeField] CapsuleDirection2D direction2D;

    float CoolDown = 2.0f;
    bool inRange;
    bool inSight;



    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        inRange = false;
        inSight = false;
        isDead = false;
    }


    bool deduction()
    {
        
        Collider2D collider = Physics2D.OverlapCapsule(Orgin.position, Size, direction2D, 0f, Player);
        if (collider != null) 
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        if (isDead && animator!= null)
        {
            return;
        }

        state();
    }


    void state()
    {
        if (deduction() && !inRange)
        {
            animator.SetTrigger("Wake");
            inRange = true;
            inSight = true;
            CoolDown = 2.0f;
        }
        if (!deduction() && inRange)
        {
            inSight = false;
            if (CoolDown >= 0)
            {
                CoolDown -= Time.deltaTime;
            }
            else
            {
                animator.SetTrigger("Sleep");
                inRange = false;
                CoolDown = 2.0f;
            }
        }
    }
}
