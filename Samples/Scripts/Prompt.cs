using ChatGPTRequest;
using ChatGPTRequest.apiData;
using ChatGPTRequest.DataFormatter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Prompt
{
    public List<Message> Messages { get; set; }
    public float TimeStart { get; set; }

    public ChatGPTKey Keys;
    public ModelSettings arguments;
    public string endpoint = "https://api.openai.com/v1/chat/completions";

    public string FullMsg { get { return DataFormatter.FullParse(this); }}


}


