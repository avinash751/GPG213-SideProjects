using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentNode : BehaviourNode
{
    protected List<BehaviourNode> childrenNodes = new List<BehaviourNode>();
    public  virtual ParentNode AddChild(BehaviourNode child)
    {
        childrenNodes.Add(child);
        return this;
    }
    public  virtual ParentNode RemoveChild(BehaviourNode child)
    {
        childrenNodes.Remove(child);
        return this;
    }

   

    public override void ResetNodeState(BehaviourNode node)
    {
        base.ResetNodeState(node);

        if (node is LeafNode ) return;

        ParentNode parentNode = null;

        if (node is ParentNode ) {  parentNode = node as  ParentNode; }

        for( int i = 0; i < parentNode.childrenNodes.Count; i++)
        {
            ResetNodeState(parentNode.childrenNodes[i]);
        }
    }
}
