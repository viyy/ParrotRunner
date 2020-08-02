using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Text jumpToStartText;

    public bool running;

    private int _currentChunk = 0;

    private List<GameObject> _chunks = new List<GameObject>();
    private Random _rnd = new System.Random();
    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        EventManager.StartListening(GameEventTypes.Death, Respawn);
        EventManager.StartListening(GameEventTypes.StartRun, OnStartRun);
        _currentChunk = 0;
        GenerateLevel();
    }

    private void Update()
    {
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
        jumpToStartText.enabled = false;
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
        mainCamera.gameObject.transform.position = new Vector3(0,0,-10f);
        jumpToStartText.enabled = true;
        foreach (var chunk in _chunks)
        {
            Destroy(chunk);
        }
        _chunks.Clear();
        _currentChunk = 0;
        GenerateLevel();
    }
}
