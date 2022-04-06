/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to handle menu interaction and events called by buttons
 * 
 * date: 03/18/22
 * 
 * biodigital jazz, man
 */
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        //print("hither");

        Application.Quit();
    }

    public void LoadScene(string n)
    {
        //print("got hither");
        SceneManager.LoadScene(n); //load the given scene
    }
    
    //in order, show the menu, give the player the cursor, make the game go reeeeeeal slow
    public void ShowMenu()
    {
        menu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.01f;
        
    }

    //in order, hide the menu, resume the game speed, hide and take away the cursor
    public void HideMenu()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
