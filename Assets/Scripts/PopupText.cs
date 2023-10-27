using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    public void Instantiate(string message, Color? color = null)
    {
        text.text = message;
        text.color = color ?? Color.white;
    }
}