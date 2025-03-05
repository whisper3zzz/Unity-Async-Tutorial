using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThreadSafeCounter : MonoBehaviour
{
    // A shared resource that needs protection
    private int _counter = 0;
    
    // Object used for locking
    private readonly object _lockObject = new object();
    
    // Thread-safe way to increment the counter
    public void IncrementCounter()
    {
        // Lock ensures only one thread can execute this block at a time
        lock (_lockObject)
        {
            _counter++;
            Debug.Log($"Counter incremented to: {_counter}");
        }
    }
    
    // Thread-safe way to get the current count
    public int GetCount()
    {
        // Lock ensures only one thread can execute this block at a time
        lock (_lockObject)
        {
            return _counter;
        }
        
    }
    
    private void Start()
    {
        // Create multiple threads that will access the counter
        for (int i = 0; i < 5; i++)
        {
            int threadNumber = i;
            Thread thread = new Thread(() => WorkerThread(threadNumber));
            thread.Start();
        }
    }
    
    private void WorkerThread(int threadId)
    {
        Debug.Log($"Thread {threadId} started");
        
        // Each thread increments the counter multiple times
        for (int i = 0; i < 10; i++)
        {
            IncrementCounter();
            var rng = new System.Random();
            Thread.Sleep(rng.Next(10, 100)); // Simulate work
          //  Thread.Sleep(Random.Range(10, 100)); // Simulate work
        }
        
        // Report final value back to main thread
        MainThreadDispatcher.Enqueue(() => 
        {
            Debug.Log($"Thread {threadId} completed. Final count: {GetCount()}");
        });
    }
}