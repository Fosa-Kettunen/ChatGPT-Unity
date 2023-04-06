using ChatGPTRequest.DataFormatter;
using System;
using UnityEngine;
namespace ChatGPTRequest
{
    public struct Models
    {
        public static readonly string gptTurbo3_5 = "gpt-3.5-turbo";
    }
    [CreateAssetMenu(fileName = "CompletionArgs", menuName = "ChatGPT/Chat Arguments")]
    public class CompletionArguments : ScriptableObject
    {
        //https://platform.openai.com/docs/api-reference/chat/create


        [Tooltip("What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic." +
            "We generally recommend altering this or top_p but not both.")]
        public float temperature = 1f;

        [Tooltip("What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic." +
            "We generally recommend altering this or top_p but not both.")]
        public float top_p = 1;

        //[Tooltip("How many chat completion choices to generate for each input message.")]
        //public int n = 1;

        //[Tooltip("Up to 4 sequences where the API will stop generating further tokens.")]
        //public string[] stop = new string[4]; // max 4

        [Tooltip("The maximum number of tokens to generate in the chat completion." +
            "The total length of input tokens and generated tokens is limited by the model's context length.")]
        public int max_tokens = 4000;

        [Tooltip("Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics." +
            "<a href=\"https://platform.openai.com/docs/api-reference/parameter-details>See more information about frequency and presence penalties.</a>")]
        public float presences_penalty = 0;

        [Tooltip("Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim." +
            "<a href=\"https://platform.openai.com/docs/api-reference/parameter-details>See more information about frequency and presence penalties.</a>")]
        public float frequency_penalty = 0;


 

        public ModelSettings ReturnModelSettings()
        {
            return new ModelSettings {
                temperature = temperature,
                top_p = top_p,
                max_tokens = Mathf.Clamp(max_tokens,5,4000),
                presence_penalty = presences_penalty,
                frequency_penalty = frequency_penalty,
                stream = false, 
                model = Models.gptTurbo3_5,


            };
        }
    }
}

