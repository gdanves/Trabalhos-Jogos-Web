using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Creature
{
    // physics
    protected Rigidbody2D m_rb;

    // components
    [SerializeField] public SaveManager _saveManager;
    public List<Transform> checkpointPositions;
    protected AudioSource m_audioSource;
    private Transform m_camera;

    private bool m_hasKey = false;

    protected override void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();
        m_camera = Camera.main.transform;
        base.Start();

        if(_saveManager) {
            int checkpoint = _saveManager.GetCheckpoint();
            if(checkpoint > 0)
                transform.position = checkpointPositions[checkpoint - 1].position;
        }
    }

    protected override void Update()
    {
        if(Input.GetButtonUp("Cancel"))
            m_gameManager.PauseGame();

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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void Hit(int value)
    {
        if (IsDead())
            return;

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || m_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            m_animator.Play("Hit");

        StartBlink();
        AddStun(300);
        AddHealth(-value);
        m_gameManager.SetHealthPercent(Mathf.Clamp(GetHealth().x/GetHealth().y, 0, 1));
    }

    public bool HasKey()
    {
        return m_hasKey;
    }

    public void OnCollectKey()
    {
        m_hasKey = true;
    }

    protected override void Die()
    {
        m_animator.Play("Death");
        Time.timeScale = 0.35f;
        Invoke("EndGame", 0.7f);
    }

    protected override void OnWalk(float h)
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

    private void EndGame()
    {
        m_gameManager.EndGame();
    }
}
