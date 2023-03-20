using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Transform left3;
    [SerializeField] private Transform left4;
    [SerializeField] private Transform right;
    [SerializeField] private Transform right2;
    [SerializeField] private Transform right3;
    [SerializeField] private Transform right4;

    [Header("Projectiles")] [SerializeField]
    private Transform enemyPrefab;

    [SerializeField] private Transform laser;

    [SerializeField] private Attacks currentAttack;
    [SerializeField] private float attackDuration;


    private readonly List<Transform> enemies = new();
    private bool isAttacking;
    private float lastAttackTime;


    private void Start()
    {
    }

    private void Update()
    {
        if (!isAttacking)
        {
            lastAttackTime += Time.deltaTime;
            if (lastAttackTime >= attackDuration)
            {
                ChooseAttack();
                lastAttackTime = 0;
            }
        }
    }

    private void ChooseAttack()
    {
        isAttacking = true;
        currentAttack = (Attacks)Random.Range(0, 4);

        //currentAttack = Attacks.TripleLeftAttack;
        switch (currentAttack)
        {
            case Attacks.PentaTriangelAttack:
                StartCoroutine(PentaAttack());
                break;
            case Attacks.TripleLeftAttack:
                StartCoroutine(LeftTriAttack());
                break;
            case Attacks.TripleRightAttack:
                StartCoroutine(RightTriAttack());
                break;
            case Attacks.PentaLeftAttack:
                StartCoroutine(LeftPentaAttack());
                break;
            case Attacks.PentaRigthAttack:
                StartCoroutine(RightPentaAttack());
                break;
        }
    }

    // daha sonra optimize et
    public IEnumerator PentaAttack()
    {
        enemies.Clear();
        var delay = 0.3f;

        InstantiateBossEnemy(middle);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(left2);
        InstantiateBossEnemy(right2);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(left4);
        InstantiateBossEnemy(right4);

        isAttacking = false;
    }

    public IEnumerator LeftTriAttack()
    {
        enemies.Clear();
        var delay = 0.3f;

        InstantiateBossEnemy(middle);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(middle);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(middle);

        isAttacking = false;
    }

    public IEnumerator RightTriAttack()
    {
        enemies.Clear();
        var delay = 0.3f;

        InstantiateBossEnemy(middle);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(middle);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(middle);

        isAttacking = false;
    }

    public IEnumerator LeftPentaAttack()
    {
        enemies.Clear();
        var delay = 0.3f;

        InstantiateBossEnemy(middle);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(left);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(left2);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(left3);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(left4);

        isAttacking = false;
    }

    public IEnumerator RightPentaAttack()
    {
        enemies.Clear();
        var delay = 0.3f;

        InstantiateBossEnemy(middle);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(right);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(right2);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(right3);
        yield return new WaitForSeconds(delay);
        InstantiateBossEnemy(right4);

        isAttacking = false;
    }

    private void InstantiateBossEnemy(Transform transform)
    {
        var enemyPrefab = Instantiate(this.enemyPrefab, transform.position, Quaternion.identity);
        enemyPrefab.GetComponent<Enemy>().SwitchState(EnemyState.BossEnemy);
    }
}