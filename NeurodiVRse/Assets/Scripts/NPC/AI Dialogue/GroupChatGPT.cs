using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAI
{
    public class GroupChatGPT : ChatGPT
    {
        // A list to keep track of all the NPCs in the group
        private List<GroupChatGPT> npcGroup = new List<GroupChatGPT>();

        protected override void Start()
        {
            base.Start();

            // Register this NPC in the group
            npcGroup.Add(this);
        }

        protected override void HandleResponse(string responseContent)
        {
            base.HandleResponse(responseContent);

            // Notify other NPCs in the group
            NotifyGroupMembers(responseContent);
        }

        private void NotifyGroupMembers(string messageContent)
        {
            // Iterate through all the NPCs in the group
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

            // Trigger a reply from this NPC
            SendReply();
        }
    }
}
