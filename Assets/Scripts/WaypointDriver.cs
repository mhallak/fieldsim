using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDriver : MonoBehaviour
{
    public GameObject waypointsParent;
    public float velocity = 10;
    RandomVelocity randvel;

    GameObject currentlyFollowedWaypoint;
    int currentlyFollowedIndex = 0;

    void getNextWaypointFromParent()
    {
        if (currentlyFollowedIndex >= waypointsParent.transform.childCount) this.enabled = false;

        currentlyFollowedWaypoint = waypointsParent.transform.GetChild(currentlyFollowedIndex).gameObject;
        currentlyFollowedIndex++;
    }
    void Start()
    {
        if (waypointsParent == null)
        {
            this.enabled = false;
            Debug.LogWarning("WaypointDriver:: no waypoints given, script disabled");
        }

        getNextWaypointFromParent();

    }
    void FixedUpdate()
    {
        if(!currentlyFollowedWaypoint.active) getNextWaypointFromParent();
        
        Vector3 direction = (currentlyFollowedWaypoint.transform.position - transform.position).normalized;
        
        // remember gravity since velocity is hard coded
        float gravityVelocityTemp =  this.GetComponent<Rigidbody>().velocity.y;
        Vector3 velocityToApply = direction * velocity;
        velocityToApply.y = gravityVelocityTemp;

        this.GetComponent<Rigidbody>().velocity = velocityToApply;
        transform.LookAt(currentlyFollowedWaypoint.transform);

    }
}
