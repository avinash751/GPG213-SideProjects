using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Astar : MonoBehaviour
{
    [SerializeField] Grid grid;

    Vector3 currentNodePosition = Vector3.zero;
    Vector3 goalNodePosition =  Vector3.zero;

    Node startNode;
    Node currentNode;
    Node goalNode;

    bool startNodeIsSet;
    bool pathFound;

    int algorithimVersion;

    [SerializeField] List<Node> startNodeNeighboursList = new List<Node>(4);
    [SerializeField] List<Node> openNodeList = new List<Node>();
    //[SerializeField] List<Node> finalPathNodeList = new List<Node>();

    // Neibhour node offsets
    Vector3 topNodePosition = new Vector3(0, 0, 1);
    Vector3 bottomNodePosition = new Vector3(0, 0, -1);
    Vector3 leftNodePosition = new Vector3(-1, 0, 0);
    Vector3 rightNodePosition = new Vector3(1, 0, 0);

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public bool FindBestPath(Vector3 startNodeGridPosition, Vector3 goalNodeGridPosition, out List<Node> finalPath)
    {
        finalPath = new List<Node>();
        ResetAllAStarAlorithimAndItsStats(startNodeGridPosition, goalNodeGridPosition, ref finalPath);

        while (!pathFound)
        {
            InitialiseStartNodeAndAddToOpenList(); // to makse sure start node color does not get overidern
            if (openNodeList.Count <= 0) return false;

            SetNewCurrentNodeThatGetsVisitedBasedOnFcost();
            
            GetNeighboursForCurrentNode(grid.getNodeBasedOnLocalPosition(currentNodePosition), ref finalPath);
            ResetNeigbhourNodeStatsBasedOnAlgorithimVersion();

            SetParentNodeForNeigbhouringNodes();
            SetHcostGcostForNeigbhouringNodes();

            AddNodesToOpenList();
            CheckWhetherCurrentNodeIsGoal(currentNodePosition, ref finalPath);
        }
        return pathFound;
    }

    void ResetAllAStarAlorithimAndItsStats(Vector3 startNodeGridPosition, Vector3 goalNodeGridPosition, ref List<Node> finalPath)
    {
        SetNewGoalAndStartNodeAndItsPostions(startNodeGridPosition, goalNodeGridPosition);
        ResetAStarPathRelatedStats(ref finalPath);
    }

    void InitialiseStartNodeAndAddToOpenList()
    {
        if (!startNodeIsSet)
        {
            startNodeIsSet = true;
            openNodeList.Add(startNode);
            startNode.isVisited = true;
        }
    }

    void GetNeighboursForCurrentNode(Node node, ref List<Node> finalPath)
    {
        Vector3 startNodePosition = currentNodePosition;

        startNodeNeighboursList.Clear();

        if (startNodePosition.x + 1 < grid.gridSize.x)
        {
            GetNeighbourNodeAndAddToListAndCheckIfItsGoalNode(startNodePosition + rightNodePosition, ref finalPath);
        }

        if (startNodePosition.x - 1 >= 0)
        {
            GetNeighbourNodeAndAddToListAndCheckIfItsGoalNode(startNodePosition + leftNodePosition, ref finalPath);
        }

        if (startNodePosition.z + 1 < grid.gridSize.z)
        {
            GetNeighbourNodeAndAddToListAndCheckIfItsGoalNode(startNodePosition + topNodePosition, ref finalPath);
        }

        if (startNodePosition.z - 1 >= 0)
        {
            GetNeighbourNodeAndAddToListAndCheckIfItsGoalNode(startNodePosition + bottomNodePosition, ref finalPath);
        }
    }

    void AddNodesToOpenList()
    {
        for (int i = 0; i < startNodeNeighboursList.Count; i++)
        {
            if (!startNodeNeighboursList[i].isVisited && !openNodeList.Contains(startNodeNeighboursList[i]) && startNodeNeighboursList[i].localPosition != startNode.localPosition)
            {
                openNodeList.Add(startNodeNeighboursList[i]);
                startNodeNeighboursList[i].SetColorOfMesh(Color.grey);
            }
        }
    }

    void SetNewCurrentNodeThatGetsVisitedBasedOnFcost()
    {
        // if its empty it will not set new curreent node or remove node from open list 
        if (openNodeList.Count == 0) return;

        openNodeList.Sort();
        currentNodePosition = openNodeList[0].localPosition;
        currentNode = openNodeList[0];
        currentNode.SetColorOfMesh(Color.magenta);
        openNodeList[0].isVisited = true;
        openNodeList.RemoveAt(0);
    }

    void SetParentNodeForNeigbhouringNodes()
    {
        for (int i = 0; i < startNodeNeighboursList.Count; i++)
        {
            SetAParentForANode(startNodeNeighboursList[i]);
        }
    }

    void SetAParentForANode(Node nodeToSetParent)
    {
        Node newParent = currentNode;

        if (nodeToSetParent.localPosition == startNode.localPosition) { return; }
        if (nodeToSetParent.parentNode == null)
        {
            nodeToSetParent.parentNode = currentNode;
            return;
        }
        // this is to make sure is set a parent of lowest fcost so in the end it will give the correct and the most effceint path
        if (nodeToSetParent.parentNode.Fcost <= newParent.Fcost) { return; }
        nodeToSetParent.parentNode = currentNode;

    }

    void SetHcostGcostForNeigbhouringNodes()
    {
        for (int i = 0; i < startNodeNeighboursList.Count; i++)
        {
            startNodeNeighboursList[i].Hcost = GetDistnaceFromTwoNodePositions(startNodeNeighboursList[i].localPosition, goalNodePosition);

            float newGcost = GetDistnaceFromTwoNodePositions(startNodeNeighboursList[i].localPosition, currentNodePosition);
            // this is make sure that i dont get null refrences and  it does not set start node g cost 
            if (startNodeNeighboursList[i].parentNode != null)
            {
                newGcost += startNodeNeighboursList[i].parentNode.Gcost;
            }
            if (newGcost < startNodeNeighboursList[i].Gcost || startNodeNeighboursList[i].Gcost == 0 && startNodeNeighboursList[i] != startNode)
            {
                startNodeNeighboursList[i].Gcost = newGcost;
            }
        }
    }

    float GetDistnaceFromTwoNodePositions(Vector3 startPosition, Vector3 destinationPosition)
    {
        float distnace = Mathf.Abs((destinationPosition.x - startPosition.x)) +
                         Mathf.Abs((destinationPosition.y - startPosition.y)) +
                         Mathf.Abs((destinationPosition.z - startPosition.z));
        return distnace;
    }

    void GetNeighbourNodeAndAddToListAndCheckIfItsGoalNode(Vector3 nodePosition, ref List<Node> finalPath)
    {
        Node node = grid.getNodeBasedOnLocalPosition(nodePosition);
        startNodeNeighboursList.Add(node);
        CheckWhetherCurrentNodeIsGoal(nodePosition, ref finalPath);
    }

    Node GetNodeBasedOnPositionAndSetColorOfMesh(Vector3 nodePosition, Color meshColor, bool returnNode)
    {
        Node node = grid.getNodeBasedOnLocalPosition(nodePosition);
        node.SetColorOfMesh(meshColor);

        if (returnNode) return node;
        else return null;
    }

    bool CheckWhetherCurrentNodeIsGoal(Vector3 nodePostion, ref List<Node> finalPath)
    {
        if (nodePostion == goalNodePosition)
        {
            pathFound = true;
            Node checkedNode = GetNodeBasedOnPositionAndSetColorOfMesh(goalNodePosition, Color.red, true);
            SetAParentForANode(checkedNode);
            // this is to make sure that as soon as goal node is found it will show the final path immedialty;
            GetAndShowFinalPath(checkedNode, ref finalPath);
            finalPath.Reverse();
            return true;
        }
        return false;
    }

    void GetAndShowFinalPath(Node node, ref List<Node> finalPath)
    {
        
        finalPath.Add(node);
        node.SetColorOfMesh(Color.white);
        if (node.parentNode != null)
        {
            GetAndShowFinalPath(node.parentNode, ref finalPath);
        }
        return;
    }

    private void ResetNeigbhourNodeStatsBasedOnAlgorithimVersion()
    {
        for (int i = 0; i < startNodeNeighboursList.Count; i++)
        {
            if (startNodeNeighboursList[i].nodeVersion < algorithimVersion)
            {
                ResetNodeStats(startNodeNeighboursList[i]);
                startNodeNeighboursList[i].nodeVersion = algorithimVersion;
                startNodeNeighboursList[i].isVisited = false;
            }
        }
    }

    void ResetAStarPathRelatedStats(ref List<Node> finalPath)
    {
        if (!pathFound) return;
        openNodeList.Clear();
        finalPath.Clear();
        grid.ClearWholeGridColor();
        algorithimVersion++;
        pathFound = false;
        startNodeIsSet = false;

        ResetNodeStats(startNode);
        ResetNodeStats(goalNode);
    }

    void ResetNodeStats(Node node)
    {
        node.Hcost = 0;
        node.Gcost = 0;
        node.Fcost = 0;
        node.parentNode = null;
    }

    void SetNewGoalAndStartNodeAndItsPostions(Vector3 startNodeGridPosition, Vector3 goalNodeGridPosition)
    {
        goalNodePosition = goalNodeGridPosition;
        startNode = GetNodeBasedOnPositionAndSetColorOfMesh(startNodeGridPosition, Color.green, true);
        goalNode = GetNodeBasedOnPositionAndSetColorOfMesh(goalNodePosition, Color.cyan, true);
    }
}

