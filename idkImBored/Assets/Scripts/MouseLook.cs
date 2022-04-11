/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to handle the player's 3rd person camera movement.
 *Could have used cinemachine, opted not to. 
 * 
 * date: 10/05/21
 * 
 * biodigital jazz, man
 */
using UnityEngine;


public class MouseLook : MonoBehaviour
{
    #region variables
    [Tooltip("Speed at which the camera rotates around the player. (Sensitivity, as it's named in most games)")]
    public float rotSpeed = 1;

    [Tooltip("Speed at which the player Lerps rotation to camera forward")]
    public float time = 0.5f;

    public GameManager gm;
    public Transform target, player;
    [Tooltip("Player Controller attached to the dude")]
    public PlayerController playerController;
    bool moving;
    float mouseX, mouseY;
    Vector3 initCamPos = Vector3.zero;
    Quaternion initCamRot;
    public Vector3 diveCamPos;
    public GameObject diveCamTarget;
    public Vector3 diveCamRot;
    public float step;
    bool isDive = false;
    public bool canControlCam = true;
    bool didDive = false;
    #endregion

    #region start and late update
    private void Start()
    {
        Cursor.visible = false;                   //take away the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        CamControl();
        moving = playerController.moving;   //check to see if we're moving to change camera behavior
        isDive = playerController.isDiving; //are we diving?
        
        if(isDive) //if we are, grab the camera collision script and turn it off. That way no spooky action
        {
            CamCollisionAvoidance camCollisionScript = GetComponent<CamCollisionAvoidance>();
            camCollisionScript.enabled = false;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.5f, 6.23f, -1.51f), step); //hard coded values based on a website interactable XYZW calculator
            canControlCam = false;  //take away controll
            didDive = true;     //yea, we did the dive
        }
        if(playerController.groundedChar && didDive) //if we are now grounded and we did the dive, reset the camera position
        {
            PostLandingCameraPosition();
        }
    }

    #endregion

    #region camera control methods
    void PostLandingCameraPosition()
    {
        if(didDive) //making sure we did the dive [this is redundant, as the function won't be called until didDive is true (see lateUpdate) but i'm going to leave it in for now as it's not pressing]
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 3.27f, -5.1f), step); //set the localposition to the original (hard coded as I got it from the inspector during testing. CHANGE THIS
            canControlCam = true;   //give back camera controll
            didDive = false;    //reset dive boolean so we can do the whole thing again
            CamCollisionAvoidance camCollisionScript = GetComponent<CamCollisionAvoidance>();
            camCollisionScript.enabled = true;  //re-enable camera collision checking
        }
    }

    void CamControl() //the meat and potatoes
    {
        transform.LookAt(target); //make sure we're looking at the target. TODO: make it lag a little bit!
        if (canControlCam)  //may i?
        {
            mouseX += Input.GetAxis("Mouse X") * rotSpeed;  //get the x and y values of the mouse, rotSpeed is sensitivity. TODO: change this to work with either mouse or gamepad
            mouseY -= Input.GetAxis("Mouse Y") * rotSpeed;  
            mouseY = Mathf.Clamp(mouseY, -60, 50);  //clamp the mouse so it doesn't freak out.
            

            if (!moving)    //if we are stationary, allow movement around the player.
            {
                target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            }
            else //if we're moving, make the player rotate with the camera. Feels much more natural
            {
                target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
                player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, mouseX, 0), time);
                //player.rotation = Quaternion.Euler(0, mouseX, 0);
            }
        }
    }
    #endregion
}
