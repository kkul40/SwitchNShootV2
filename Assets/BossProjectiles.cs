using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum Attacks
{
    PentaTriangelAttack,
    TripleLeftAttack,
    TripleRightAttack,
    PentaLeftAttack,
    PentaRigthAttack
}
public class BossProjectiles : MonoBehaviour
{
    [SerializeField] private Transform middle;
    [SerializeField] private Transform left;
    [SerializeField] private Transform left2;
    [SerializeField] private Transform right;
    [SerializeField] private Transform right2;

    [Header("Projectiles")]
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform laser;


    private List<Transform> enemies = new List<Transform>();

    private Attacks currentAttack;
    private bool isAttacking;
    [SerializeField] private float attackDuration;
    private float lastAttackTime;


    void Start()
    {
        ChooseAttack();
    }

    void Update()
    {

    }

    private void ChooseAttack()
    {
        currentAttack = (Attacks)Random.Range(0, 4);

        currentAttack = Attacks.PentaTriangelAttack;
        switch (currentAttack)
        {
            case Attacks.PentaTriangelAttack:
                StartCoroutine(PentaAttack());
                break;
            case Attacks.TripleLeftAttack:
                break;
            case Attacks.TripleRightAttack:
                break;
            case Attacks.PentaLeftAttack:
                break;
            case Attacks.PentaRigthAttack:
                break;
            default:
                break;
        }
    }

    public IEnumerator PentaAttack()
    {
        float delay = 0.1f;


        enemies.Add(Instantiate(enemyPrefab, middle.position, Quaternion.identity));

        yield return new WaitForSeconds(delay);
        enemies.Add(Instantiate(enemyPrefab, left.position, Quaternion.identity));

        enemies.Add(Instantiate(enemyPrefab, right.position, Quaternion.identity));


        yield return new WaitForSeconds(delay);
        enemies.Add(Instantiate(enemyPrefab, left2.position, Quaternion.identity));

        enemies.Add(Instantiate(enemyPrefab, right2.position, Quaternion.identity));

        yield return new WaitForNextFrameUnit();

        foreach (var item in enemies)
        {
            item.GetComponent<Enemy>().SwitchState(EnemyState.BossEnemy);
        }

    }


    
}
