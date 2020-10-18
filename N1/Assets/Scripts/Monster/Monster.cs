using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Creature
{
    // vars
    public bool m_boss = false;

    protected GameObject m_target;

    // Use this for initialization
    protected override void Start()
    {
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
