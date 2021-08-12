using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : Interactables
{
    private void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
    }

    public int NPCIndex;

    public Dialog dialog;

    public DialogManager dialogManager;

    public override void triggerInteraction(PlayerInteractionController controller)
    {
        base.triggerInteraction(controller);
        if(dialogManager == null)
        {
            dialogManager = FindObjectOfType<DialogManager>();
        }
        dialogManager.startDialog(dialog);
    }
}
