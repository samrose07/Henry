/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to handle the boss's projectile's events
 * 
 * date: 02/04/22
 * 
 * biodigital jazz, man
 */
using UnityEngine;

public class SpiderProjectile : MonoBehaviour
{
    /*
     * upon collision, set player speed to 2
     * upon collision set player max jump velocity to 1.
     * this will have the effect of slowing them down.
     * Do this for a count of five seconds
     */
    GameManager gm;

    #region awake and trigger event
    private void Awake()
    {
        gm = GameObject.Find("GAMEMANAGER").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);    //send that thing to the shadow realm
        CheckCollision(other); 
    }
    #endregion

    #region custom methods
    private void CheckCollision(Collider c) //we're making sure the player character is what we interacted with. Could have done this in the trigger event, but i wanted to make it look prettier
    {
        string colName = c.gameObject.name;
        if(colName == "ThirdPersonController")
        {
            gm.EditPlayer("HitBySpiderWeb"); //if so, tell the GM to do the ting
        }
    }
    #endregion
}
