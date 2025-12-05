using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovementTutorial : MonoBehaviour
{
    public Vector3 hitboxLocation;
    
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
    
    private int currentStep = 1;
    private int lastStep = 0; // Track previous step
    private bool[] stepCompleted = new bool[4]; // Track completion of steps 1-4
    private bool isInTrigger = false;
    
    private void OnTriggerEnter(Collider other)
    {
        isInTrigger = true;
        
        // If this is the first time entering, start with step 1
        if (!stepCompleted[0])
        {
            currentStep = 1;
        }
        else
        {
            // Otherwise, find the first incomplete step
            for (int i = 0; i < stepCompleted.Length; i++)
            {
                if (!stepCompleted[i])
                {
                    currentStep = i + 1;
                    break;
                }
            }
        }
        
        UpdateTutorialText();
    }
    
    private void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
        // Don't clear text or reset progress when leaving trigger
    }
    
    private void Update()
    {
        // Continuously check step conditions regardless of trigger state
        CheckStepConditions();
        
        // Only update text if step changed and we're in the trigger
        if (currentStep != lastStep && isInTrigger)
        {
            UpdateTutorialText();
            lastStep = currentStep;
        }
    }
    
    private void UpdateTutorialText()
    {
        switch (currentStep)
        {
            case 1:
                tutorialText.SetText("Enter the trigger area");
                break;
            case 2:
                tutorialText.SetText("Press Left Control/Left Trigger to crouch");
                break;
            case 3:
                tutorialText.SetText("Press E/Right Trigger to pick up journal");
                break;
            case 4:
                tutorialText.SetText("Journal collected. Open door and hand to Husband.");
                exit.tutorialFinished = true;
                break;
        }
    }
    
    private void CheckStepConditions()
    {
        // Don't progress if all steps are completed
        if (currentStep > 4) return;
        
        switch (currentStep)
        {
            case 1:
                // Step 1 is completed just by entering the trigger
                if (isInTrigger && !stepCompleted[0])
                {
                    stepCompleted[0] = true;
                    currentStep = 2;
                }
                break;
                
            case 2:
                // Step 2 is completed by crouching (anywhere, not just in trigger)
                if (player.isCrouching && !stepCompleted[1])
                {
                    stepCompleted[1] = true;
                    currentStep = 3;
                }
                break;
                
            case 3:
                // Step 3 is completed by interacting with journal (anywhere)
                if ((body.isInteracting || Input.GetKeyDown(KeyCode.E)) && !stepCompleted[2])
                {
                    stepCompleted[2] = true;
                    currentStep = 4;
                }
                break;
                
            case 4:
                // Step 4 marks completion
                if (!stepCompleted[3])
                {
                    stepCompleted[3] = true;
                    // Tutorial is complete
                }
                break;
        }
    }
}