using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace ChatGPTRequest.apiData
{
    [Serializable]
    public struct Message
    {
        public  enum Roles { system, user, assistant };
        [TextArea]
        public string message;
        public Roles role;
        public string roleStr { get { return GetRoleString(role); } }
        public int? total_tokens { get; }


        /// <summary>
        /// Represents a message in a chat application.
        /// </summary>
        /// <param name="message">The text of the message.</param>
        /// <param name="role">The role of the message (default is user).</param>
        public Message(string message, Roles role = Roles.user, int? tokenSize = null)
        {
            this.role = role;
            this.message = message;
            this.total_tokens = tokenSize;

        }

        public static string GetRoleString(Roles role) => role switch
        {
            Roles.system => "system",
            Roles.user => "user",
            Roles.assistant => "assistant",
            _ => "user",
        };





    }

}
