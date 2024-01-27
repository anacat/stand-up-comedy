using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Message", menuName = "ScriptableObjects/Message", order = 1)]
public class Message : ScriptableObject
{
    [TextArea(5,20)]
    public string message;
    public int charSize;
}
