using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    bool _isTriggered = false;
    private void OnEnable() => EventCenter.Subscribe(GameEvent.PlayerRespawned, OnRespawn);

    private void OnDisable() => EventCenter.Unsubscribe(GameEvent.PlayerRespawned, OnRespawn);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isTriggered) return;
        if (!collision.CompareTag("Player")) return;
        _isTriggered = true;
        EventCenter.Broadcast(GameEvent.PlayerDeath,null);
    }

    private void OnRespawn(object arg)
    {
        _isTriggered = false;
    }
}
