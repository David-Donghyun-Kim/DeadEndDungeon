using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    static Queue<ICommand> commandBuffer;

    private void Awake()
    {
        commandBuffer = new Queue<ICommand>();
    }

    public static void AddCommand(ICommand c)
    {
        commandBuffer.Enqueue(c);
    }

    // Update is called once per frame
    void Update()
    {
        while (commandBuffer.Count > 0 )
        {
            commandBuffer.Dequeue().Execute();
        }
    }
}
