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
    [Space(10)]


    //private MsgLogHandler msgLog = new();
    private OpenAIRequest request = new();
    private OpenAIRequestSteam OpenAIRequestSteam = new();
    private Prompt prompt = new();

    // "{ \"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"Hello!\"}] }";

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
    public void DoApiCompletation(List<Message> message, Action<ApiDataPackage> onSuccess, Action<string> onFailure = null,bool stream = false)
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

        //StartCoroutine(request.RUnAPI(prompt, onSuccess, onFailure));
        //start task
        if (stream)
        {
            StartCoroutine(OpenAIRequestSteam.RunAPI(prompt, onSuccess, onFailure));
        }
        else
        {
            StartCoroutine(request.RUnAPI(prompt, onSuccess, onFailure));
        }

    }
}

