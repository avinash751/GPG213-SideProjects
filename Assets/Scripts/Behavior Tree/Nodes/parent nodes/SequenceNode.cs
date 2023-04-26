using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : ParentNode
{
    public override NodeState ExecuteNode()
    {
        for (int i = 0; i < childrenNodes.Count; i++)
        {
            Debug.Log("currently on sequence node");
            childrenNodes[i].ExecuteNode();

            // this makes sure that even if one child is a failure the sequence node will fail and exit out of the loop 
            if (childrenNodes[i].CurrentnodeState == NodeState.Failure)
            {
                CurrentnodeState = NodeState.Failure;
                Debug.Log("sequence node failed execution");
                return CurrentnodeState;
            }
            // this prevents the sequence node from exiting and continue to check all children
            else if (childrenNodes[i].CurrentnodeState == NodeState.Success)
            {
                CurrentnodeState = NodeState.Success;
            }

            else if (childrenNodes[i].CurrentnodeState == NodeState.Default)
            {
                CurrentnodeState = NodeState.Default;
            }
        }

        if (CurrentnodeState == NodeState.Success || CurrentnodeState == NodeState.Failure)
        {
            nodeExecuted = true;
            Debug.Log("sequence node executed succesfully");
        }
        return CurrentnodeState;
    }
}
