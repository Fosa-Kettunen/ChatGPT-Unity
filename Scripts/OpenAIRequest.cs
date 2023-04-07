

using ChatGPTRequest.apiData;
using ChatGPTRequest.DataFormatter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ChatGPTRequest
{

    public class OpenAIRequest
    {
        public IEnumerator RUnAPI(Prompt prompt, Action<ApiDataPackage> success, Action<string> failure)
        {
            string endpoint = prompt.endpoint;
            string orgKey   = prompt.Keys.organization;
            string apiKey   = prompt.Keys.apiKey;
            string msg      = prompt.FullMsg; //full message is constructed when data is fetched

            UnityWebRequest client = UnityWebRequest.Post(endpoint, string.Empty);
            PopulateAuthHeaders(client, apiKey, orgKey);
            //Debug.Log(msg);
            AddJsonToUnityWebRequest(client, msg);
            Debug.Log("retriving data...");
            yield return client.SendWebRequest();

            client.uploadHandler.Dispose();


            if (client.result == UnityWebRequest.Result.Success)
            {
                //Debug.Log(client.downloadHandler.text);
                success?.Invoke(
                    DataFormatter.DataFormatter.CompileJsonString(client.downloadHandler.text,prompt));
              
            }
            else
            {
                failure?.Invoke(client.error);
                //yield break;
            }
        }

        private void AddJsonToUnityWebRequest(UnityWebRequest client, string json)
        {
            client.SetRequestHeader("Content-Type", "application/json");
            client.uploadHandler = new UploadHandlerRaw(
                Encoding.UTF8.GetBytes(json)
            );
        }
        private void PopulateAuthHeaders(UnityWebRequest client,string apikey, string orgKey)
        {
            client.SetRequestHeader("Authorization", $"Bearer {apikey}");
            //client.SetRequestHeader("User-Agent", $"");
            if (!string.IsNullOrEmpty(orgKey)) client.SetRequestHeader("OpenAI-Organization", orgKey);
        }

    }
 

}
