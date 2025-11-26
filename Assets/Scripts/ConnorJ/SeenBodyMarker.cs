using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeenBodyMarker : MonoBehaviour
{
    bool PlayerSeenBody = false;
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerSeenBody) { return; }
        if (other.gameObject.tag != "Player") { return; }

        PlayerSeenBody = true;

        //Body found sting noise
        AudioManager.Instance.PlaySound("BodyFoundSting");

        if (VignetteController.instance != null)
        {
            VignetteController.instance.FlashVignette.Invoke();
        }
        
    }
}
