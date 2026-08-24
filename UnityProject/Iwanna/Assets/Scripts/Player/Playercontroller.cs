using UnityEngine;

// ⚠️ 这是一个临时占位版本，只是为了让项目能先编译通过 ⚠️

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        // 让GameManager知道场景里的玩家是谁
        // 前提是GameManager.Instance.RegisterPlayer(PlayerController player) 这个方法是可以正常调用的
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPlayer(this);
        }
    }
}