using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter
{
    private readonly string _textToWrite = null;
    private Text _uiText;
    private readonly float _timePerChar;
    private float _timer = 0f;
    private int _index = 0;
    private bool _writing = false;
    private AudioSource _sound = null;
    private bool _isUiTextNull;

    public TextWriter(Text uiText, string text, AudioSource soundSrc, float timePerChar = 0.02f)
    {
        _textToWrite = text;
        _uiText = uiText;
        _timePerChar = timePerChar;
        _writing = true;
        _sound = soundSrc;
    }

    public bool Writing => _writing;

    public void Update(float delta)
    {
        if (_isUiTextNull || !_writing) return;
        _timer -= delta;
        while (_timer <= 0f)
        {
            _timer += _timePerChar;
            _index++;
            var text = _textToWrite.Substring(0, _index);
            text += "<color=#00000000>" + _textToWrite.Substring(_index)+"</color>";
            _uiText.text = text;
            _sound.Play();
            if (_index < _textToWrite.Length) continue;
            _writing = false;
            return;
        }
    }

    public void Stop()
    {
        _writing = false;
    }

    public void Start()
    {
        _isUiTextNull = _uiText == null;
        _writing = true;
    }

    public void PrintAll()
    {
        _index = _textToWrite.Length - 1;
    }
}
