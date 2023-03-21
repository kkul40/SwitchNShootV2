using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBubble : MonoBehaviour
{
    public void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
