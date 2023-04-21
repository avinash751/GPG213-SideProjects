using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeManager : MonoBehaviour
{
    SequenceNode startSequenceNode;
    RepeaterNode rootNode;
    SelectorNode CleanNode;

    Idle idleNode;
    Wander wanderNode;
    ScanForTrash scanForTrashNode;
    DetermineTrashType determineTrashTypeNode;
    PickUpTrash pickUpTrashNode;
    Mop mopNode;
    TakeOutTrash takeOutTrashNode;
    int trashType;



    // Start is called before the first frame update
    void Start()
    {
        InitializeAllNodes();
        AddChildrenToRespectiveNodesToMakeBehviourTree();
    }

    private void AddChildrenToRespectiveNodesToMakeBehviourTree()
    {
        rootNode.AddChild(startSequenceNode);
        startSequenceNode.AddChild(idleNode);
        startSequenceNode.AddChild(wanderNode);
        startSequenceNode.AddChild(scanForTrashNode);
        startSequenceNode.AddChild(determineTrashTypeNode);
        startSequenceNode.AddChild(CleanNode);
        CleanNode.AddChild(pickUpTrashNode);
        CleanNode.AddChild(mopNode);
        startSequenceNode.AddChild(takeOutTrashNode);
       
    }

    private void InitializeAllNodes()
    {
        rootNode = new RepeaterNode(true);
        startSequenceNode = new SequenceNode();
        CleanNode = new SelectorNode();
        idleNode = new Idle();
        wanderNode = new Wander();
        scanForTrashNode = new ScanForTrash();
        determineTrashTypeNode = new DetermineTrashType(ref trashType);
        pickUpTrashNode = new PickUpTrash(trashType);
        mopNode = new Mop(trashType);
        takeOutTrashNode = new TakeOutTrash();
    }

    void Update()
    {
        if (!rootNode.NodeExecuted)
        {
            rootNode.ExecuteNode();
        }
        
    }
}
