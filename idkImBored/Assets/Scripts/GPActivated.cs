/* This code was created by Samuel Rose for a project at Boise State University
 * titled "Henry."
 * 
 * The purpose of this script is to act as a modular activation wherein the game designer
 * (me) can choose what effect a button will have in the game world.
 * 
 * date: 01/31/2022
 * 
 * Biodigital jazz, man.
 * 
 */

using UnityEngine;

public class GPActivated : MonoBehaviour
{
    #region variables
    [Header("Ground pound area to connect to:")]
    [Tooltip("The ground pound area script. Make sure it's area, not obj")]
    [SerializeField] private GroundPoundArea gpa;
    private bool GPCanDoFx = false;
    
    [Header("Function to do when activated:")]
    [Tooltip("Type in one of these to activate a function:\n" +
        "MoveDirection\n" +
        "Disappear\n" +
        "LaunchDown\n" +
        "ButtonIndicator")]
    [SerializeField] private string Function;
    private bool didLaunch = false;
    private float step;

    [Tooltip("Use this if using the MOVEDIRECTION function.")]
    [SerializeField]private Vector3 positionToMoveTo;
    [Tooltip("Use this if launching at an angle")]
    [SerializeField] private Transform AreaToLaunchTo;
    [SerializeField] private GameManager gm;
    [SerializeField] private string GOToTrackWithCam;
    [SerializeField] private Transform areaToMoveCamTo;
    #endregion

    #region update
    private void Update()
    {
        //don't need to multiply Time.deltaTime by 1, but it looks pretty i think
        step = 1.0f * Time.deltaTime;

        //am I allowed to do the function the designer set for me? Let's check with the GroundPoundArea
        GPCanDoFx = gpa.canDo;

        //here we are going to call the function that the designer asks for in the inspector.
        if(GPCanDoFx)
        {
            switch(Function)
            {
                case "MoveDirection":
                    MoveDirection();
                    break;
                case "Disappear":
                    Disappear();
                    break;
                case "LaunchDown":
                    LaunchDown();
                    break;
                case "ButtonIndicator":
                    ButtonIndicator();
                    break;
            }
        }
    }
    #endregion

    #region User Created Methods
    //move towards a point set by the designer in inspector. Super simple.
    private void MoveDirection()
    {
        if (gameObject.transform.position != positionToMoveTo)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToMoveTo, step); //move to a location from current location in step time
        }
    }

    //originally, I wanted to have the object look at a point through rotation to launch towards,
    //and that is in there just commented out. This function sends the object towards a point using
    //a rigidbody and force
    private void LaunchDown()
    {
        if(AreaToLaunchTo != null)
        {
            Vector3 lookDir = transform.InverseTransformPoint(AreaToLaunchTo.position); //put the location of world space into local space 
            float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;            //convert the inversetransformpoint into a direction and then into degrees to be modified in the transform.
        }
        //print("called 3");
        //thisBody.transform.Rotate(0, angle, 0);
        if (gm != null) gm.MoveCamAndLook(gameObject, areaToMoveCamTo);                 //calls the function to make the camera view an event based on a given location
        Invoke("Disappear", 3f);                                                        // make the object go away visually hehe
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();                            //need access to the rigidbody to allow gravity
        rb.useGravity = true;
        if(!didLaunch)                                                                  //if it hasn't happened yet, make sure it does
        {
            if (AreaToLaunchTo != null) rb.AddForce((AreaToLaunchTo.position - transform.position), ForceMode.Impulse); //check to see if where to go is not null, and if so go there
            else rb.AddForce(new Vector3(0, -50f, 0), ForceMode.Impulse);                                               //otherwise just go straight down

            //rb.AddForce(new Vector3(0,-50f,0), ForceMode.Impulse);
            didLaunch = true;                                                           //can't do it no mo
        }
    }

    //it does this
    private void Disappear()
    {
        gameObject.SetActive(false);
    }

    private void ButtonIndicator()
    {
        Color c = gameObject.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().material.color = new Color(c.r, 255, c.b);
    }
    #endregion
}
