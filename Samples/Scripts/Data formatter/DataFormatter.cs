using ChatGPTRequest.apiData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static ChatGPTRequest.DataFormatter.JsonFormatter;

namespace ChatGPTRequest.DataFormatter
{
    public static class DataFormatter
    {
        public static bool Log = false;

        const string _pattern = "\"total_tokens\":(\\d+)}.+\"content\":\"(.+?)\"}";

        static readonly Regex _contentRegex = new("{\"content\":\"(.+?)\"}", RegexOptions.Singleline);
        static readonly Regex _finnisreasonRegex = new("\"finish_reason\":(.+?)}", RegexOptions.Singleline);
        public static ApiDataPackage CompileJsonString(string json, Prompt Prompt)
        {
            if (Prompt.arguments.stream)
            {
                if (Log) { Debug.Log($"regexed {json}"); }
                Match matchA = _contentRegex.Match(json);
                Match matchB = _finnisreasonRegex.Match(json);
                Message chat = new(
                    ReScapeJson(matchA.Groups[1].Value),
                    Message.Roles.assistant
                    );

                float executionTime = Time.realtimeSinceStartup - Prompt.TimeStart;
                return new ApiDataPackage(chat, executionTime, matchB.Groups[1].Value);
            }
            else
            {
                if (Log) { Debug.Log($"regexed {json}"); }
                Match match = Regex.Match(json, _pattern, RegexOptions.Singleline);

                int total_tokens = int.Parse(match.Groups[1].Value);

                Message chat = new(
                    ReScapeJson(match.Groups[2].Value),
                    Message.Roles.assistant,
                    total_tokens
                    );

                float executionTime = Time.realtimeSinceStartup - Prompt.TimeStart;
                return new ApiDataPackage(chat, executionTime);
            }

            
        }

 
        public static string AddMsg(List<Message> messages)
        {
            string[] pairStrings = new string[messages.Count];
            for (int i = 0; i < messages.Count; i++)
            {
                string msg = EscapeJson(messages[i].message);
                pairStrings[i] = $"{{{JsonPair("role",messages[i].roleStr)},{JsonPair("content",msg)}}}";
            }
            string joinedString = JsonPair("messages", pairStrings);
            return joinedString;
        }


        static string[] AddPromt(ModelSettings settings)
        {
            Type settingsType = typeof(ModelSettings);
            FieldInfo[] fields = settingsType.GetFields();
            string[] strings = new string[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                object value = field.GetValue(settings);
               
                strings[i] = JsonPair(field.Name, value);

            }
            return strings;
        }

        public static string FullParse(Prompt data)
        {
            List<string> strings = new() { Capacity = 13};

            strings.AddRange(AddPromt(data.arguments));
            strings.Add(AddMsg(data.Messages));
            string a = "{"+string.Join(",", strings) + "}";

            if (Log) { Debug.Log($"added {a}"); }

            return  a ;
        }
    }
}


/*
 {"content":"(.+?)"}.+"finish_reason":(.+?)}

data: {"id":"chatcmpl-12rLBzJDoc11lk2KlE55EEaDxFFAu","object":"chat.completion.chunk","created":1680439397,"model":"gpt-3.5-turbo-0301","choices":[{"delta":{"content":"?"},"index":0,"finish_reason":null}]}
    */

