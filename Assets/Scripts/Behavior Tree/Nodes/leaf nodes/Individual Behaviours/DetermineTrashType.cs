using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetermineTrashType : LeafNode
{
    int trashType;

    public DetermineTrashType(ref int trashType)
    {
        trashType = Random.Range(0, 4);
        this.trashType = trashType;
    }
    public override NodeState ExecuteNode()
    {
        if (nodeExecuted) return NodeState.Default;

        nodeExecuted = true;
        ScanningForTrash();
        return CurrentnodeState;
    }

    void ScanningForTrash()
    {
        Debug.Log("currenlty in determine trash type  node");

        if (trashType == 1)
        {
            Debug.Log("trash type /solid/ was detrmined successfuly");
            CurrentnodeState = NodeState.Success;
            return;
        }
        Debug.Log("trash type /liquid/ was detrmined successfuly");
        CurrentnodeState = NodeState.Success;
    }
}