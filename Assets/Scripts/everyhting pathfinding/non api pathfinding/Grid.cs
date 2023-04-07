using System.Runtime.ExceptionServices;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public  Vector3 gridSize;
    public GameObject nodePrefab;
    [SerializeField] Node[] nodesArray;
    [SerializeField] int totalNodesInGrid;
    Color defaultGridColor = new Color(0, 0.02276306f, 0.3207547f, 1);
    private void Awake()
    {
        totalNodesInGrid = (int)(gridSize.x * gridSize.z);
        nodesArray = new Node[totalNodesInGrid];
        InitilizeGridWithNodes();
       
    }


    void InitilizeGridWithNodes()
    {
        for (int zNodes = 0; zNodes < gridSize.z; zNodes++)
        {
            for (int xNodes = 0; xNodes < gridSize.x; xNodes++)
            {
                int index = (int)(xNodes + zNodes * gridSize.x);
                Vector3 gridSpawnPosition = new Vector3(xNodes, 0, zNodes);
                Vector3 worldSpawnPosition = new Vector3(gridSpawnPosition.x * gridSize.x,0, gridSpawnPosition.z *gridSize.z) + transform.position + (gridSize/2);
                nodesArray[index] = new Node(gridSpawnPosition, worldSpawnPosition);
                SpawnCubeAtNodeWorldPosition(worldSpawnPosition, index, nodesArray[index]);
               
            }
        }   
    }

    public Node getNodeBasedOnLocalPosition(Vector3 position)
    {
        int index =(int)(position.x + position.z * gridSize.x);
        return nodesArray[index];
    }


    void SpawnCubeAtNodeWorldPosition(Vector3 spawnPosition,int index,Node node)
    {
        node.Mesh = Instantiate(nodePrefab, spawnPosition, Quaternion.identity);
        node.Mesh.transform.localScale = gridSize- new Vector3(0.01f,0,0.01f);
        node.SetColorOfMesh(defaultGridColor);
    } 

    public void ClearWholeGridColor()
    {
        for(int i = 0; i < nodesArray.Length; i++)
        {
            nodesArray[i].SetColorOfMesh(defaultGridColor);
        }
    }

   
}
    