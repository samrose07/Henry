/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to handle the boss's health and events
 * 
 * date: 02/04/22
 * 
 * biodigital jazz, man
 */
using System.Collections;
using UnityEngine;

public class Parasite : MonoBehaviour
{
    #region variables
    [SerializeField] private int Health;
    [SerializeField] private Material mat;
    #endregion

    #region event and enumerators
    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("SpikeyBoi")) //was it a spike?
        {
            Health--;   //lives - 1
            Color c = gameObject.GetComponent<Renderer>().material.color;   //get the color from the material attached to the parasite
            gameObject.GetComponent<Renderer>().material.color = new Color(255, c.g, c.b);  //set it to red
            StartCoroutine(ChangeColorBack(c)); //once it is set to red, change it back to original after .5 seconds
            StartCoroutine(DestroySpike(other.gameObject, 3f)); //once happened, destroy the spike in 3 seconds
        }
    }
    IEnumerator ChangeColorBack(Color color)
    {
        yield return new WaitForSeconds(.5f); //wait until, and then do the thing
        gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b);  //change color back to original color
    }

    IEnumerator DestroySpike(GameObject g, float delayTime)
    {
        yield return new WaitForSeconds(delayTime); //wait until, then do the ting
        Destroy(g); //get rid of that spike
        if(Health <= 0) //is the parasite dead? then tell the manager as such
        {
            GameManager gm = GameObject.Find("GAMEMANAGER").GetComponent<GameManager>();
            gm.LoadNewScene("EndGame");
        }
    }
    #endregion
}
