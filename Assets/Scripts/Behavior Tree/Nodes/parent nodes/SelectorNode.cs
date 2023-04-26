using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : ParentNode
{


    public override NodeState ExecuteNode()
    {
        for (int i = 0; i < childrenNodes.Count; i++)
        {
            Debug.Log("currently on selector node");
            childrenNodes[i].ExecuteNode();
            
            if (childrenNodes[i].CurrentnodeState == NodeState.Success)
            {
                CurrentnodeState = NodeState.Success;
                Debug.Log("selector node executed succesfully");
                nodeExecuted = true;
                return CurrentnodeState;
            }
            else if (childrenNodes[i].CurrentnodeState == NodeState.Failure)
            {
                CurrentnodeState = NodeState.Failure;
                Debug.Log("selector current child node failed execution ");
                nodeExecuted = true;
                continue;
            }
            else if (childrenNodes[i].CurrentnodeState == NodeState.Default)
            {
                CurrentnodeState = NodeState.Default;
                Debug.Log("selector node execution pending");
            }
        }
        return CurrentnodeState;
    }
       
    

   
}
