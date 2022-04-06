/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to handle the loading of the second level. Could be made modular in the future easily.
 * 
 * date: 02/13/22
 * 
 * biodigital jazz, man
 */
using UnityEngine;

public class LoadShift : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "ThirdPersonController") //was it the player who triggered? then do the ting
        {
            GameManager gm = GameObject.Find("GAMEMANAGER").GetComponent<GameManager>(); //find the game manager and tell it to load the next scenes
            gm.LoadNewScene("ArachnidBoss");
        }
    }
}
