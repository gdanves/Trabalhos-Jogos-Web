using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Player
{
    protected override void Start ()
    {
        base.Start();
    }

    protected override void Update ()
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

        float speedMod = !IsOnGround() ? m_mspd * 1.5f : m_mspd;
        m_rb.velocity = m_movement * speedMod + m_pushMovement;
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == m_groundLayer || collision.gameObject.tag == "Enemy")
            m_grounded = true;
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.layer == m_groundLayer || collision.gameObject.tag == "Enemy")
            m_grounded = false;
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
        m_movement.y = 3.5f;
    }

    private void UpdateJump()
    {
        if(m_movement.y > 0)
            m_movement += Vector2.up * Physics2D.gravity * Time.deltaTime;
        else if(m_movement.y < 0 || !IsOnGround())
            m_movement += Vector2.up * Physics2D.gravity * Time.deltaTime;
    }

    private void UseBasicAttack()
    {
        Collider2D[] enemys = new Collider2D[99];
        GetCreaturesInOverlap(m_basicAttackCollider, enemys);
        foreach(Collider2D enemyCol in enemys) {
            if(enemyCol && !enemyCol.isTrigger && enemyCol.gameObject.CompareTag("Enemy"))
                enemyCol.GetComponent<Monster>().Hit(1);
        }
    }
}
