using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterNode : ParentNode
{
    protected bool repeatOnFailure;

    public RepeaterNode(bool repeatOnFailure)
    {
        this.repeatOnFailure = repeatOnFailure;
    }
    public override NodeState ExecuteNode()
    {
        for (int i = 0; i < childrenNodes.Count; i++)
        {
            Debug.Log("currently on repeat node");
            childrenNodes[i].ExecuteNode();

            // repeats the node execution if children node state is faiure

            if (childrenNodes[i].CurrentnodeState == NodeState.Failure && repeatOnFailure)
            {
                Debug.Log("repeat execution  on failure in progress");
                ResetNodeState(this);
                i = 0;
                continue;

            }
            else if (childrenNodes[i].CurrentnodeState == NodeState.Success && repeatOnFailure)
            {
                CurrentnodeState = childrenNodes[i].CurrentnodeState;
                nodeExecuted = true;
                Debug.Log("repeat node executed succesfully ");
                return CurrentnodeState;
            }

            // repeats the node execution if children node state is success
            else if (childrenNodes[i].CurrentnodeState == NodeState.Success && !repeatOnFailure)
            {
                Debug.Log("repeat execution  on success in progress");
                ResetNodeState(this);
                i = 0;
                continue;
            }
            else if (childrenNodes[i].CurrentnodeState == NodeState.Failure && !repeatOnFailure)
            {
                CurrentnodeState = childrenNodes[i].CurrentnodeState;
                nodeExecuted = true;
                Debug.Log("repeat node executed succesfully ");
                return CurrentnodeState;
            }
        }
        return CurrentnodeState;
    }
}
