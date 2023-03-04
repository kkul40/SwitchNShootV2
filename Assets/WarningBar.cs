using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningBar : MonoBehaviour
{

    [SerializeField] private Transform leftWarningBar, rightWarningBar;


    void Start()
    {
        transform.position = Vector3.zero;

        CloseWarningBars();
    }
    


    public void OpenWarningBars()
    {
        leftWarningBar.gameObject.SetActive(true);
        rightWarningBar.gameObject.SetActive(true);
    }

    public void CloseWarningBars()
    {
        leftWarningBar.gameObject.SetActive(false);
        rightWarningBar.gameObject.SetActive(false);
    }
}
