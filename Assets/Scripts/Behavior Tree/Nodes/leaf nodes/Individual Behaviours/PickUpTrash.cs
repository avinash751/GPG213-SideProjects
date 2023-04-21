using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTrash : LeafNode
{
    public int trashType;
    public PickUpTrash(int trashType)
    {
        this.trashType = trashType;
    }

    public float pickUpTrashTime = 0.5f;
    public override NodeState ExecuteNode()
    {
        if (NodeExecuted) return NodeState.Default;

        NodeExecuted = true;
        PickingUpTrash();
        return CurrentnodeState;
    }


    void PickingUpTrash()
    {
        Debug.Log("currenlty in picking up trash  node");
        float timePassed = 0;

        while (timePassed < pickUpTrashTime)
        {
            timePassed += Time.deltaTime;
        }

      
        if (trashType== 1)
        {
            Debug.Log("trash picked up successfully , as trash was correct type");
            CurrentnodeState = NodeState.Success;
            return;
        }

        CurrentnodeState = NodeState.Failure;
        Debug.Log("trash pick up failed , trash type was not correct type");
    }
}
