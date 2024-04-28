using System.Collections;
using UnityEngine;

public class WallCtrl : MonoBehaviour
{
    //스파크 파티클 프리팹 연결할 변수
    public GameObject sparkEffect;

    //충돌이 시작될 때 발생하는 이벤트
    void OnCollisionEnter(Collision coll)
    {
        //충돌한 게임 오브젝트의 태그 값 비교
        if(coll.collider.tag == "BULLET")
        {
            //스파클 파티클을 동적으로 생성
            GameObject spark = (GameObject)Instantiate(sparkEffect, coll.transform.position, Quaternion.identity);

            // ParticleSystem 컴포넌트의 수행시간(duration)이 지난 후 삭제 처리
            Destroy(spark, sparkEffect.GetComponent<ParticleSystem>().duration + 0.2f);

            //충돌한 게임 오브젝트 삭제
            Destroy(coll.gameObject);
        }
    }
}
