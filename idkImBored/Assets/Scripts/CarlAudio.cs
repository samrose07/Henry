/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to handle the player's audio effects (jump, land, ground pound)
 * Each method is called by either another script or an event triggered by an animation.
 * 
 * date: 04/06/22
 * 
 * biodigital jazz, man
 */
using UnityEngine;
using UnityEngine.Audio;

public class CarlAudio : MonoBehaviour
{
    #region variables
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip wooshSound;
    [SerializeField] private AudioClip footstepSound;
    private float pitch = 1;
    private float pitchLow = 0.8f;
    private float pitchHigh = 1.2f;
    #endregion
    #region callable methods
    public void PlayStep()
    {
        pitch = GetRandomPitch(); //getting a new pitch to add variation in this one sound. Make it seem like it's a new sound when really it's not!
        source.pitch = pitch;   //set the pitch
        source.PlayOneShot(footstepSound); //play the sound
    }

    //for what these methods do, see PlayStep()
    public void PlayJump()
    {
        pitch = GetRandomPitch();
        source.pitch = pitch;
        source.PlayOneShot(jumpSound);
    }

    public void PlayWoosh()
    {
        pitch = GetRandomPitch();
        source.pitch = pitch;
        source.PlayOneShot(wooshSound);
    }

    //gets a new pitch float value and returns it when asked
    private float GetRandomPitch()
    {
        return Random.Range(pitchLow, pitchHigh);
    }
    #endregion
}
