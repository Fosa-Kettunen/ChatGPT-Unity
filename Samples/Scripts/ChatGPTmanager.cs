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

    public event Action<ApiDataPackage> onSuccess;
    public event Action<string> onFailure = null;
    public event Action<ApiDataPackage> streamData = null;
    //private MsgLogHandler msgLog = new();
    private OpenAIRequest request = new();
    private OpenAIRequestSteam OpenAIRequestSteam = new();
    private Prompt prompt = new();



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
            StartCoroutine(OpenAIRequestSteam.RunAPI(prompt, streamData, onFailure));
        }
        else
        {
            StartCoroutine(request.RUnAPI(prompt, onSuccess, onFailure));
        }

    }
}

