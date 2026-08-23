using System;
using System.Collections.Generic;

// 广播事件清单
public enum GameEvent
{
    PlayerDeath,        // 玩家死亡
    PlayerRespawned,    // 玩家重生完成
    SavePointReached,   // 触碰存档点
    LevelComplete,      // 到达终点通关
    DeathCountChanged,  // 死亡次数变化
    TimerTick,          // 计时
    Pause, Resume       // 暂停/恢复
}

// 广播事件接口
public static class EventCenter
{
    static readonly Dictionary<GameEvent, Action<object>> _events = new();
    // 订阅：登记"我想听这个事件"
    public static void Subscribe(GameEvent e, Action<object> handler)
    {
        if (!_events.TryGetValue(e, out _)) _events[e] = delegate { };
        _events[e] += handler;   // 追加回调（多播委托，可挂多个）
    }
    // 退订：取消登记（临时对象销毁前必须调用）
    public static void Unsubscribe(GameEvent e, Action<object> handler)
    {
        if (_events.TryGetValue(e, out var list)) _events[e] -= handler;
    }
    // 广播：把"事件 + 参数"发给所有订阅者
    public static void Broadcast(GameEvent e, object arg = null)
    {
        if (_events.TryGetValue(e, out var list)) list?.Invoke(arg);
    }
}