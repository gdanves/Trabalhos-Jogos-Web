using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Player
{
    const float fallMultiplier = 1.5f;

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
        if(Input.GetButton("Jump") && CanJump())
            Jump();
        else if(m_movement.y < 0 && IsOnGround())
            m_movement.y = 0;
        else
            UpdateJump();

        base.Update();

        float speedMod = m_animator.GetCurrentAnimatorStateInfo(0).IsName("Roll") ? 6f : 3f;
        m_rb.velocity = m_movement * speedMod + m_pushMovement;
    }

    private void Attack()
    {
        AddAttackExhaust(400);
        if(IsOnGround())
            AddRoot(300);
        m_animator.Play("Attack_sword");
        // delay to make it more realistic, based on the animation
        Invoke("UseBasicAttack", 0.1f);
    }

    private void Jump()
    {
        m_movement.y = 5f;
        //m_animator.Play("Attack_sword");
    }

    private void UpdateJump()
    {
        if(m_movement.y < 0)
            m_movement += Vector2.up * Physics2D.gravity * fallMultiplier * Time.deltaTime;
        else if(m_movement.y > 0)
            m_movement += Vector2.up * Physics2D.gravity * Time.deltaTime;
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
