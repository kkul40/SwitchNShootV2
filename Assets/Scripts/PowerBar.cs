using UnityEngine;

public class PowerBar : MonoBehaviour
{
    [SerializeField] private GameObject fillObject;
    public bool isFilled = false;

    public void Fill()
    {
        fillObject.SetActive(true);
        isFilled = true;
    }

    public void DontFill()
    {
        fillObject.SetActive(false);
        isFilled = false;
    }
}