/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to play a clip and then allow the base audioclip in the source to loop
 * 
 * date: 04/06/22
 * 
 * biodigital jazz, man
 */
using UnityEngine;
using UnityEngine.Audio;
public class MusicLoop : MonoBehaviour
{
    #region variables
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip IntroClip;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        source.PlayOneShot(IntroClip);  //start the introductory clip
        source.PlayScheduled(AudioSettings.dspTime + IntroClip.length); //start the loop clip once the first clip ends
    }
}
