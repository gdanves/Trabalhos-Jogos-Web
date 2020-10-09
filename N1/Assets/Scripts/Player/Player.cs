using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    // physics
    protected Rigidbody2D m_rigidbody;

    // components
    private Transform m_camera;

    override protected void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_camera = Camera.main.transform;
        base.Start();
    }

    override protected void Update()
    {
        //if(Input.GetButtonUp("Cancel"))
            //m_gameManager.PauseGame();

        if(IsDead()) {
            m_rigidbody.velocity *= 0f;
            return;
        }

        if(CanWalk())
            Walk();
        else
            StopWalk();
 
        base.Update();

        m_rigidbody.velocity = (m_movement * GetSpeedFactor()) + m_pushMovement;

        // camera follow
        m_camera.position = new Vector3(transform.position.x, transform.position.y, m_camera.position.z);
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
    }

    override protected void OnWalk(float h, float v)
    {
        base.OnWalk(h, v);
    }

    private void Walk()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        OnWalk(h, v);
    }
}
