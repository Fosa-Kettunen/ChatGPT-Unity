using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Key", menuName = "ChatGPT/Key")]

    public class ChatGPTKey : ScriptableObject
{
    public string apiKey;
    public string organization;
}
