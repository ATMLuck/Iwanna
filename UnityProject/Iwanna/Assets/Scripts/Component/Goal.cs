using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    bool _isVectory=false;
    public Sprite activatedSprite;
    SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player") || _isVectory )return;
        _isVectory = true;
        _sr.sprite=activatedSprite;
        EventCenter.Broadcast(GameEvent.LevelComplete);
    }
}
