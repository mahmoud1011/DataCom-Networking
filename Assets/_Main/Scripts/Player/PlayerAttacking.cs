using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : NetworkBehaviour
{
    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    public GameObject hitEffect;
    //public AudioClip swordSwing;
    //public AudioClip hitSound;

    //[SerializeField] private AudioSource audioSource;
    [SerializeField] private Camera cam;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;
    Animator animator;

    public override void OnStartAuthority()
    {
        enabled = true;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (!readyToAttack || attacking)
            return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        //audioSource.pitch = Random.Range(0.9f, 1.1f);
        //audioSource.PlayOneShot(swordSwing);

        if (attackCount == 0)
        {
            // ChangeAnimationState(ATTACK1); // Implement your animation change here
            animator.SetTrigger("Attack"); 
            attackCount++;
        }
        else
        {
            // ChangeAnimationState(ATTACK2); // Implement your animation change here
            attackCount = 0;
        }
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        if(cam == null)
        {
            cam = GetComponentInChildren<Camera>();
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            HitTarget(hit.point);

            if (hit.transform.TryGetComponent(out MyPlayerHealth target))
            {
                if (isServer)
                {
                    target.TakeDamage(attackDamage);
                }
                else
                {
                    CmdDealDamage(target.gameObject, attackDamage);
                }
            }
        }
    }

    [Command]
    void CmdDealDamage(GameObject target, int damage)
    {
        target.GetComponent<MyPlayerHealth>().TakeDamage(damage);
    }

    void HitTarget(Vector3 pos)
    {
        //audioSource.pitch = 1;
        //audioSource.PlayOneShot(hitSound);

        GameObject hitEffectGO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(hitEffectGO, 20);
    }
}
