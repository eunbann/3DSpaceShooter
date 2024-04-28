using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet; //총알 오브젝트
    public Transform firePos; //총알 발사 좌표
    public AudioClip fireSfx; //총알 발사 사운드
    private AudioSource source = null; //AudioSource 컴포넌트를 저장할 변수
    public MeshRenderer muzzleFlash; 

    void Start()
    {
        source = GetComponent<AudioSource>();
        muzzleFlash.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }    
    }

    void Fire()
    {
        CreateBullet();
        source.PlayOneShot(fireSfx, 0.9f);
        StartCoroutine(this.ShowMuzzleFlash());
    }

    void CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
    }

    IEnumerator ShowMuzzleFlash()
    {
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;

        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));
        muzzleFlash.enabled = false;
    }
}
