using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class ReactiveHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // 血量条 UI
    [SerializeField] private Image fillImage; // 血量条填充图像
    [SerializeField] private Color normalColor = Color.green; // 正常颜色
    [SerializeField] private Color warningColor = Color.red; // 警告颜色
    [SerializeField] private float warningThreshold = 0.3f; // 警告阈值（30%）

    private float _health = 1f; // 当前血量（0-1）
    private CancellationTokenSource _cts;

    void Update()
    {
        // 模拟血量变化
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(0.1f);
        }
    }
    void Start()
    {
        // 启动响应式逻辑
        _cts = new CancellationTokenSource();
        HealthUpdateLoop(_cts.Token).Forget();
    }

    void OnDestroy()
    {
        // 组件销毁时取消任务
        _cts?.Cancel();
    }

    // 响应式血量更新逻辑
    private async UniTaskVoid HealthUpdateLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            // 监听血量变化
            await UniTask.WaitUntilValueChanged(this, x => x._health, cancellationToken: token);

            // 更新血量条
            healthSlider.value = _health;

            // 根据血量变化调整颜色
            if (_health <= warningThreshold)
            {
                fillImage.color = warningColor;
                await ShowWarningAsync(token); // 显示警告
            }
            else
            {
                fillImage.color = normalColor;
            }
        }
    }

    // 模拟血量变化（外部调用）
    public void TakeDamage(float damage)
    {
        _health = Mathf.Clamp01(_health - damage);
    }

    // 显示警告逻辑
    private async UniTask ShowWarningAsync(CancellationToken token)
    {
        Debug.Log("血量过低，警告！");
        float blinkInterval = 0.2f;
        int blinkCount = 5;

        for (int i = 0; i < blinkCount; i++)
        {
            fillImage.enabled = !fillImage.enabled; // 闪烁效果
            await UniTask.Delay((int)(blinkInterval * 1000), cancellationToken: token);
        }

        fillImage.enabled = true; // 恢复显示
    }
}