using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChatGPTRequest;
using ChatGPTRequest.DataFormatter;
using System;
using ChatGPTRequest.apiData;


public class ChatGPTmanager : MonoBehaviour
{
    public ChatGPTKey key;
    public CompletionArguments completionArguments;
    /// <summary>
    /// Returns one package with whole message inside it 
    /// <br/> This May take several seconds depending on the length of the response
    /// </summary>
    public event Action<ApiDataPackage> PackagedData;
    /// <summary>
    /// returns error message
    /// </summary>
    public event Action<string> OnFailure = null;
    /// <summary>
    /// Returns multiple packages.<br/>
    /// NOTE: last package will be empty and has its value "finnish_reason" as string "\"stop\"".
    /// </summary>
    public event Action<ApiDataPackage> StreamData = null;
    //private MsgLogHandler msgLog = new();
    private readonly OpenAIRequest request = new();
    private readonly OpenAIRequestSteam OpenAIRequestSteam = new();
    private readonly Prompt prompt = new();



    private void Start()
    {
        
        if (key == null || string.IsNullOrEmpty(key.apiKey))
        {
            Debug.LogError("No valid key set in:" + key.name);
            return;
        }

        if (completionArguments == null)
        {
            Debug.LogError(completionArguments.name + " not set");
            return;
        }
    }
    /// <summary>
    /// Executes a webrequest and returns results via Actions
    /// </summary>
    /// <param name="input">Text input</param>
    /// <param name="onSuccess">Action that outputs data class</param>
    /// <param name="onFailure">Sends error message if request fails</param>
    public void DoApiCompletation(List<Message> message,bool stream = false)
    {
          
        if (message == null)
        {
            Debug.LogWarning("Input is empty");
            return;
        }


        //prompt gather
        prompt.TimeStart = Time.realtimeSinceStartup;
        prompt.arguments = completionArguments.ReturnModelSettings();
        prompt.arguments.stream = stream;
        prompt.Keys = key;
        prompt.Messages = message;

        //start task
        if (stream)
        {
            StartCoroutine(OpenAIRequestSteam.RunAPI(prompt, StreamData, OnFailure));
        }
        else
        {
            StartCoroutine(request.RUnAPI(prompt, PackagedData, OnFailure));
        }

    }
}

