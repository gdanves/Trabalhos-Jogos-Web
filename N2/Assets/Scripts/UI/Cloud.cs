using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float m_speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(m_speed, 0);
    }

    void FixedUpdate()
    {
        // repeat movement path
        if(transform.position.x > 10)
            transform.position = new Vector3(-10, transform.position.y, transform.position.z);
    }
}
