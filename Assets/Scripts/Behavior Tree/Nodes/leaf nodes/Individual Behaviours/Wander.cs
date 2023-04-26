using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Wander: LeafNode
{
    public float wanderTime = 1f;
    public override NodeState ExecuteNode()
    {
        if (nodeExecuted) return NodeState.Default;

        nodeExecuted = true;
        Wandering();
        return CurrentnodeState;
    }


    void  Wandering()
    {
        Debug.Log("currenlty in wander node");

        float timePassed = 0;

        while (timePassed < wanderTime)
        {
            timePassed += Time.deltaTime;
        }
        Debug.Log("wander  node executed succesfuly ");
        CurrentnodeState = NodeState.Success;
    }
}
