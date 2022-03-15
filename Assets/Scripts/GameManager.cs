using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events.Args;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    public Camera mainCamera;

    public GameObject map;

    public List<GameObject> levelChunks;

    public int levelSize = 18;

    public GameObject startScreen;

    public GameObject dialogScreen;

    public GameObject pauseScreen;

    public bool running;

    private int _currentChunk = 0;

    private bool _isPause = false;

    private List<GameObject> _chunks = new List<GameObject>();
    private Random _rnd = new System.Random();
    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        EventManager.StartListening(GameEventTypes.Death, Respawn);
        EventManager.StartListening(GameEventTypes.StartRun, OnStartRun);
        EventManager.StartListening(GameEventTypes.StartDialogComplete, OnDialogComplete);
        _currentChunk = 0;
        dialogScreen.SetActive(true);
        startScreen.SetActive(false);
        pauseScreen.SetActive(false);
        GenerateLevel();
    }
    
    
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (_isPause) Exit();
            TogglePause();
        }

        if (_isPause)
        {
            if (Input.GetButtonDown("Jump"))
                TogglePause();
        }
        if (!(player.transform.position.x >= 9 + levelSize * _currentChunk)) return;
            AddChunk();
            ++_currentChunk;
    }

    private void AddChunk()
    {
        var go = Instantiate(levelChunks[_rnd.Next(0, levelChunks.Count)], map.transform);
        go.transform.position += new Vector3(levelSize*(3+_currentChunk),0,0);
        _chunks.Add(go);
        if (_chunks.Count < 7) return;
        var tmp = _chunks[0];
        _chunks.RemoveAt(0);
        Destroy(tmp);
    }

    private void OnStartRun(EventArgs arg0)
    {
        startScreen.SetActive(false);
    }

    private void OnDialogComplete(EventArgs args)
    {
        dialogScreen.SetActive(false);
        startScreen.SetActive(true);
        EventManager.StopListening(GameEventTypes.StartDialogComplete, OnDialogComplete);
        EventManager.TriggerEvent(GameEventTypes.ReadyToRun, null);
    }

    private void GenerateLevel()
    {
        
        for (var i = 0; i < 2; i++)
        {
            var go = Instantiate(levelChunks[_rnd.Next(0, levelChunks.Count)], map.transform);
            go.transform.position += new Vector3(levelSize*(1+i),0,0);
            _chunks.Add(go);
        }
    }

    private void Respawn(EventArgs args)
    {
        player.transform.position = new Vector3(-6f,-1.36f);
        player.transform.rotation = Quaternion.identity;
        mainCamera.gameObject.transform.position = new Vector3(0,0,-10f);
        startScreen.SetActive(true);
        foreach (var chunk in _chunks)
        {
            Destroy(chunk);
        }
        _chunks.Clear();
        _currentChunk = 0;
        GenerateLevel();
    }

    public void TogglePause()
    {
        _isPause = !_isPause;
        pauseScreen.SetActive(_isPause);
        EventManager.TriggerEvent(GameEventTypes.SetPause, new PauseArgs{IsPauseActive = _isPause});
    }

    public void Exit()
    {
        Application.Quit();
    }
}
