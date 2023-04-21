using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOutTrash : LeafNode
{
    float takingTrashOutTimer =5;
    public override NodeState ExecuteNode()
    {
        if (NodeExecuted) return NodeState.Default;
        NodeExecuted = true;
        TakingOutTrash();
        return CurrentnodeState;
    }
    void TakingOutTrash()
    {
        Debug.Log("currenlty in TakingOutTrash node");
        float timePassed = 0;
        while (timePassed < takingTrashOutTimer)
        {
            timePassed += Time.deltaTime;
        }
        Debug.Log("trash taken out successfully, full behviour tree cycle  iteration completed");
        CurrentnodeState = NodeState.Success;
    }
}

