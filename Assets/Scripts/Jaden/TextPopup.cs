using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{

    public float ShowTextInSeconds = 3f;
    public string TextToShow;
    public bool Show = false;

    public TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {
        if(Show)
        {
            Text.text = TextToShow;
            Text.enabled = true;
            StartCoroutine(HideText());
        }
        
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(ShowTextInSeconds);
        Text.enabled = false;
    }
}
