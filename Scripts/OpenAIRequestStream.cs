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
    public class OpenAIRequestStream
    {
        private class StreamDownloadHandler : DownloadHandlerScript
        {
            private Action<string> _onDataReceived;
            private Action<string> _onError;

            public StreamDownloadHandler(Action<string> onDataReceived, Action<string> onError) : base(new byte[1024])
            {
                _onDataReceived = onDataReceived;
                _onError = onError;
            }

            protected override bool ReceiveData(byte[] data, int dataLength)
            {
                if (data == null || data.Length < 1)
                {
                    _onError?.Invoke("No data received.");
                    return false;
                }

                string receivedData = Encoding.UTF8.GetString(data, 0, dataLength);
                _onDataReceived?.Invoke(receivedData);
                return true;
            }
        }

        public IEnumerator RunAPI(Prompt prompt, Action<ApiDataPackage> success, Action<string> failure)
        {
            string endpoint = prompt.endpoint;
            string orgKey = prompt.Keys.organization;
            string apiKey = prompt.Keys.apiKey;
            string msg = prompt.FullMsg;
            Debug.Log(msg);
            UnityWebRequest client = new(endpoint, "POST");
            StreamDownloadHandler streamDownloadHandler = new(data =>
            {
                success?.Invoke(DataFormatter.DataFormatter.CompileJsonString(data, prompt));
            }, error =>
            {
                failure?.Invoke(error);
            });

            client.downloadHandler = streamDownloadHandler;
            PopulateAuthHeaders(client, apiKey, orgKey);
            AddJsonToUnityWebRequest(client, msg);

            yield return client.SendWebRequest();
            client.uploadHandler.Dispose();
            if (client.result == UnityWebRequest.Result.ConnectionError || client.result == UnityWebRequest.Result.ProtocolError)
            {
                failure?.Invoke(client.error);
            }
        }

        private void AddJsonToUnityWebRequest(UnityWebRequest client, string json)
        {
            client.SetRequestHeader("Content-Type", "application/json");
            client.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        }

        private void PopulateAuthHeaders(UnityWebRequest client, string apiKey, string orgKey)
        {
            client.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            //client.SetRequestHeader("User-Agent", $"");
            if (!string.IsNullOrEmpty(orgKey)) client.SetRequestHeader("OpenAI-Organization", orgKey);
        }
    }
}