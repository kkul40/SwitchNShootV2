using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;


    public void SetText(string message)
    {
        text.text = message;
    }
}