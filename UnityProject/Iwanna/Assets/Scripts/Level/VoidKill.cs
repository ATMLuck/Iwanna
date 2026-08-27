using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }
        EventCenter.Broadcast(GameEvent.PlayerDeath);
    }
}
