using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AsyncExample : MonoBehaviour
{
    async UniTaskVoid CalculateSumAsync(int a, int b)
    {
        Debug.Log("开始计算...");
        // 在后台线程执行耗时计算
        // await Task.Run(() =>
        // {
        //     Thread.Sleep(2000); // 模拟复杂计算
        // });
        await UniTask.Delay(2000); // 模拟复杂计算
        // 返回主线程后继续执行
        Debug.Log("计算结果：" + (a + b));
    }

    private async void Start()
    {
        CalculateSumAsync(3, 5).Forget();
        Debug.Log("显示UI...");
    }
}