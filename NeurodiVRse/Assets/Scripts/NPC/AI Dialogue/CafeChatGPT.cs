/*using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeChatGPT : ChatGPT
{
    [SerializeField] private static CafeChatGPT activeNPC;

    public static CafeChatGPT ActiveNPC
    {
        get { return activeNPC; }
        private set { activeNPC = value; }
    }

    protected override void Start()
    {
        base.Start();
        if (activeNPC == null)
        {
            activeNPC = this;
        }
        else
        {
            DeactivateNPC();
        }
    }

    protected override async void SendReply()
    {
        if (ActiveNPC != this || isDialoguePaused)
        {
            Debug.Log("Dialogue is paused or this NPC is not active. Reply not sent.");
            return;
        }

        base.SendReply();
    }

    public override void ActivateNPC()
    {
        base.ActivateNPC();
        ActiveNPC = this;
    }

    public override void DeactivateNPC()
    {
        base.DeactivateNPC();
        if (ActiveNPC == this)
        {
            ActiveNPC = null;
        }
    }
}
*/