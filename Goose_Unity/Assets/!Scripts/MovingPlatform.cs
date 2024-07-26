using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform[] waypoints; //List of all waypoints
    [SerializeField] float[] waitTimes;     //List of wait times when it reaches a waypoint
    [SerializeField] float[] targetRotations;   //List of rotation values to rotate to when it reaches a waypoint
    [SerializeField] float speed;               //Platform Move Speed
    [SerializeField] bool startMove;            //Start Platform Move
    [SerializeField] bool dontMovePlayer;       //Does not move the player with the platform

    [Header("Platform Move Type")]
    [SerializeField] bool loopPlatform;         //Infinitely loops
    [SerializeField] bool comeBackPlatform;     //Gets to the end of the list & goes backwards to the start
    [SerializeField] bool randomPlatform;       //Randomly selects waypoints - Can not go back to back to waypoints


    Transform newParent;
    Transform playerTransform;
    int waypointIndex;
    int previousWaypointIndex = -1;
    bool isWaiting;
    bool isReversing;
    float initialYRotation;
    float targetYRotation;
    float rotationStartTime;
    float rotationDuration;

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
        if (waitTimes.Length != waypoints.Length || targetRotations.Length != waypoints.Length)
            Debug.LogError("The number of wait times and target rotations must match the number of waypoints.");
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
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);

        float moveFraction = speed * Time.deltaTime / distanceToWaypoint;

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        float distanceFraction = Mathf.Clamp01(moveFraction * (1f / distanceToWaypoint));
        float elapsedTime = Time.time - rotationStartTime;
        float lerpFactor = Mathf.Clamp01(elapsedTime / rotationDuration);
        float currentYRotation = Mathf.LerpAngle(initialYRotation, targetYRotation, lerpFactor);
        transform.eulerAngles = new Vector3(0, currentYRotation, 0);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            transform.eulerAngles = new Vector3(0, targetYRotation, 0);
            StartCoroutine(WaitAtWaypoint());
        }
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
        else if (comeBackPlatform)
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
        initialYRotation = transform.eulerAngles.y;
        targetYRotation = targetRotations[waypointIndex];
        rotationStartTime = Time.time;
        rotationDuration = Vector3.Distance(transform.position, waypoints[waypointIndex].position) / speed;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (dontMovePlayer)
                return;
            playerTransform = collision.transform;
            playerTransform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (dontMovePlayer)
                return;
            playerTransform.SetParent(null);
            playerTransform = null;
        }
    }
}
