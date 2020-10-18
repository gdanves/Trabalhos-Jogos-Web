using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.isTrigger && col.gameObject.CompareTag("Player") && col.GetComponent<Player>().HasKey()) {
            // TODO: animation?
            Destroy(gameObject);
        }
    }
}
