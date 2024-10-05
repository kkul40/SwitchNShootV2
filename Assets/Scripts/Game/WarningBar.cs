using UnityEngine;

public class WarningBar : MonoBehaviour
{
    [SerializeField] private Transform leftWarningBar, rightWarningBar;

    private void Start()
    {
        transform.position = Vector3.zero;
        CloseWarningBars();
    }

    public void OpenWarningBars()
    {
        leftWarningBar.position = new Vector3(CameraBorder.leftX + 0.4f, leftWarningBar.position.y, leftWarningBar.position.z);
        rightWarningBar.position = new Vector3(CameraBorder.rightX - 0.4f, rightWarningBar.position.y, rightWarningBar.position.z);
        
        leftWarningBar.gameObject.SetActive(true);
        rightWarningBar.gameObject.SetActive(true);
    }

    public void CloseWarningBars()
    {
        leftWarningBar.gameObject.SetActive(false);
        rightWarningBar.gameObject.SetActive(false);
    }
}