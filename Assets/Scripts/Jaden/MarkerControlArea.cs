using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerControlArea : MonoBehaviour
{

    public string marker;
    public NewPlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<NewPlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(marker == "SpawnMarker")
            {
                player.SpawnMarker.enabled = true;
            }
            if (marker == "DeadForestMarker")
            {
                player.DeadForestMarker.enabled = true;
            }
            if (marker == "TowerMarker")
            {
                player.TowerMarker.enabled = true;
            }
            if (marker == "StatueMarker")
            {
                player.StatueMarker.enabled = true;
            }
            if (marker == "RockMarker")
            {
                player.RockMarker.enabled = true;
            }
            if (marker == "WellMarker")
            {
                player.WellMarker.enabled = true;
            }
            if (marker == "BushesMarker")
            {
                player.BushesMarker.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (marker == "SpawnMarker")
            {
                player.SpawnMarker.enabled = false;
            }
            if (marker == "DeadForestMarker")
            {
                player.DeadForestMarker.enabled = false;
            }
            if (marker == "TowerMarker")
            {
                player.TowerMarker.enabled = false;
            }
            if (marker == "StatueMarker")
            {
                player.StatueMarker.enabled = false;
            }
            if (marker == "RockMarker")
            {
                player.RockMarker.enabled = false;
            }
            if (marker == "WellMarker")
            {
                player.WellMarker.enabled = false;
            }
            if (marker == "BushesMarker")
            {
                player.BushesMarker.enabled = false;
            }
        }
    }
}
