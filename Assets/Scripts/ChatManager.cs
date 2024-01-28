using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    private Transform _messageLayout;
    [SerializeField]
    private GameObject _messageControllerPrefab;
    [SerializeField]
    private MessageController _loadingMessage;

    private bool _started = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!_started && Input.anyKeyDown)
        {
            _started = true;
            _loadingMessage.gameObject.SetActive(true);
        }
    }

    public void WriteMessage(Message message)
    {
        MessageController msgCon = Instantiate(_messageControllerPrefab, _messageLayout).GetComponent<MessageController>();

        msgCon.Init(message);
    }

    public void WriteMessageEnd(Message message)
    {
        MessageController msgCon = Instantiate(_messageControllerPrefab, _messageLayout).GetComponent<MessageController>();

        msgCon.Init(message);
        msgCon.GetComponent<TypewriterEffect>().finishedEvent.AddListener(() => Application.Quit());
    }



    public void WriteModifiedMessage(Message message, List<StandUpAction> actions)
    {
        MessageController msgCon = Instantiate(_messageControllerPrefab, _messageLayout).GetComponent<MessageController>();

        msgCon.Init(message, actions);
    }
}
