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

        // ---------- 等比替换 begin ----------
        Sprite oldSprite = _sr.sprite;           // 先缓存原图
        Vector3 oldScale = transform.localScale; // 缓存当前缩放

        _sr.sprite = activatedSprite;            // 替换新图

        if (oldSprite != null && activatedSprite != null)
        {
            // 获取两张图在"单位缩放"下的世界空间尺寸（由 PPU 和像素尺寸决定）
            Vector3 oldSize = oldSprite.bounds.size;
            Vector3 newSize = activatedSprite.bounds.size;

            // 计算需要补偿的缩放比例，使新图显示大小和原图一致
            // 如果要严格"等比不拉伸"，取 Min 保证不变形
            float scaleFactor = Mathf.Min(
                oldSize.x / newSize.x,
                oldSize.y / newSize.y
            );

            transform.localScale = new Vector3(
                oldScale.x * scaleFactor,
                oldScale.y * scaleFactor,
                oldScale.z
            );
        }
        EventCenter.Broadcast(GameEvent.SavePointReached, transform.position);
        _isActivated = true;
    }
}
