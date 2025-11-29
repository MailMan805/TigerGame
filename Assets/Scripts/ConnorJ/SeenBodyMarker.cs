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

        if (VignetteController.instance != null)
        {
            VignetteController.instance.FlashVignette.Invoke();
        }
        
    }
}
