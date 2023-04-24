using ChatGPTRequest.apiData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePromptChat : MonoBehaviour
{

    [SerializeField] private ChatGPTmanager gpt;
    [SerializeField] private Text input;
    [SerializeField] private Text output;

    [SerializeField] private bool Stream = false;

    [SerializeField] private string UserPrefix = "User: ";
    [SerializeField] private string UserSufix = "\n";
    [SerializeField] private string BotPrefix = "Bot: ";
    [SerializeField] private string BotSufix = "\n";

    [SerializeField] private List<Message> systemPrompts;

    bool _runOnce;

    //https://platform.openai.com/docs/guides/chat

    private void Start()
    {
        gpt.PackagedData += DistributeData;
        gpt.OnFailure += onFail;
    }

    // A helper method for creating a list of chat messages based on a user message input.
    List<Message> ChatMessage(string message)
    {
        List<Message> msgList = new();

        // sysMsg (optonal)
        if (systemPrompts.Count > 0)
            msgList.AddRange(systemPrompts);
        // userMsg
        msgList.Add(new Message(message, Message.Roles.user));

        return msgList;
    }

    // A public method for making a chat request using the user's input message.
    public void DoRequest()
    {
        _runOnce = true;

        gpt.DoApiCompletation(ChatMessage(input.text),Stream);
    }

    // method for processing the results of a chat request.
    void DistributeData(ApiDataPackage data)
    {

        if (Stream)
        {
            if (_runOnce)
            {
                _runOnce = false;

                output.text += UserPrefix + input.text + UserSufix;
                output.text += BotPrefix;
            }

            if (data.finnish_reason == "\"stop\"")
            {
                output.text += BotSufix;
            }
            else
            {
                output.text += data.Message.message;
            }
        }
        else
        {
            output.text += (UserPrefix + input.text + UserSufix);
            output.text += BotPrefix + data.Message.message + BotSufix;

            print($"Executed ChatGPT call in: {data.ExecutionTime:F3} seconds");
        }
    }

    void onFail(string errMsg)
    {
        Debug.LogWarning(errMsg);
    }

}

