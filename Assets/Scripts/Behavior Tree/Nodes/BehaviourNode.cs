using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourNode
{
    public bool NodeExecuted;
    public enum NodeState
    {
        Success,
        Failure,
        Default
    }

    public List<BehaviourNode> childrenNodes = new List<BehaviourNode>();

    public NodeState CurrentnodeState = NodeState.Default;

    public virtual NodeState ExecuteNode()
    {
        return NodeState.Default;
    }
    public virtual void ResetNodeState(BehaviourNode node )
    {
        node.CurrentnodeState = NodeState.Default;
        node.NodeExecuted = false;
    }
}
