using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text txtScore; // Text UI 항목 연결을 위한 변수
    private int totScore = 0; // 누적 점수를 기록하기 위한 변수

    void Start()
    {
        totScore = PlayerPrefs.GetInt("TOT_SCORE", 0); // 처음 실행 후 저장된 스코어 정보 로드

        DispScore(0);
    }
    // 점수 누적 및 화면 표시
    public void DispScore(int score)
    {
        totScore += score;
        txtScore.text = "SCORE<color=#ff0000>" + totScore.ToString() + "</color>";

        PlayerPrefs.SetInt("TOT_SCORE", totScore); // 스코어 저장
    }
}