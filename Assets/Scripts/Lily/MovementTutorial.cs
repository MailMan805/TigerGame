using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MovementTutorial : MonoBehaviour
{
    public Vector3 hitboxLocation;

    private int playerTutorialStep = 1;

    NewPlayerMovement player;

    Body body;

    public TextMeshProUGUI tutorialText;
    public HouseExit1 exit;

    void Start()
    {
        exit = FindObjectOfType<HouseExit1>();
        player = FindObjectOfType<NewPlayerMovement>();
        body = FindObjectOfType<Body>();
        tutorialText.SetText("Use your mouse or right joystick to look around the scene. Press WASD on your keyboard or use the left joystick to move.");
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerTutorialStep == 1)
        {
            playerTutorialStep += 1;
        }
        else if (playerTutorialStep == 2)
        {
            playerTutorialStep += 1;
            tutorialText.SetText("Press Left Control on your keyboard or the Left Trigger on your controller to crouch to look beneath your bed.");
        }
        else if (playerTutorialStep == 3 && player.isCrouching == true)
        {
            playerTutorialStep += 1;
            tutorialText.SetText("Press E on your keyboard or use the Right Trigger to pick up the journal from beneath your bed.");
        }
        else if (playerTutorialStep == 4 && body.isInteracting == true) // i'll figure out why the new player input system doesn't work for this >:(
        {
            playerTutorialStep += 1;
            tutorialText.SetText("You collected the journal. Now open the door and hand it to your Husband.");
            exit.tutorialFinished = true;
        }
    }
}
