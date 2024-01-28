using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private TypewriterEffect _typeWriter;

    public void Init(Message msg, bool type = true)
    {
        string text = msg.message;
        _text.text = text;
        _typeWriter.enabled = type;
    }

    public void Init(Message msg, List<StandUpAction> standUpActions)
    {
        string text = msg.message;

        for (int i = 0; i < standUpActions.Count; i++)
        {
            text = text.Replace($"{{{i}}}", standUpActions[i].description);
        }

        _text.text = text;
        _typeWriter.enabled = true;
    }
}