using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float[] waitTimes;
    [SerializeField] float speed;
    [SerializeField] bool startMove;

    [SerializeField] bool loopPlatform;
    [SerializeField] bool pingPongPlatform;
    [SerializeField] bool randomPlatform;

    Transform newParent;
    Transform playerTransform;
    int waypointIndex;
    int previousWaypointIndex = -1;
    bool isWaiting;
    bool isReversing;

    void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        newParent = GameObject.Find("Transform Positions").transform;
        Transform[] allTransforms = GetComponentsInChildren<Transform>();
        waypoints = new Transform[allTransforms.Length - 1];
        int index = 0;
        foreach (Transform t in allTransforms)
        {
            if (t != transform)
            {
                waypoints[index] = t;
                t.SetParent(newParent);
                index++;
            }
        }
        if (waitTimes.Length != waypoints.Length)
            Debug.LogError("The number of wait times must match the number of waypoints!");
    }

    void Update()
    {
        if (waypoints.Length == 0) 
            return;
        if (startMove && !isWaiting)
            MoveObject();

    }
    void MoveObject()
    {
        Transform targetWaypoint = waypoints[waypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            StartCoroutine(WaitAtWaypoint());
    }
    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimes[waypointIndex]);
        UpdateWaypointIndex();
        isWaiting = false;
    }
    void UpdateWaypointIndex()
    {
        if (loopPlatform)
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        else if (randomPlatform)
        {
            int newWaypointIndex;
            do
            {
                newWaypointIndex = Random.Range(0, waypoints.Length);
            } while (newWaypointIndex == previousWaypointIndex);

            previousWaypointIndex = waypointIndex;
            waypointIndex = newWaypointIndex;
        }
        else if (pingPongPlatform)
        {
            if (isReversing)
            {
                waypointIndex--;
                if (waypointIndex <= 0)
                {
                    waypointIndex = 0;
                    isReversing = false;
                }
            }
            else
            {
                waypointIndex++;
                if (waypointIndex >= waypoints.Length - 1)
                {
                    waypointIndex = waypoints.Length - 1;
                    isReversing = true;
                }
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            playerTransform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform.SetParent(null);
            playerTransform = null;
        }
    }
}
