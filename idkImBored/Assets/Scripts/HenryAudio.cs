/* This script was created for the project "Henry" at Boise State by
 * Samuel Rose
 * 
 * The purpose of this script is to handle the boss's audio! Each method is called by an animation event from the boss itself
 * 
 * date: 04/06/22
 * 
 * biodigital jazz, man
 */
using UnityEngine.Audio;
using UnityEngine;

public class HenryAudio : MonoBehaviour
{
    #region variables
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip ViolinClip;
    [SerializeField] private AudioClip SpitClip;
    #endregion

    #region callable methods
    public void PlaySpit()
    {
        source.PlayOneShot(SpitClip);
    }

    public void PlayViolin()
    {
        source.PlayOneShot(ViolinClip);
    }
    #endregion
}
