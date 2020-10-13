using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    // physics
    protected Rigidbody2D m_rb;

    // components
    private Transform m_camera;

    override protected void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_camera = Camera.main.transform;
        base.Start();
    }

    override protected void Update()
    {
        //if(Input.GetButtonUp("Cancel"))
            //m_gameManager.PauseGame();

        if(IsDead()) {
            m_rb.velocity *= 0f;
            return;
        }

        if(CanWalk())
            Walk();
        else
            StopWalk();
 
        base.Update();

        //m_rb.velocity = (m_movement * GetSpeedFactor()) + m_pushMovement;

        // camera follow
        m_camera.position = new Vector3(transform.position.x, transform.position.y, m_camera.position.z);
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
    }

    override protected void OnWalk(float h)
    {
        base.OnWalk(h);
    }

    private void Walk()
    {
        float h = Input.GetAxisRaw("Horizontal");
        // vertical movement is currently not used
        //float v = Input.GetAxisRaw("Vertical");
        OnWalk(h);
    }
}
