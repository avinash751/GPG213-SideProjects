using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    Astar aStar;
    bool followPath;
    List<Node> finalPath;

    [SerializeField] float speed;

    [SerializeField] Vector3 startposition;
    [SerializeField] Vector3 endposition;
    [SerializeField] int currentPathIndex;

    void Start()
    {
        aStar = FindObjectOfType<Astar>();
        followPath = aStar.FindBestPath(startposition, endposition, out finalPath);
        SetStartPositionForObject();
    }


    void Update()
    {
        MoveTowardsGoalPosition();
    }

    void SetStartPositionForObject()
    {
        if(followPath)
        {
            transform.position = finalPath[0].worldPosition;
          
        }
    }



    void MoveTowardsGoalPosition()
    {
        if (followPath)
        {

            Vector3 goalDirection = finalPath[currentPathIndex].worldPosition - transform.position;
            Vector3 normalisedGoalDirection = goalDirection.normalized;
            transform.position += normalisedGoalDirection * speed * Time.deltaTime;
            IncrementPathIndexBasedOnDistance();
  
        }
    }
    void IncrementPathIndexBasedOnDistance()
    {
        float distance = Vector3.Distance(transform.position, finalPath[currentPathIndex].worldPosition);

        if (distance <= 0.1f) currentPathIndex++;

        if (currentPathIndex >= finalPath.Count)
        {
            transform.position = finalPath[finalPath.Count - 1].worldPosition;
            followPath = false;
            return;
        }
        

       
    }
}
