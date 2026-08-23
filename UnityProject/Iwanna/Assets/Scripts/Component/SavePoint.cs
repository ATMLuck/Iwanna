using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    bool _isActivated = false;
    public Sprite activatedSprite;
    SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player") || _isActivated)
        {
            return;
        }
        _sr.sprite = activatedSprite;
        EventCenter.Broadcast(GameEvent.SavePointReached, transform.position);
        _isActivated = true;
    }
}
