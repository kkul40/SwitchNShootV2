using UnityEngine;

public class Fades : MonoBehaviour
{
    [SerializeField] private Transform LeftFade, RightFade;
    [SerializeField] private float fadePosX;

    private void Start()
    {
        transform.position = Vector3.zero;
        OpenFades();

        LeftFade.transform.position = new Vector3(-fadePosX, 0f, 0f);
        RightFade.transform.position = new Vector3(fadePosX, 0f, 0f);
    }


    public void OpenFades()
    {
        LeftFade.gameObject.SetActive(true);
        RightFade.gameObject.SetActive(true);
    }

    public void CloseFades()
    {
        LeftFade.gameObject.SetActive(false);
        RightFade.gameObject.SetActive(false);
    }
}