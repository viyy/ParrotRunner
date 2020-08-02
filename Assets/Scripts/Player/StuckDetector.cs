using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isStuck = false;

    private void OnTriggerEnter(Collider other)
    {
        isStuck = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isStuck = false;
    }
}
