using System;
using System.Collections;
using System.Collections.Generic;
using Events.Args;
using UnityEngine;
using UnityEngine.UI;
using Utility;

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
    private bool _isReady = false;
    private bool _frameSkipped = false;
    public Text scoreText;

    private float _score = 0f;    
    private bool _inAir = false;

    public float Hp = 100f;
    public float MaxHp = 100f;
    public float StuckHpLoss = 75f;
    public float HpRegen = 20f;

    public Slider hpBar;
    private bool hpBarVisible = false;
    public CanvasGroup hpBarCg;

    private AudioSource _sound;
    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        camera = Camera.main;
        EventManager.StartListening(GameEventTypes.Death, OnDeath);
        EventManager.StartListening(GameEventTypes.ReadyToRun, arg0 => _isReady = true);
        EventManager.StartListening(GameEventTypes.SetPause, arg => { _isReady = !((PauseArgs) arg).IsPauseActive; } );
        _sound = gameObject.GetComponent<AudioSource>();
        hpBar.maxValue = MaxHp;
        hpBar.value = Hp;
        hpBarVisible = false;
        hpBarCg = hpBar.gameObject.GetComponent<CanvasGroup>();
        hpBarCg.alpha = 0f;
    }

    private void OnDeath(EventArgs arg0)
    {
        _isRunning = false;
        _score = 0f;
        Hp = MaxHp;
        hpBar.value = Hp;
        hpBarVisible = false;
        hpBarCg.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isReady) return;
        if (!_isRunning)
        {
            if (!Input.GetButtonDown("Jump") && !InputUtility.CheckTouchPhase(TouchPhase.Began)) return;
            if (!_frameSkipped)
            {
                _frameSkipped = true;
                return;
            }
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
            {
                camera.gameObject.transform.position = new Vector3(transform.position.x + 6, 0, -10);
                if (Hp < MaxHp) {Hp += HpRegen * Time.deltaTime;
                    if (Hp >= MaxHp) Hp = MaxHp;
                }
                else hpBarVisible = false;
            }
            else
            {
                Hp -= StuckHpLoss * Time.deltaTime;
                if (Hp <= 0f)
                {
                    EventManager.TriggerEvent(GameEventTypes.Death, EventArgs.Empty);
                }
                if (Hp < MaxHp)
                {
                    hpBarVisible = true;
                }
            }
            hpBar.value = Hp;
            hpBarCg.alpha = hpBarVisible ? 1f : 0f;
            if (!_inAir)
            {
                if (Math.Abs(transform.rotation.eulerAngles.z) > 0.01)
                    transform.Rotate(Vector3.forward, 20f);
            }
            if (!_inAir && (Input.GetButtonDown("Jump") || InputUtility.CheckTouchPhase(TouchPhase.Began)))
            {
                rigid.velocity = Vector2.up * jumpSpeed;
                transform.Rotate(Vector3.forward, -20f);
                _sound.Play();
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

    
}
