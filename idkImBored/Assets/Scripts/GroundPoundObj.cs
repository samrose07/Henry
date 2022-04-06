/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to make the ground pound object despawn upon hit
 * 
 * date: 01/31/22
 * 
 * biodigital jazz, man
 */
using UnityEngine;

public class GroundPoundObj : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        Destroy(gameObject);
        //Debug.Log("Collider is " + collision.gameObject.tag);
    }
}
