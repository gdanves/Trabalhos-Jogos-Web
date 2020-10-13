using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    // stats
    public Vector2 m_health;
    public float m_atk;
    public float m_mspd;

    public Collider2D m_basicAttackCollider;
    public Transform m_feetTransform;
    public LayerMask groundLayer;
    protected Vector2 m_movement;

    protected Animator m_animator;
    protected SpriteRenderer m_spriteRenderer;
    //protected GameManager m_gameManager;

    protected float m_attackEnd = 0, m_rootEnd = 0, m_stunEnd = 0;
    protected Vector2 m_pushMovement = new Vector2(0, 0);

    private AnimationClip[] m_animationClips;
    private float m_blinkStartTime;
    private float m_speedModifier = 1;
    private const float m_blinkDuration = 250;

    virtual protected void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animationClips = m_animator.runtimeAnimatorController.animationClips;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        //m_gameManager = FindObjectOfType<GameManager>();
    }

    virtual protected void Update() { }
    virtual protected void FixedUpdate() { }

    public float GetAnimationTime(string name)
    {
        foreach(AnimationClip clip in m_animationClips) {
            if (clip.name == name)
                return clip.length;
        }
        return 0;
    }

    public IEnumerator ChangeSpeed(float modifier, float duration)
    {
        m_speedModifier += modifier;
        yield return new WaitForSeconds(duration);
        m_speedModifier -= modifier;
    }

    virtual protected void Die() { }

    virtual protected void StopWalk()
    {
        m_animator.SetBool("Walking", false);
        m_movement *= 0f;
    }

    protected void AddHealth(int value)
    {
        if(IsDead())
            return;

        m_health = new Vector2(Mathf.Clamp(m_health.x + value, 0, m_health.y), m_health.y);
        if(m_health.x == 0)
            Die();
    }

    protected void AddAttackExhaust(float duration)
    {
        m_attackEnd = Mathf.Max(m_attackEnd, Mathf.Round(Time.time*1000) + duration);
    }

    protected void AddRoot(float duration)
    {
        m_rootEnd = Mathf.Max(m_rootEnd, Mathf.Round(Time.time*1000) + duration);
    }

    protected void AddStun(float duration)
    {
        m_stunEnd = Mathf.Max(m_stunEnd, Mathf.Round(Time.time*1000) + duration);
    }

    public void SetMaxHealth(int maxHealth, bool updateHealth = true)
    {
        m_health = new Vector2(updateHealth ? maxHealth : m_health.x, maxHealth);
    }

    protected int GetCreaturesInOverlap(Collider2D attackCollider, Collider2D[] creatures)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        return attackCollider.OverlapCollider(contactFilter, creatures);
    }

    protected Vector2 GetHealth()
    {
        return m_health;
    }

    protected float GetSpeed(bool noModifiers = false)
    {
        return noModifiers ? m_mspd : m_mspd * m_speedModifier;
    }

    protected float GetSpeedFactor(bool noModifiers = false)
    {
        return GetSpeed(noModifiers) / 100 * 6;
    }

    protected bool IsDead()
    {
        return m_health.x == 0;
    }

    virtual protected bool CanAttack()
    {
        return Util.GetTimeMillis() > m_attackEnd && !IsStunned();
    }

    virtual protected bool CanJump()
    {
        return IsOnGround() && !IsStunned() && !IsRooted();
    }

    virtual protected bool CanWalk()
    {
        return GetSpeed() > 0  && !IsStunned() && !IsRooted();
    }

    virtual protected bool IsRooted()
    {
        return Util.GetTimeMillis() <= m_rootEnd;
    }

    virtual protected bool IsStunned()
    {
        return Util.GetTimeMillis() <= m_stunEnd;
    }

    public bool IsOnGround()
    {
        Collider2D colliders = Physics2D.OverlapCircle(m_feetTransform.position, 1.01f, groundLayer);
        return colliders != null;
    }

    virtual protected void OnWalk(float h) 
    {
        if (h != 0.0f)
            FlipCreature(h < 0);

        float v =  m_movement.y;
        m_animator.SetBool("Walking", (h != 0 || v != 0));
        m_movement = new Vector2(h, v);
    }

    // sprite related
    protected void FlipCreature(bool flip)
    {
        m_spriteRenderer.flipX = flip;

        // we should flip the attack collider too
        if((flip && m_basicAttackCollider.offset.x > 0) || (!flip && m_basicAttackCollider.offset.x < 0))
            m_basicAttackCollider.offset = new Vector2(m_basicAttackCollider.offset.x * -1, m_basicAttackCollider.offset.y);
    }

    protected void StartBlink()
    {
        if(m_blinkStartTime + m_blinkDuration > Mathf.Round(Time.time*1000))
            return;

        m_blinkStartTime = Mathf.Round(Time.time*1000);
        StartCoroutine("Blink");
    }

    private IEnumerator Blink()
    {
        float flashAmount = 0f;
        float timeElapsed = 1;
        while(timeElapsed / m_blinkDuration < 1.0) {
            timeElapsed = Mathf.Max(1f, Mathf.Round(Time.time*1000) - m_blinkStartTime);
            flashAmount = timeElapsed / m_blinkDuration;
            if(flashAmount > 0.5)
                flashAmount = Mathf.Max(0, 1f - flashAmount);

            m_spriteRenderer.material.SetFloat("_FlashAmount", flashAmount);
            yield return new WaitForSeconds(.05f);
        }

        m_spriteRenderer.material.SetFloat("_FlashAmount", 0f);
    }
}
