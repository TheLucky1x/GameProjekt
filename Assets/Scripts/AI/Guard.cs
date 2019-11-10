using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{

  public float speed = 5f; // Move speed
  public float waitTime = .3f; // Time to wait when he reached a waypoint
  public float turnSpeed = 90; // the amount of degrees he can rotate in a second

  public Transform pathHolder; // The object that has all of the different waypoints

  void Start() {
    Vector3[] waypoints = new Vector3[pathHolder.childCount];
    for (int i = 0; i < waypoints.Length; i ++) /* Going through all the different waypoints */ {
      waypoints[i] = pathHolder.GetChild (i).position; // Get the different positions of the waypoints
      waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints [i].z); // Make a new Vector3 for each waypoint
    }

    StartCoroutine(FollowPath(waypoints)); // Starting the "Function" or Coroutine for following the path
  }

  // Creating the Function for following the path
  IEnumerator FollowPath(Vector3[] waypoints) /* set the name of the function and create a new Vector3 */ {
    transform.position = waypoints [0];

    int targetWaypointIndex = 1;
    Vector3 targetWaypoint = waypoints [targetWaypointIndex];
    transform.LookAt(targetWaypoint);

    while(true) {
      transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
      if (transform.position == targetWaypoint) {
        targetWaypointIndex = (targetWaypointIndex+1) % waypoints.Length; // Make him start the path all over again
        targetWaypoint = waypoints [targetWaypointIndex]; // Set the target waypoint
        yield return new WaitForSeconds(waitTime); // if reached the target waypoint wait for x seconds
        yield return StartCoroutine(TurnToFace (targetWaypoint)); // Start the Coroutine for looking in the right direction
      }
      yield return null;
    }
  }

  // Creating the Function for looking in the right direction
  IEnumerator TurnToFace(Vector3 lookTarget) {
    Vector3 dirToLookTarget = (lookTarget-transform.position).normalized;
    float targetAngle = 90-Mathf.Atan2(dirToLookTarget.z,dirToLookTarget.x) * Mathf.Rad2Deg;

    while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)  {
      float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetAngle,turnSpeed * Time.deltaTime);
      transform.eulerAngles = Vector3.up * angle;
      yield return null;
    }
  }

  void OnDrawGizmos() /* For debug use, for better readability in the editor, only if Gizmos is activated */ {
    Vector3 startPosition = pathHolder.GetChild(0).position;
    Vector3 previousPosition = startPosition;
    foreach (Transform waypoint in pathHolder) {
      Gizmos.DrawSphere(waypoint.position, 0.3f); // Draw a sphere at each waypoint
      Gizmos.DrawLine(previousPosition,waypoint.position); // Draw a line between 2 waypoints
      previousPosition = waypoint.position;
    }
    Gizmos.DrawLine(previousPosition,startPosition); // Draw a line between the first and the last waypoint
  }
}
