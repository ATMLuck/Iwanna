using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key1 : MonoBehaviour
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
            Key_Lock1 opt_lock = opt.GetComponent<Key_Lock1>();
            opt_lock.isTrigger = true;
            gameObject.SetActive(false);
            
        }
    }
}
