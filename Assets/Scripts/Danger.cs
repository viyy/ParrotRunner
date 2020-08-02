using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : MonoBehaviour
{
    private Collider2D collider;
    // Start is called before the first frame update
    private void Awake()
    {
        collider = gameObject.GetComponent<CompositeCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
#if UNITY_EDITOR
        Debug.Log($"Event triggered: Death");
#endif
        if (other.gameObject.CompareTag("Player"))
            EventManager.TriggerEvent(GameEventTypes.Death, EventArgs.Empty);
    }
}
