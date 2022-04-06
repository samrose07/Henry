using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShift : MonoBehaviour
{
    public Transform[] locations;
    public int StartLocationToTravelTo;
    public int CurrentLocationToTravelTo;
    [SerializeField] private float speedMult;
    private float step;
    [SerializeField] private float stepMultiplier;
    private bool goingBackwards = false;
    private GameObject player;
    private void Start()
    {
        player = GameObject.Find("ThirdPersonController");
        CurrentLocationToTravelTo = StartLocationToTravelTo;
    }
    private void FixedUpdate()
    {
        step = stepMultiplier * Time.deltaTime;
        DoTheMove();
    }

    private void DoTheMove()
    {
        Transform locToGo;
        locToGo = locations[CurrentLocationToTravelTo];
        float distance = Vector3.Distance(transform.position, locToGo.transform.position);
        if (distance <= 0.5f)
        {
            if (locToGo == locations[locations.Length - 1])
            {
                goingBackwards = true;
            }
            else if (locToGo == locations[0]) goingBackwards = false;
            if (goingBackwards)
            {
                locToGo = locations[CurrentLocationToTravelTo - 1];
                CurrentLocationToTravelTo--;
                //for(int i = 0; i < locations.Length; i++)
                //{
                //    if (locations[i] == locToGo) CurrentLocationToTravelTo = i;
                //}              
            }
            if (!goingBackwards)
            {
                locToGo = locations[CurrentLocationToTravelTo + 1];
                CurrentLocationToTravelTo++;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, locToGo.position, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.transform.parent = gameObject.transform;
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.speed /= speedMult;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.speed *= speedMult;
            
        }
    }
}
