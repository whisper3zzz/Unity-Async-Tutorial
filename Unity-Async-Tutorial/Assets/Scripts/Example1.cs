using System.Threading;
using UnityEngine;

public class Example1 : MonoBehaviour
{
    private void Start()
    {
        // Ensure MainThreadDispatcher is attached to a GameObject
        if (FindObjectOfType<MainThreadDispatcher>() == null)
        {
            gameObject.AddComponent<MainThreadDispatcher>();
        }

        // Create a new thread to perform heavy work
        Thread thread = new Thread(DoHeavyWork);
        Debug.Log("Starting thread: " + thread.ManagedThreadId);
        thread.Start();
        Debug.Log("Thread started!");
    }

    private void DoHeavyWork()
    {
        // Perform complex calculations on a background thread
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("Calculating... ");
            
            // Note: Do not directly manipulate Unity objects here!
        }

        // Pass the result back to the main thread
        MainThreadDispatcher.Enqueue(() => { Debug.Log("Calculation complete!"); });
    }
}