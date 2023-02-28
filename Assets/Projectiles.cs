using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField] private List<Transform> projectileList = new List<Transform>();

    [SerializeField] private int projectileLevel;

    public Transform ChooseProjectile()
    {

        if (projectileLevel < projectileList.Count)
        {
            return projectileList[projectileLevel];
        }
        else
        {
            return null;
        }
    }


    public void LevelUp()
    {
        projectileLevel++;

        if (projectileLevel >= projectileList.Count - 1)
        {
            projectileLevel = projectileList.Count - 1;
        }
    }
}
