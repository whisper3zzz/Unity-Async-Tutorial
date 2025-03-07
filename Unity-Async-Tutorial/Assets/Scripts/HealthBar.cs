using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class HealthBar : MonoBehaviour
{
    [Header("血条UI")]
    public Image insideBar;  // 内部血条（快速变化）
    public Image outsideBar; // 外部血条（缓慢变化）

    [Header("血量属性")]
    public AsyncReactiveProperty<float> health = new AsyncReactiveProperty<float>(100); // 响应式血量

    private float _lastHealth = 100; // 上一次的血量值

    [Header("模拟更新")]
    public Button addButton; // 增加血量按钮
    public Button subButton; // 减少血量按钮

    private void Start()
    {
        // 初始化血条
        insideBar.fillAmount = 1;
        outsideBar.fillAmount = 1;
        health.Value = 100;
        _lastHealth = health.Value;

        // 绑定按钮事件
        addButton.onClick.AddListener(OnClickAdd);
        subButton.onClick.AddListener(OnClickSub);

        // 监听血量变化
        health.WithoutCurrent() // 忽略初始值
              .Queue()         // 确保顺序执行
              .SubscribeAwait(async x =>
              {
                  Debug.Log($"当前血量为{x}，上一次的血量为{_lastHealth}，差值为{x - _lastHealth}，开始同步血量...");
                  await SyncHealth(x - _lastHealth, this.GetCancellationTokenOnDestroy()); // 同步血量
                  Debug.Log("同步血量完成！");
                  _lastHealth = x; // 更新上一次的血量
                  Debug.Log($"把上一次的血量更新为{_lastHealth}");
              });
    }

    // 增加血量
    private void OnClickAdd()
    {
        health.Value += 10;
    }

    // 减少血量
    private void OnClickSub()
    {
        health.Value -= 20;
    }

    // 同步血量变化
    private async UniTask SyncHealth(float delta, CancellationToken token = default)
    {
        if (delta == 0) return; // 无变化则直接返回

        // 确定快速变化和缓慢变化的血条
        Image quickBar = delta > 0 ? insideBar : outsideBar;
        Image slowBar = delta > 0 ? outsideBar : insideBar;

        // 更新快速血条
        quickBar.fillAmount = Mathf.Clamp01((quickBar.fillAmount * 100 + delta) / 100f);

        // 平滑过渡缓慢血条
        float changeTime = 0.75f; // 过渡时间
        while (changeTime > 0)
        {
            await UniTask.DelayFrame(1, cancellationToken: token); // 每帧等待
            changeTime -= Time.deltaTime;

            // 使用 Lerp 实现平滑过渡
            slowBar.fillAmount = Mathf.Lerp(quickBar.fillAmount, slowBar.fillAmount, changeTime / 0.75f);
        }

        // 确保最终值一致
        slowBar.fillAmount = quickBar.fillAmount;
    }
}