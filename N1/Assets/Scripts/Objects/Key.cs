using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.isTrigger && col.gameObject.CompareTag("Player")) {
            col.GetComponent<Player>().OnCollectKey();
            Destroy(gameObject);
        }
    }
}
