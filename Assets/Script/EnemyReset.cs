using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyReset : MonoBehaviour
{
    //�̰� 0�� Ű�� ������ ���� ���µǴ� ����� ����Դϴ�!!
    //Enemy�� �ƹ��� ���� ����!!
 
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
