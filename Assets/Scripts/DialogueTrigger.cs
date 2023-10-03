using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    public bool isStoryboard;

    void Start()
    {
        if(isStoryboard)
            StartCoroutine(TriggerDialogue());
    }

    IEnumerator TriggerDialogue()
    {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}