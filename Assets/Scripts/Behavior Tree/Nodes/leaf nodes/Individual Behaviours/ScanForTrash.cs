using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScanForTrash : LeafNode
{
    public float scanForTrashTime = 0.5f;
    public override NodeState ExecuteNode()
    {
        if (NodeExecuted) return NodeState.Default;

        NodeExecuted = true;
        ScanningForTrash();
        return CurrentnodeState;
    }


    void  ScanningForTrash()
    {
        Debug.Log("currenlty in scanForTrash node");
        float timePassed = 0;

        while(timePassed < scanForTrashTime)
        {
            timePassed += Time.deltaTime;
        }

        int trashFound = Random.Range(0,4);

        if(trashFound == 1)
        {
            Debug.Log("trash  node executed succesfuly, as trash was found  ");
            CurrentnodeState = NodeState.Success;
            return;
        }

       
        CurrentnodeState = NodeState.Failure;
        Debug.Log("trash  node failed , as trash was not found ");
    }
}
