using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SkillCdUniTask : MonoBehaviour
{
    public float cdTime = 3f;
    private bool _isReady = true;
    private CancellationTokenSource _cts; // 用于取消任务

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CastSkill();
        }
    }

    private async void CastSkill()
    {
        if (_isReady)
        {
            _isReady = false;
            Debug.Log("技能释放！");
            _cts?.Cancel(); // 取消之前的任务（防止重复调用）
            _cts = new CancellationTokenSource();
            StartCdAsync(_cts.Token).Forget();
        }
        else if (!_isReady)
        {
            Debug.Log("技能冷却中...");
        }
    }

    private async UniTaskVoid StartCdAsync(CancellationToken token)
    {
        await UniTask.Delay((int)(cdTime * 1000), cancellationToken: token);
        _isReady = true;
        Debug.Log("技能已就绪！");
    }

    void OnDestroy()
    {
        _cts?.Cancel(); // 对象销毁时取消任务
    }
}