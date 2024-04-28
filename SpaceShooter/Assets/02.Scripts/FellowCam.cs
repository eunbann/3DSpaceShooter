using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellowCam : MonoBehaviour
{
    public Transform targetTr;
    public float dist = 10.0f;
    public float height = 3.0f;
    public float dampTrace = 20.0f;
    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();    
    }

    void Update()
    {
        tr.position = Vector3.Lerp(
            tr.position,
            targetTr.position - (targetTr.forward * dist) + (Vector3.up * height),
            Time.deltaTime * dampTrace); // 보간 시간
        // 카메라가 타깃 게임 오브젝트를 바라보게 설정
        tr.LookAt(targetTr.position);
    }
}
