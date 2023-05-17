using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    [SerializeField] private GameObject fillObject;

    public void Fill()
    {
        fillObject.SetActive(true);
    }

    public void DontFill()
    {
        fillObject.SetActive(false);
    }
}
