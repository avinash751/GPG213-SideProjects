using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Idle : LeafNode
{
    public float idleTime = 2;
    public override NodeState ExecuteNode()
    {
        if (nodeExecuted) return NodeState.Default;

        nodeExecuted = true;
        StayIdle();
        return CurrentnodeState;
    }


    void  StayIdle()
    {
        Debug.Log("currenlty in idle node");
        float timePassed = 0;

        while (timePassed < idleTime)
        {
            timePassed += Time.deltaTime;
        }

        Debug.Log("idle node executed succesfuly ");
        CurrentnodeState = NodeState.Success;
    }
}
