using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAI
{
    public class GroupChatGPT : ChatGPT
    {
        private List<GroupChatGPT> npcGroup = new List<GroupChatGPT>();

        protected override void Start()
        {
            base.Start();
            npcGroup.Add(this);
        }

        protected override void HandleResponse(string responseContent)
        {
            base.HandleResponse(responseContent);
            NotifyGroupMembers(responseContent);
        }

        private void NotifyGroupMembers(string messageContent)
        {
            foreach (var npc in npcGroup)
            {
                if (npc != this)
                {
                    npc.ReceiveMessage(messageContent, this);
                }
            }
        }

        public void ReceiveMessage(string messageContent, GroupChatGPT sender)
        {
            if (isDialoguePaused || activeNPC != this)
            {
                return;
            }

            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = messageContent
            };

            messages.Add(newMessage);
            SendReply();
        }
    }
}
