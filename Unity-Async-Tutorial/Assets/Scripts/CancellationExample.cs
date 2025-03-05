using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

public class CancellationExample : MonoBehaviour
{
    private CancellationTokenSource _cts;

    private void Start()
    {
        StartDownloadAsync();
    }

    // 启动一个可取消的异步任务
    private async UniTaskVoid StartDownloadAsync()
    {
        _cts = new CancellationTokenSource();
        try
        {
            // 将 Token 传递给异步方法
            await DownloadFileAsync("https://example.com/file", _cts.Token);
            Debug.Log("下载完成！");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("任务被取消！");
        }
        finally
        {
            _cts?.Dispose(); // 释放资源
            _cts = null;
        }
    }

    // 模拟下载任务
    async UniTask DownloadFileAsync(string url, CancellationToken token)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(3)); // 3 秒后自动取消任务
        for (int i = 0; i < 100; i++)
        {
            token.ThrowIfCancellationRequested(); // 检查是否取消
            await UniTask.Delay(1000, cancellationToken: cts.Token); // 模拟分块下载
            Debug.Log($"进度: {i}%");
        }
    }

    // 手动取消任务（如按钮点击）
    public void CancelDownload()
    {
        _cts?.Cancel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("手动取消任务！");
            CancelDownload();
        }
    }

    // 对象销毁时自动取消
    void OnDestroy()
    {
        _cts?.Cancel();
    }
}