/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to handle the boss's rotation and shooting mechanics.
 * 
 * date: 02/04/22
 * 
 * biodigital jazz, man
 */
using UnityEngine;

public class BigSpiderBoiScript : MonoBehaviour
{
    #region variables
    [SerializeField] private GameObject thisBody;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private GameObject projectilePath;
    [SerializeField] private float projectileSpeed;
    [SerializeField] public bool canTrack;
    private bool didSpawn = false;
    [SerializeField] private bool shooting;
    [SerializeField] private float chargeTime;
    [SerializeField] private float maxChargeTime;
    [SerializeField] private float aimTime;
    [SerializeField] private Animator anim;

    #endregion

    #region update
    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
        CheckAnimation();
    }
    #endregion

    #region methods
    //checks to make sure it can track, then turns the body to look at the player.
    //Originally built for an enemy with a head on a swivel, rather than a spider.
    void TrackPlayer()
    {
        if(canTrack)
        {
            TurnBody();
            gameObject.transform.LookAt(player.transform);
           // chargeTime++;
           // if(chargeTime > aimTime) projectilePath.SetActive(true);
           // if (chargeTime < aimTime) projectilePath.SetActive(false);
        }
        //if (chargeTime >= maxChargeTime) ShootPlayer();
    }

    //Does things based on what animation is playing at any given time.
    void CheckAnimation()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            ResetShot(); //if idle, reset the shot
            return;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("tothreat") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("threatidle") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("spit") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            ShootPlayer(); //you spittin? best be shootin
        }
    }

    //takes a local direction of the player and converts it to a tangible angle in which the body of the spider rotates.
    //This is done so the x and z axis of rotation doesn't happen, and instead just rotates on the y axis to look at the player
    void TurnBody()
    {
        Vector3 lookDir = transform.InverseTransformPoint(player.transform.position);
        float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
        thisBody.transform.Rotate(0, angle, 0);
    }
    void ShootPlayer()
    {
       // canTrack = false; deprecated for non-usecase.
        if (!didSpawn)
        {
            GameObject proj = Instantiate(projectile, projectileSpawn, false); //get the new projectile goin
            Rigidbody rb = proj.AddComponent<Rigidbody>(); //give it a rigidbody
            rb.AddForce(gameObject.transform.forward * projectileSpeed, ForceMode.Impulse); //propel it forward
            proj.transform.parent = null; //take away the parenting so it doesn't rotate with the spider
            didSpawn = true; //no more shootin once shot was shot
            
        }
    }

    void ResetShot()
    {

        //after shooting, pick a new maxcharge time.
        //should be between 500 and 700
        //charge time should be 500 exactly away from max chargetime
        maxChargeTime = Random.Range(700, 1300);
        aimTime = maxChargeTime - 500;
        canTrack = true; //you can shoot again
        chargeTime = 0; //deprecated
        didSpawn = false;
        
    }
    /*
     * hi you're a cool person don't let anyone tell you different
     */
    #endregion
}
