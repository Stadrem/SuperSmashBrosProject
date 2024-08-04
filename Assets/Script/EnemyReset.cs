using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyReset : MonoBehaviour
{
    //이건 0번 키를 누르면 씬이 리셋되는 디버깅 기능입니다!!
    //Enemy랑 아무런 관련 없음!!
 
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            SceneRest();
        }
    }

    void SceneRest()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Dong");
    }
}
