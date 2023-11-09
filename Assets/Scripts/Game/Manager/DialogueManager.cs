using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialogueObject;
    public TextMeshPro text;
    public List<Dialogue> dialogues;

    public static event Action OnDialogueEnded;

    private int currentDialogue;

    public bool dialgoueActive;

    private void Start()
    {
        currentDialogue = 0;
        DialogueObject.SetActive(false);
        dialgoueActive = false;
    }

    public void StartDialogue(int stage)
    {
        if (currentDialogue > dialogues.Count - 1) return;
        
        StartCoroutine(DialogueWriterCo(stage));
    }

    IEnumerator DialogueWriterCo(int stage)
    {
        text.text = "";
        dialgoueActive = true;
        DialogueObject.SetActive(true);

        while (dialogues[currentDialogue].stage == stage)
        {
            foreach (var a in dialogues[currentDialogue].line)
            {
                text.text += a;
                yield return new WaitForSeconds(0.075f);
            }

            yield return new WaitForSeconds(1f);
            currentDialogue++;
            text.text = "";


            if (currentDialogue > dialogues.Count - 1)
            {
                Debug.Log("Cikis yap");
                break;
            }
        }

        text.text = "";
        dialgoueActive = false;
        DialogueObject.SetActive(false);

        OnDialogueEnded?.Invoke();
    }
}

[Serializable]
public class Dialogue
{
    public int stage;
    [TextArea(3,3)]
    public string line;
}
