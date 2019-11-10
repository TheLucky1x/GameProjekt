using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{

  public float speed = 5f; // Move speed
  public float waitTime = .3f; // Time to wait when he reached a waypoint
  public float turnSpeed = 90; // the amount of degrees he can rotate in a second

  public Light spotlight; // The flashlight
  public float viewDistance; // How far he is able to "see"
  public LayerMask viewMask; // The mask to check if an object is in line of sight
  float viewAngle; // The angle/fow he has, this is set by the spotAngle of the spotlight

  public Transform pathHolder; // The object that has all of the different waypoints
  Transform player;
  Color originalSpotlightColor; // The originalSpotlightColor

  void Start() {

    player = GameObject.FindGameObjectWithTag("Player").transform; // Find and set the object with the "player" tag as the variable player
    viewAngle = spotlight.spotAngle; // Set the viewAngle of the Guard
    originalSpotlightColor = spotlight.color; // Set the originalSpotlightColor

    Vector3[] waypoints = new Vector3[pathHolder.childCount];
    for (int i = 0; i < waypoints.Length; i ++) /* Going through all the different waypoints */ {
      waypoints[i] = pathHolder.GetChild (i).position; // Get the different positions of the waypoints
      waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints [i].z); // Make a new Vector3 for each waypoint
    }

    StartCoroutine(FollowPath(waypoints)); // Starting the "Function" or Coroutine for following the path
  }

  void Update() {
    if (CanSeePlayer()) {
      spotlight.color = Color.red; // if we can see the player, set the color to red
    }
    else {
      spotlight.color = originalSpotlightColor; // Set the color back to the original color
    }
  }

  bool CanSeePlayer() {
    if (Vector3.Distance(transform.position,player.position) < viewDistance) /* Checking if the player is in viewDistance or not */ {
      Vector3 dirToPlayer = (player.position - transform.position).normalized; // Get the direction to the player
      float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward,dirToPlayer); // Get the smallest angle between guard and player
      if (angleBetweenGuardAndPlayer < viewAngle / 2f) /* checking if the angle between the guard and the player is smaller then the viewAngle/2 */ {
        if (!Physics.Linecast(transform.position,player.position,viewMask)) /* checking if we hit anything except the player, if not do the next thing */ {
          return true; // when everything is correct, is he we able to see the player
        }
      }
    }
    return false; // if something is in the way, the player isnt in the view distance or if the player isnt in the viewAngle then we cant see him
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

    Gizmos.color = Color.red;
    Gizmos.DrawRay(transform.position,transform.forward * viewDistance);
  }
}
