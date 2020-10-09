using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Player
{
    override protected void Start ()
    {
        base.Start();
 
        // set stats
        SetMaxHealth(100);
    }

    override protected void Update ()
    {
        if(IsDead()) {
            base.Update();
            return;
        }

        if(Input.GetButton("Fire1") && CanAttack())
            Attack();

        base.Update();

        float speedMod = m_animator.GetCurrentAnimatorStateInfo(0).IsName("Roll") ? 6f : 3f;
        m_rigidbody.velocity = m_movement * speedMod + m_pushMovement;
    }

    private void Attack()
    {
        AddAttackExhaust(800);
        AddRoot(300);
        m_animator.Play("Attack_sword");
        Invoke("UseBasicAttack", 0.1f);
    }

    private void UseBasicAttack()
    {
        Collider2D[] enemys = new Collider2D[99];
        int count = GetCreaturesInOverlap(m_basicAttackCollider, enemys);
        for (int i = 0; i <= count -1; i++) {
            Collider2D enemyCol = enemys[i];
            // TODO
        }
    }
}
