using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    private Transform _messageLayout;
    [SerializeField]
    private GameObject _messageControllerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initializing()
    {

    }

    public void WriteMessage(Message message)
    {
        MessageController msgCon = Instantiate(_messageControllerPrefab, _messageLayout).GetComponent<MessageController>();

        msgCon.Init(message);
    }
}
