using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Creature
{
    // vars
    public bool m_boss = false;

    // components
    public AudioClip m_sfxHit;
    protected AudioSource m_audioSource;
    protected GameObject m_target;

    // Use this for initialization
    protected override void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Hit(int value)
    {
        if(IsDead())
            return;

        if(!IsBoss()) {
            AddStun(500);
            m_animator.Play("Hit");
        }

        StartBlink();
        m_audioSource.PlayOneShot(m_sfxHit, 7.5f);
        AddHealth(-value);
    }

    protected bool IsBoss()
    {
        return m_boss;
    }

    protected void SetSpeed(float speed)
    {
        m_mspd = speed;
    }

    virtual protected void Walk() { }
}
