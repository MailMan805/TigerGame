using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Settings")]
    public float typingSpeed = 0.05f;

    [Header("Dialogue Lines")]
    [TextArea(3, 10)]
    public string[] dialogueLines;

    [Header("Auto Start (Testing)")]
    public bool startOnAwake = false;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool dialogueActive = false;
    private Coroutine typingCoroutine;
    private NewPlayerMovement player;

    void Awake()
    {
        
        // Ensure dialogue is hidden at start
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Start()
    {
        player = FindAnyObjectByType<NewPlayerMovement>();
        if (startOnAwake && dialogueLines.Length > 0)
        {
            StartDialogue();
        }
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            HandleDialogueInput();
        }
        if(dialogueActive)
        {
            player.movementSpeed = 0;
        }
    }

    private void HandleDialogueInput()
    {
        if (isTyping)
        {
            // Skip typing animation
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogueText.text = dialogueLines[currentLineIndex];
            isTyping = false;
        }
        else
        {
            // Go to next line or end dialogue
            currentLineIndex++;
            if (currentLineIndex < dialogueLines.Length)
            {
                StartTyping();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void StartDialogue()
    {
        if (dialogueActive || dialogueLines.Length == 0) return;

        currentLineIndex = 0;
        player.canMove = false;
        dialogueActive = true;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        StartTyping();
    }

    public void StartDialogue(string[] customDialogueLines)
    {
        if (dialogueActive) return;

        dialogueLines = customDialogueLines;
        StartDialogue();
    }

    private void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        currentLineIndex = 0;
        player.canMove = true;
        player.movementSpeed = 5;
    }

    // Public method to check if dialogue is active
    public bool IsDialogueActive()
    {
        return dialogueActive;
    }

    // Method to manually set dialogue lines and start
    public void SetAndStartDialogue(string[] newDialogueLines)
    {
        dialogueLines = newDialogueLines;
        StartDialogue();
    }
}