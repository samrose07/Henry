/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * This is the Game Manager script. It manages the game. Let me show you how!
 * 
 * date: 01/10/22
 * 
 * biodigital jazz, man
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region variables
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Camera cam;
    private GameObject gameOb;
    [SerializeField] private float timeRemaining;
    [SerializeField] private bool isCountingDown = true;
    [SerializeField] private TMP_Text timerText;
    private string ttext = "";
    #endregion

    #region start and update
    private void Start()
    {
        //timerText = TMP_Text.FindObjectOfType<TMP_Text>(); //find the timer to display the time
        ttext = timeRemaining.ToString();   //set the timer's text value to the current time
        timerText.text = ttext;     //set the UI text to the timer text value
    }

    private void Update()
    {
        TimerBabe(); //make the timer happen
    }

    #endregion

    #region custom methods
    public void EditPlayer(string editHow) //a modular approach to changing the player. Set it up this way so I can add to it in the future. 
    {
        switch(editHow) //how we gonna do it? Changes based on what the designer sends with the call
        {
            case "HitBySpiderWeb":
                /*
                 * upon collision, set player speed to 2
                 * upon collision set player max jump velocity to 1.
                 * this will have the effect of slowing them down.
                 * Do this for a count of five seconds
                 */
                playerController.speed = 2f;
                playerController.maxJumpVelocity = 1;
                playerController.playerVelocity = Vector3.zero;
                Invoke("ResetPlayer", 3);
                break;
                //TODO: add more!
        }
    }

    private void ResetPlayer()
    {
        //print("Aye bruh i got here"); for testing
        playerController.speed = 10; //done being slowed? reset speed. 
        playerController.maxJumpVelocity = Mathf.Abs(playerController.gravity) * playerController.timeToJumpApex; //done being slowed? reset maximum jump height.
    }

    public void LoadNewScene(string s)
    {
        SceneManager.LoadScene(s); //new scene boyo
    }

    public void MoveCamAndLook(GameObject g, Transform camLoc)
    {
        if(cam != null) //camera exists
        {
            cam.transform.position = camLoc.position; //set the position of it to the given position from the call
            gameOb = g; //gameob is the gameobject we're looking at. Terrible and not telling name. CHANGE THIS
            cam.GetComponent<MouseLook>().enabled = false; //take away camera control
            cam.transform.LookAt(gameOb.transform); //look at the event
            
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 40, 1f); //shorten field of view for a more cinematic appeal
          
            Invoke("TakeCam", 0.03f); //when ready, take the cam
            Invoke("GiveCamBack", 3f); //when event is done (hard coded to 3 seconds), give camera back


            //StartCoroutine(Unpause(true));
            //Time.timeScale = 0;
        }
    }
    private void TakeCam()
    {
        //print("called");
        cam.GetComponent<MouseLook>().enabled = false; //take camera control (i called this above, but do it again juuuuust in case somethin funky happens)
        if (gameOb != null) cam.transform.LookAt(gameOb.transform); //if the event exists, look at the event (called above, but to it again in case of spooky action
        else GiveCamBack(); //otherwise give back control

    }
    private void GiveCamBack()
    {
        //print("called 2");
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90, 1f); //change field of view back to normal
        cam.GetComponent<MouseLook>().enabled = true; //give player control of the camera again
        
    }

    private void TimerBabe() //countdown
    {
        switch(isCountingDown) //not finished?
        {
            case true:
                if (timeRemaining > 0) //make sure its more than zero so we can
                {
                    timeRemaining -= Time.deltaTime; // ^ count down based on realtime, not weird framerules
                    if (timeRemaining < 10) //once we hit scary time left set the color of the timer to RED
                    {
                        timerText.color = Color.red;
                    }
                    ConvertToTime(timeRemaining); //format the string
                   // print(ttext);
                    timerText.text = ttext; //set the text
                    
                }
                else isCountingDown = false; //not counting down no mo
                break;
            case false:
                timeRemaining = 0; //no more time

                Cursor.lockState = CursorLockMode.None; //give player the mouse
                Cursor.visible = true;
                LoadNewScene("OutOfTime"); //load out of time scene TODO: let player restart level rather than whole game
               // ttext = "Game Over";
                //timerText.text = ttext;
                break;
        }
    }

    void ConvertToTime(float time)
    {
        //print("called");
        float minutes = Mathf.FloorToInt(time / 60); //get minutes from time number
        float seconds = Mathf.FloorToInt(time % 60); //get the modulus of time and 60, makes it act as the seconds.
        ttext = string.Format("{0:00}:{1:00}", minutes, seconds); //convert to 0:00 time-format
    }

    #endregion
}
