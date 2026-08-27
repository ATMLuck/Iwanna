using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key2 : MonoBehaviour
{
    [SerializeField] GameObject opt;
    private int bulletLayer;

    private void Awake()
    {
        bulletLayer = LayerMask.NameToLayer("Bullet");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == bulletLayer)
        {
            Key_Lock2 opt_lock = opt.GetComponent<Key_Lock2>();
            opt_lock.isTrigger = true;
            gameObject.SetActive(false);
        }
    }
}

