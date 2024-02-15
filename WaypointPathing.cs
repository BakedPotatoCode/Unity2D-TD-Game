using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPathing : MonoBehaviour
{
    public string waypointTag = "Waypoint"; // Tag used for waypoints in the scene
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f; // Speed of rotation towards the next waypoint

    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        // Find waypoints in the scene with the specified tag
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag(waypointTag);
        waypoints = new List<Transform>(waypointObjects.Length);

        foreach (GameObject waypointObject in waypointObjects)
        {
            waypoints.Add(waypointObject.transform);
        }

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("No waypoints found with tag: " + waypointTag);
            enabled = false;
        }
 
    }

    void Update()
    {
        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            // Get the current waypoint
            Transform targetWaypoint = waypoints[currentWaypointIndex];

            // Calculate the direction to the waypoint
            Vector3 direction = targetWaypoint.position - transform.position;
            direction.Normalize();

            // Rotate towards the waypoint
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move towards the waypoint
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

            // Check if the object is close enough to the waypoint to switch to the next one
            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
            {
                // Move to the next waypoint
                currentWaypointIndex++;

                // Check if it's the last waypoint
                if (currentWaypointIndex == waypoints.Count)
                {
                    // Stop updating position and rotation
                    enabled = false;
                }
            }
        }
    }
}
