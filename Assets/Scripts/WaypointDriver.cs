using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bumdata{
    public bool isVisible;
    public Vector3 velocity;
}

public class WaypointDriver : MonoBehaviour
{
    public GameObject waypointsParent;
    public float velocity = 10;
    public Camera cam;    

    RandomVelocity randvel;

    GameObject currentlyFollowedWaypoint;
    int currentlyFollowedIndex = 0;
    
    // --- Bums detection vars
        public GameObject bumsParent;
        public Bumdata[] walkers = new Bumdata[5];

    // ---

    void getNextWaypointFromParent()
    {
        if (currentlyFollowedIndex >= waypointsParent.transform.childCount) this.enabled = false;

        currentlyFollowedWaypoint = waypointsParent.transform.GetChild(currentlyFollowedIndex).gameObject;
        currentlyFollowedIndex++;
    }

    private bool frustrumCameraCheck(GameObject Object) {

      Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

      if (GeometryUtility.TestPlanesAABB(planes , Object.GetComponent<Collider>().bounds))
          return true;

      else
          return false;
    }

     void PrintVisibleBums(){

        for (int idx = 0; idx < bumsParent.transform.childCount; idx ++){

            GameObject childObj = bumsParent.transform.GetChild(idx).gameObject;
            
            // frustrum AND visibilty check to see if bums are visible
            if(childObj.GetComponent<Renderer>().isVisible && frustrumCameraCheck(childObj)) {
                Debug.Log("Car:: spotted walker " +childObj.name);
                walkers[idx].isVisible = true;
                walkers[idx].velocity = childObj.GetComponent<Rigidbody>().velocity;
            }

            else{
                walkers[idx].isVisible = false;
                walkers[idx].velocity = new Vector3(); // 0,0,0 vel if invisible
            }
            
        }
    } 

    // when data gets edited from the outsider, call this
    public void Refresh(){
        getNextWaypointFromParent();
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

   

    private void Update() {
        
        PrintVisibleBums();
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
