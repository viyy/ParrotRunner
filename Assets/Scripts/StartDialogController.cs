using UnityEngine;
using UnityEngine.UI;
using Utility;

public class StartDialogController : MonoBehaviour
{

    public Text uiText;
    public string text;
    public Text hint;
    private TextWriter _tw;
    public AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        _tw = new TextWriter(uiText, text, sound);
        hint.text = "<JUMP> для пропуска";
    }

    private void OnEnable()
    {
        _tw = new TextWriter(uiText, text, sound);
        hint.text = "<JUMP> для пропуска";
    }

    // Update is called once per frame
    void Update()
    {
        _tw.Update(Time.deltaTime);
        if (!Input.GetButtonDown("Jump") && !InputUtility.CheckTouchPhase(TouchPhase.Began)) return;
        if (_tw.Writing)
        {
            _tw.PrintAll();
            hint.text = "<JUMP> для продолжения";
        }
        else
        {
            _tw.Stop();
            EventManager.TriggerEvent(GameEventTypes.StartDialogComplete, null);
        }
    }
}
