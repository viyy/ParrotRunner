using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 7f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float cameraMoveTreshold = 0.001f;
    public Camera camera;
    public Rigidbody2D rigid;
    public StuckDetector check;

    private bool _isRunning = false;
    public Text scoreText;

    private float _score = 0f;    
    private bool _inAir = false;
    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        camera = Camera.main;
        EventManager.StartListening(GameEventTypes.Death, OnDeath);
    }

    private void OnDeath(EventArgs arg0)
    {
        _isRunning = false;
        _score = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRunning)
        {
            if (!Input.GetButtonDown("Jump") && !CheckTouchPhase(TouchPhase.Began)) return;
            _isRunning = true;
            EventManager.TriggerEvent(GameEventTypes.StartRun, EventArgs.Empty);
            //rigid.velocity = Vector2.right*speed;
        }
        else
        {
            _score += speed * Time.deltaTime;
            _inAir = Math.Abs(rigid.velocity.y) > 0.001;
            var dx = speed * Time.deltaTime * Vector3.right;
            transform.position += dx;
            //rigid.MovePosition(transform.position+dx);
            if (!check.isStuck)
                camera.gameObject.transform.position = new Vector3(transform.position.x + 6, 0, -10);
            if (!_inAir && (Input.GetButtonDown("Jump") || CheckTouchPhase(TouchPhase.Began)))
            {
                rigid.velocity = Vector2.up * jumpSpeed;
            }

            if (rigid.velocity.y < 0)
            {
                rigid.velocity += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * Vector2.up;
            }
            else if (rigid.velocity.y > 0 && (!Input.GetButton("Jump") && Input.touchCount == 0))
            {
                rigid.velocity += Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime * Vector2.up;
            }

            scoreText.text = $"{_score:000000}";
        }
    }

    private bool CheckTouchPhase(TouchPhase targetPhase)
    {
        if (Input.touchSupported && Input.touchCount > 0)
            return Input.GetTouch(0).phase == targetPhase;
        return false;
    }
}
