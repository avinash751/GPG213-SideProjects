using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mop : LeafNode
{
    public int trashType;
    public Mop(int trashType)
    {
        this.trashType = trashType;
    }

    public float MoppingTIme = 0.5f;
    public override NodeState ExecuteNode()
    {
        if (NodeExecuted) return NodeState.Default;

        NodeExecuted = true;
        MoppingUpTrash();
        return CurrentnodeState;
    }


    void MoppingUpTrash()
    {
        Debug.Log("currenlty in mopping trash  node");
        float timePassed = 0;

        while (timePassed < MoppingTIme)
        {
            timePassed += Time.deltaTime;
        }


        if (trashType != 1)
        {
            Debug.Log("trash moped successfully , as trash was correct type  ");
            CurrentnodeState = NodeState.Success;
            return;
        }

        CurrentnodeState = NodeState.Failure;
        Debug.Log("trash moping failed  , as  trash type was not correct type");
    }
}
