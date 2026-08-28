using UnityEngine;

// 挂载到开始界面里那个用来展示待机动画的角色物体上
// 作用：确保这个角色一直停在"待机(Idle)"状态，不会被其他脚本/输入意外切换到别的动画
public class MenuIdleCharacter : MonoBehaviour
{
    [Header("Animator里待机状态的名字，要跟状态机里的State名字完全一致")]
    public string idleStateName = "Idle";

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (animator != null)
        {
            animator.Play(idleStateName, 0, 0f);
        }
    }
}