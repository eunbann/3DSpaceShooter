using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable] // 어트리뷰트
public class Anim
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runRight;
    public AnimationClip runLeft;
}

public class PlayerCtrl : MonoBehaviour
{
    #region 멤버 변수
    private float h = 0.0f;
    private float v = 0.0f;

    private Transform tr;
    public float moveSpeed = 10.0f; // 이동 속도 변수
    public float rotSpeed = 100.0f;

    public Anim anim;
    public Animation _animation;

    public int hp = 100; //플레이어의 생명 변수

    private int initHp; //플레이어의 생명 초깃 값
    public Image imgHpbar; //플레이어의 Health bar 이미지

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    #endregion

    void Start()
    {
        initHp = hp;

        tr = GetComponent<Transform>(); // 컴포너트 캐쉬 처리

        _animation = GetComponentInChildren<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();
    }
    
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Debug.Log("H=" + h.ToString());
        Debug.Log("V=" + h.ToString());

        // 전 후 좌 우 이동 방향 벡터 계산 
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed, Space.Self);
        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));

        if(v >= 0.1f)
        {
            _animation.CrossFade(anim.runForward.name, 0.3f);
        } else if (v <= -0.1f)
        {
            _animation.CrossFade(anim.runBackward.name, 0.3f);
        } else if (h >= 0.1f)
        {
            _animation.CrossFade(anim.runRight.name, 0.3f);
        } else if (h <= -0.1f)
        {
            _animation.CrossFade(anim.runLeft.name, 0.3f);
        }
        else
        {
            _animation.CrossFade(anim.idle.name, 0.3f);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        // 충돌한 콜라이더가 몬스터의 펀치면 플레이어 HP 차감
        if (coll.gameObject.tag == "PUNCH")
        {
            hp -= 10;

            imgHpbar.fillAmount = (float)hp / (float)initHp;

            Debug.Log("Player HP = " + hp.ToString());
            // 플레이어 생명이 0이하면 사망 처리
            if (hp <= 0)
            {
                PlayerDie();
                //SceneManager.LoadScene("Lose");
            }
        }
    }


    void PlayerDie()
    {
        Debug.Log("Player Die!");
        //SceneManager.LoadScene("Lose");

        /*GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        foreach(GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }*/

        OnPlayerDie();
        
    }

}
