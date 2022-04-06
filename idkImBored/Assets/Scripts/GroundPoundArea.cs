/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to handle a collision on the buttons to activate an effect
 * 
 * date: 01/31/22
 * 
 * biodigital jazz, man
 */
using UnityEngine;

public class GroundPoundArea : MonoBehaviour
{
    #region variables
    public GameManager gm;
    [HideInInspector] public bool canDo = false;
    #endregion

    #region collision handler
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "gpOBJ") //was it a groundpound object? then do the tiiiiing
        {
            //gm.ObjectHitWithGroundPound(collision.gameObject);
            canDo = true; //set the public variable to true so that linked object can act upon it
        }
    }
    #endregion
}
