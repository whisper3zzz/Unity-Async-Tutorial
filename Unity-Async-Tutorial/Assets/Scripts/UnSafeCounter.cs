using System.Threading;
using UnityEngine;

public class UnsafeCounter : MonoBehaviour
{
    // Shared counter without any thread-safety
    private int _counter = 0;

    private void Start()
    {
        // Create and start multiple threads that increment the counter concurrently
        for (int i = 0; i < 5; i++)
        {
            Thread thread = new Thread(IncrementCounter);
            thread.Start();
        }
    }

    private void IncrementCounter()
    {
        // Each thread increments the counter 1000 times without any locking mechanism,
        // which can lead to race conditions and incorrect final counter value.
        for (int i = 0; i < 1000; i++)
        {
            _counter++;
        }

        Debug.Log("Thread finished. Counter: " + _counter);
    }
}