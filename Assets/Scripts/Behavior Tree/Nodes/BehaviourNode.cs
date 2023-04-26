using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourNode
{
    protected bool nodeExecuted;
    public bool NodeExecuted { get { return nodeExecuted;}}

    public enum NodeState
    {
        Success,
        Failure,
        Default
    }

   

    public NodeState CurrentnodeState = NodeState.Default;

    public virtual NodeState ExecuteNode()
    {
        return NodeState.Default;
        
    }
    public virtual void ResetNodeState(BehaviourNode node )
    {
        node.CurrentnodeState = NodeState.Default;
        node.nodeExecuted = false;
    }
}
