using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Delay : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        Debug.Log("Start");
        PlaySound();
        Debug.Log("End");
    }

    // Update is called once per frame
    void Update()
    {
    }

    async UniTaskVoid PlaySound()
    {
        Debug.Log("Start playing sound");
        await UniTask.Delay(1000);
        Debug.Log("End playing sound");
    }
}