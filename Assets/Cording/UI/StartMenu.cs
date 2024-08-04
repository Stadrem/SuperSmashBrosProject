using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
//씬 매니지먼트를 설정하자.
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Text logoTextFactory;
    public Text startTextFactoty;

    bool isLogoControl = true;
    // Start is called before the first frame update
    void Start()
    {
        logoTextFactory.transform.localScale = Vector3.one * 0.1f;
        startTextFactoty.transform.position = Vector3.zero;
        startTextFactoty.transform.localScale = new Vector3(3, 3, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLogoControl == false) return; 
        {
            //print("update call" + 1111);
            logoTextControl();
        }

        StartTextControl();

    }//업데이트 끝

    public void OnClickStart()
    {
        //DongScene 로드하자.
        SceneManager.LoadScene("MainMenu");
    }

    void logoTextControl()
    {
        //iTween을 이용해서 움직임을 주자.
        //해시테이블 설정을 해주자. 
        //스케일 키값에 벡터원, 타임키값 0.5f, 이지타입키값 이지아웃바운드
        Hashtable logoHash = iTween.Hash(
            "scale", Vector3.one,
            "time", 3,
            "easetype", iTween.EaseType.easeOutBounce,
            "oncompletetarget", gameObject,
            "oncomplete", nameof(PrintText));
        //oncompletetarget 지정한 오브젝트네 함수호출
        //oncomplete기능호출
        //nameof를 자동으로 텍스트롤 바꿔줍니다.

        iTween.ScaleTo(logoTextFactory.gameObject, logoHash);
        //lastSecond를 second로 갱신

        isLogoControl = false;
    }

    void PrintText()
    {
        print("로고를 이징펑션으로 컨트롤 해봅니다");
    }

    void StartTextControl()
    {
        //startTextFactoty
        Hashtable startHash = iTween.Hash(
        "position", new Vector3(1600, 325, 0),
        "scale", Vector3.one,
        "time", 3,
        "easetype", iTween.EaseType.easeOutBounce,
        "oncompletetarget", gameObject);

        iTween.MoveTo(startTextFactoty.gameObject, startHash);
        iTween.ScaleTo(startTextFactoty.gameObject, startHash);
    }


}//class 끝
