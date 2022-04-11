/* This code was originally created for a personal project, but has been added to for the purposes
 * of multiple projects. This version is for the project titled "Henry," a third person platformer.
 * 
 * The purpose of this script is to make the player M O V E.
 * 
 * date: 10/05/2021
 * 
 * Biodigital jazz, man.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public CarlAudio audioScript;
    public GameObject menu;
    public Animator carlimator;
    public CharacterController playerController;
    public Transform cam;
    //public Camera camera;
    public float t;
    public float maxFOV;
    public float startFOV;
    public float speed;
    public float sprintSpeed;
    //private bool sprinting = false;
    public bool moving;
    public bool toggleSprint = true;
    public float mouseSensitivity = 100f;
    public float gravity = -9.14f;
    public float jumpHeight = 1.0f;
    private int jumpCount = 2;
    public float maxJumpHeight = 10f;
    public float minJumpHeight = 1f;
    public Vector3 playerVelocity;
    public float maxJumpVelocity;
    public float minJumpVelocity;
    public float timeToJumpApex = 0.4f;
    [SerializeField]public bool groundedChar;

    public GameObject groundPoundObject;
    public Transform gpObjSpawn;
    public float groundPoundLift = 10;
    private bool moveGroundPound = false;
    public float dashLength;
    //public float turnSmoothTime = 0.1f;
    //float turnSmoothVelocity;
    public Vector3 move;
    int numGroundPounds = 1;
    public bool isDiving = false;
    public bool diveKeyUp = false;
    public float divingSpeedAdjuster = 1f;
    private float divSpeedAdjDefault;
    public Vector3 diveTargetPos;
    public Vector3 diveTargetRot;
    public Vector3 diveTargetScale;
    private bool gPound = false;
    private bool jumping = false;
    private bool playedStep = false;
    #endregion

    #region Start and Update
    private void Start()
    {

        //we have to make sure all the defaults are applied upon starting, including minimum and maximum jump velocities and diving speeds
        menu = GameObject.Find("menu");
        menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        divSpeedAdjDefault = divingSpeedAdjuster;

    }
    // Update is called once per frame
    void Update()
    {
        
        DoGroundPound();                                                        //Ground pound happens whenever someone presses the shift key while airborn
        MoveCharacterXZ();                                                      //Get the 2D movement of the character (no jumps)
        MoveCharacterY();                                                       //Get that third dimension of movment (da jumps)
        DiveBro();                                                              //currently has no purpose other than to look flashy
        OpenMenu();                                                             //Pause function for the player
        if(isDiving)
        {
            divingSpeedAdjuster *= 1.2f;                                        //Make the character go faster when diving
        }

        //this code just calls for animations on the character.
        #region Animation editors Spaghetti                                     
        bool canJump = false;
        float move = 0;
        if (jumpCount > 0)
        {
            canJump = true;
        }
        if (moving) move = 1;
        if (jumping)
        {
            carlimator.SetFloat("jumpin", 1);
        }
        if (!jumping)
        {
            carlimator.SetFloat("jumpin", 0);
        }
        //MoveCharacterSprint();
        if (speed >= 10)
        {
            carlimator.SetFloat("Speed", 1f);
        }
        else
        {
            carlimator.SetFloat("Speed", .1f);
        }

        carlimator.SetBool("poundPressed", gPound);
        carlimator.SetBool("grndd", groundedChar);
        carlimator.SetBool("canJump", canJump);
        carlimator.SetFloat("movin", move);

        #endregion                                             

        
    }

    #endregion


    #region Custom Methods

    //get the movement of the character on the x and z axis (2D) and apply that to the character controller.
    //side note: when editing this for my platformer i realized too late that what I wanted to do with it would have been
    //easier and prettier if I used rigidbody instead of Character controller. But we still made it happen!
    void MoveCharacterXZ()
    {
        groundedChar = playerController.isGrounded;                             //Double checking to make sure you're grounded
        if (groundedChar)                                                       //resets jump and ground pount count to original. FUTURE: Make the amount of jumps editable for powerup purposes?
        {
          jumpCount = 2;
          numGroundPounds = 1;
            if (!playedStep)
            {
                audioScript.PlayStep();
                playedStep = true;
            }


        }

        float x = Input.GetAxis("Horizontal");                                  //these two get inputs from the axes - WASD for PC or left joystick for controllers
        float z = Input.GetAxis("Vertical");                                    
        if (x != 0 || z != 0) moving = true;                                    //if either of the axes are not zero we are considered moving. Used by the MouseLook class for stationary vs moving camera controls
        else moving = false;
        move = transform.right * x + transform.forward * z;                     //the vector3 to use by the CharacterController's move function.
        playerController.Move(move * speed * Time.deltaTime);                   //transforms the gameobject with the character controller based on speed (arbitrary value) and time. Smooth like buttah.
    }

    //Handle the jumps. When jump is pressed do the things. Some super totally fancy math
    //I will say though, the jump height is variable based on how long you hold the jump
    //button and it feels super nice
    void MoveCharacterY()
    {
        if (jumpCount > 0)                                                      // Got jumps? 0.o
        {
            if (!groundedChar && Input.GetButtonDown("Jump"))                   //you in the air and HOLDING jump?
            {
                playedStep = false;
                playerVelocity.y = Mathf.Sqrt(maxJumpVelocity * -3.0f * gravity); //gimme that good high jump
                audioScript.PlayJump();                                         //plays the jumpin audio
                jumping = true;
            }
            if (Input.GetButtonUp("Jump"))                                      //You no longer holding jump?
            {
                if (playerVelocity.y > minJumpVelocity)                         //Your velocity is now the minimum, stopping your height gain and allowing gravity to catch up
                {
                    playerVelocity.y = minJumpVelocity;
                }
                jumpCount--;                                                    //you got one less jumps man
            }
        }
        if (Input.GetButtonDown("Jump") && groundedChar)                        //basically what happens above, just uh, if you're grounded and then you don't need to subtract the jump count again!
        {
            playedStep = false;
            playerVelocity.y = Mathf.Sqrt(maxJumpVelocity * -3.0f * gravity);
            audioScript.PlayJump();                                             //plays the jumpin audio
            jumping = true;
        }
        if (Input.GetButtonUp("Jump"))                                          // ^
        {
            if (playerVelocity.y > minJumpVelocity)
            {
                playerVelocity.y = minJumpVelocity;
            }
            jumping = false;
        }
        //Debug.Log("Jumpcount is: " + jumpCount);
        playerVelocity.y += gravity * Time.deltaTime;                           //the player now falls with gravity smoothly
        if (playerVelocity.y <= -60) playerVelocity.y = -60;                    // cap the minimum velocity. The issue was that your velocity would continuously fall until you jumped.
                                                                                //Making it so the velocity was zero'd out when grounded would cause buggy jumps as it takes a few frames
                                                                                //for the character controller to register that it is not grounded anymore. Had to cap it at an 
                                                                                //arbitrary value to make it work a bit.
                                                                                //TEST THIS: figure out a good feeling max downward velocity and make it that instead of an arbitrary value like
                                                                                //60
        
        playerController.Move(playerVelocity * Time.deltaTime);                 //move the char.
        //end jump
    }

    //basically another miniature jump but you also shoot out a little ball of air to activate things too!
    //Combining this with both maximum jumps gives you great height and great distance.
    void DoGroundPound()
    {
        if(!groundedChar && numGroundPounds > 0)                                //airborn and got ground pound? O.o
        {
            if (Input.GetButtonDown("Woosh"))                              //tryna ground?
            {

                //spawn a ground pound object below you.
                GameObject instance = Instantiate(groundPoundObject, gpObjSpawn.position, gpObjSpawn.rotation);
                groundPoundObject.transform.position = gpObjSpawn.position;     //set its position to the correct area.
                //Instantiate(groundPoundObject);
                //leaving these in for future references in case i need to look at them
                //Rigidbody rb = groundPoundObject.GetComponent<Rigidbody>();
                //Debug.Log(rb.velocity.y);
                instance.GetComponent<Rigidbody>().AddForce(-transform.up * 1000);//add downward force to the game object
                playerVelocity.y = groundPoundLift;                               // add upward force to the player
                numGroundPounds = 0;                                              //ain't got no ground pounds left bro
                gPound = true;                                                    //made sure that it doesn't groundpound more than once. thinking about it, this is not needed.
                audioScript.PlayWoosh();                                          //plays the woosh audio
                Invoke("NoLongerGroundPound", .5f);                               // give it time and then allow it later.
            }
        }
    }

    void NoLongerGroundPound()
    {
        gPound = false;
    }

    //a flashy move that was planned to have more implementation, but does not.
    void DiveBro()                                  
    {
        if (!groundedChar && Input.GetButton("Dive"))                        //airborn and pressin alt? (not available on game pads)
        {
            //this can be used to hover a bit if needed!     playerVelocity.y *= .9f;
            playerVelocity.y -= (0.1f + divingSpeedAdjuster);                      //you're gonna fall real fast if you're diving.
            isDiving = true;
        }
        else
        {
            if(Input.GetKeyUp(KeyCode.LeftAlt))                                    // no longer diving, cancel it out. The booleans in this function are used by the camera to update the camera position, giving a greater sense of speed
            {
                diveKeyUp = true;
                if (groundedChar) diveKeyUp = false;
            }
            divingSpeedAdjuster = divSpeedAdjDefault;                              //reset to default
            isDiving = false;
        }

    }

    //show or hide the menu. The GAMEMANAGER in the scene checks to see if the menu is active and adjusts game time
    //based on that. So all I have to do here is activate it.
    void OpenMenu()
    {
        if (Input.GetButtonDown("OpenMenu"))
        {
            if (!menu.activeInHierarchy)
            {
                menu.GetComponent<MenuScript>().ShowMenu();
            }
            else if (menu.activeInHierarchy)
            {
                menu.GetComponent<MenuScript>().HideMenu();
            }
        }
    }

    #endregion

    #region Previous spaghetti that wasn't used for this project

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if(playerController.isGrounded && hit.transform.tag == "movingPlatform")
    //    {
    //        playerController.Move(hit.gameObject.transform.position * Time.deltaTime);
    //    }
    //}

    /* yendere dev
    void MoveCharacterSprint()
    {
        //sprint
        //if player chooses to toggle (default) sprint
        if (toggleSprint)
        {
            if (groundedChar && Input.GetButtonDown("Sprint"))
            {
                Sprint();

            }
        }
        //if player chooses to hold sprint (toggle by default)
        if (!toggleSprint)
        {
            if (groundedChar && Input.GetButton("Sprint"))
            {
                Sprint();
            }
        }
        if (sprinting && Input.GetAxis("Horizontal") != 0)
        {
            speed /= sprintSpeed;
            sprinting = false;
            Camera.main.fieldOfView -= 5 * Time.deltaTime;
        }
        //print("horizontal axis" + Input.GetAxis("Horizontal"));
        if (sprinting)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, maxFOV, t);
        }
        if (!sprinting)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, startFOV, t);
        }
    }
    void Sprint()
    {
        if (!sprinting)
        {
            speed *= sprintSpeed;
            sprinting = true;
            Camera.main.fieldOfView += 5 * Time.deltaTime;
        }
        else
        {
            speed /= sprintSpeed;
            sprinting = false;
            Camera.main.fieldOfView -= 5 * Time.deltaTime;
        }

    }
    */

    #endregion
}
