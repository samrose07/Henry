/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to handle the player when they fall off the map.
 * 
 * date: 03/05/22
 * 
 * biodigital jazz, man
 */
using UnityEngine;

public class Fallbox : MonoBehaviour
{
    #region variables
    [SerializeField] private Transform whereTo;
    [SerializeField] private GameObject player;
    #endregion

    #region fell into trigger event
    private void OnTriggerEnter(Collider other)
    {
        print("i got here");
        if(other.gameObject.name == "ThirdPersonController") //is it the player who is triggering this? just in case
        {
            player.SetActive(false);    //"despawn" the player
            player.transform.position = whereTo.position; //set the new position
            //print("i also got here"); was a test
            player.SetActive(true); //"respawn" the player

            //COULD ADD:
            //make it happen visually so it's not so jarring for the player
        }
    }
    #endregion
}
