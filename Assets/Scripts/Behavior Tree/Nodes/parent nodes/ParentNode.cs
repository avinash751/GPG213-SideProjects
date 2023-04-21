using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentNode : BehaviourNode
{
  
    public virtual  void AddChild(BehaviourNode child)
    {
        childrenNodes.Add(child);
    }
    public virtual void RemoveChild(BehaviourNode child)
    {
        childrenNodes.Remove(child);
    }

    public virtual NodeState EvaluateNode(int nodeIndex)
    {
        return CurrentnodeState;
    }

    public override void ResetNodeState(BehaviourNode node)
    {
        base.ResetNodeState(node);

        if (node is LeafNode || node.childrenNodes.Count<=0) return;

        for( int i = 0; i < node.childrenNodes.Count; i++)
        {
            ResetNodeState(node.childrenNodes[i]);
        }
    }
}
