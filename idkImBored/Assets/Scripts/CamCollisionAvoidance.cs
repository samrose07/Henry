/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to make sure the camera doesn't go through objects.
 * It has some smoothing out to do, but it does work!
 * 
 * date: february 2022
 * 
 * biodigital jazz, man
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCollisionAvoidance : MonoBehaviour
{
    #region variables
    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    Vector3 dollyDirection;
    public Vector3 dollyDirectionAdjusted;
    public float distance;
    public Transform camTransform;
    #endregion

    #region Awake and update
    private void Awake()
    {
        dollyDirection = transform.localPosition.normalized; //get the direction we are looking
        distance = transform.localPosition.magnitude;   //get how far away we start from the player
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredCamPos = transform.TransformPoint(dollyDirection * maxDistance);  //have to get location in worldspace so we can maybe go there
        RaycastHit hit; //init a hit point

        if(Physics.Linecast(transform.parent.position, desiredCamPos, out hit)) //send out a ray to the player. If anything crosses that ray, we are going to move the camera forward
        {
            distance = Mathf.Clamp(hit.distance * 0.3f, minDistance, maxDistance); //here we have a minimum distance to say, "ayo, be here AT LEAST." and a maximum to say "ayo, no further than this."
                                                                                   //we clamp the distance between the object which hit the ray and the minimum and maximum distance.
            
        }
        else
        {
            distance = maxDistance; //nothin hittin? do the MAX
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirection * distance, Time.deltaTime * smooth); //make sure the camera transitions smoothly between points.
    }
    #endregion
}
