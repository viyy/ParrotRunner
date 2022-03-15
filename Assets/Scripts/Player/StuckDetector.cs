using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isStuck = false;

    private void Start()
    {
        EventManager.StartListening(GameEventTypes.Death, arg0 => isStuck = false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isStuck = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isStuck = false;
    }
}
