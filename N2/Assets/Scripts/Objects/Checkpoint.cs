using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] public SaveManager _saveManager;
    public int m_checkpoint;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.isTrigger && col.gameObject.CompareTag("Player")) {
            _saveManager.SetCheckpoint(m_checkpoint);
            Destroy(gameObject);
        }
    }
}
