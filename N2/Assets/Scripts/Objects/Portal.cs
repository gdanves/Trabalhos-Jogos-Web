using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameManager m_gameManager;

    void Start()
    {
        m_gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.isTrigger && col.gameObject.CompareTag("Player"))
            m_gameManager.EndGame(true);
    }
}
