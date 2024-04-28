using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterCtrl : MonoBehaviour
{
    #region 멤버 변수
    public enum MonsterState { idle, trace, attack, die };
    public MonsterState monsterState = MonsterState.idle;

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private Animator animator;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;
    private bool isDie = false;

    public GameObject bloodEffect; // 혈흔효과 프리팹
    public GameObject bloodDecal; // 혈흔 데칼 효과 프리팹

    public int hp = 100; // 몬스터 생명 변수

    private GameUI gameUI; // GameUI에 접근하기 위한 변수
    #endregion

    void Start()
    {
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();

        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();

        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());

        //nvAgent.destination = playerTr.position;
    }

    void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
    }

    void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)
            {
                monsterState = MonsterState.attack;
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                // idle 상태
                case MonsterState.idle:
                    nvAgent.Stop(); // 추적중지
                    animator.SetBool("IsTrace", false);
                    break;

                // 추적 상태
                case MonsterState.trace:
                    nvAgent.destination = playerTr.position;
                    nvAgent.Resume(); // 추적 재시작
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsTrace", true);
                    break;

                // 공격 상태
                case MonsterState.attack:
                    nvAgent.Stop(); //추적 중지
                    animator.SetBool("IsAttack", true);
                    break;
            }
            yield return null;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "BULLET")
        {
            CreateBloodEffect(coll.transform.position);
            Destroy(coll.gameObject);
            animator.SetTrigger("IsHit");

            hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
            if (hp <= 0)
            {
                MonsterDie();
                SceneManager.LoadScene("Win");
            }

            Destroy(coll.gameObject);
            animator.SetTrigger("IsHit");
        }
    }

  void MonsterDie()
    {
        StopAllCoroutines();

        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.Stop();
        animator.SetTrigger("IsDie");

        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }

       gameUI.DispScore(50); // GameUI의 스코어 누적과 스코어 표시 함수 호출
    }

    void CreateBloodEffect(Vector3 pos)
    {
        // 혈흔효과
        GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(blood1, 2.0f);

        // 데칼효과
        // 이동(위치)
        Vector3 decalPos = monsterTr.position + (Vector3.up * 0.5f);
        // 회전
        Quaternion decalRot = Quaternion.Euler(90, 0, Random.Range(0, 360));
        // 프리펩
        GameObject blood2 = (GameObject)Instantiate(bloodDecal, decalPos, decalRot);
        // 크기
        float scale = Random.Range(1.5f, 3.5f);
        blood2.transform.localScale = Vector3.one * scale;

        Destroy(blood2, 5.0f);
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();
        nvAgent.Stop();
        animator.SetTrigger("IsPlayerDie");
    }
}