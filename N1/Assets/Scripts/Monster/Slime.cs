using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    public Key m_keyPrefab;

    // Start is called before the first frame update
    protected override void Start ()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update ()
    {
        if(IsDead())
            return;

        if(CanAttack() && Attack())
            StopWalk();
        else if(CanWalk())
            Walk();

        base.Update();
    }

    protected override void Die()
    {
        m_animator.SetBool("Dead", true);
        foreach(Collider2D collider in GetComponents<Collider2D>())
            collider.enabled = false;
        Invoke("InternalDestroy", 1);
    }

    protected override void Walk()
    {
        bool canWalk = false;
        Collider2D[] colliders = new Collider2D[99];
        m_edgeCollider.OverlapCollider(new ContactFilter2D(), colliders);
        foreach(Collider2D col in colliders) {
            if(col && col.gameObject.layer == m_groundLayer) {
                canWalk = true;
                break;
            }
        }

        if(!canWalk)
            FlipCreature(!m_spriteRenderer.flipX);

        int offX = m_spriteRenderer.flipX ? -1 : 1;
        Vector3 targetPosition = new Vector3(transform.position.x + offX, transform.position.y, transform.position.z);
        Vector3 oldPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, m_mspd * Time.deltaTime);
        OnWalk(transform.position.x - oldPosition.x);
    }

    private bool Attack()
    {
        bool shouldAttack = false;
        Collider2D[] players = new Collider2D[99];
        GetCreaturesInOverlap(m_basicAttackCollider, players);
        foreach(Collider2D playerCol in players) {
            if(playerCol && !playerCol.isTrigger && playerCol.gameObject.CompareTag("Player")) {
                shouldAttack = true;
                break;
            }
        }

        if(shouldAttack) {
            AddAttackExhaust(600);
            AddRoot(600);
            m_animator.Play("Attack_basic");
            Invoke("UseBasicAttack", 0.4f);
            return true;
        }
        return false;
    }

    private void UseBasicAttack()
    {
    	if(IsDead() || !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_basic") )
            return;

        Collider2D[] players = new Collider2D[99];
        GetCreaturesInOverlap(m_basicAttackCollider, players);
        foreach(Collider2D playerCol in players) {
            if(playerCol && !playerCol.isTrigger && playerCol.gameObject.CompareTag("Player")) {
                playerCol.GetComponent<Player>().Hit((int)m_atk);
            }
        }
    }

    private void InternalDestroy()
    {
        if(m_keyPrefab)
            Instantiate(m_keyPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }
}
