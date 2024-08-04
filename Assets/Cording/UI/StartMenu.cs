using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
//�� �Ŵ�����Ʈ�� ��������.
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

    }//������Ʈ ��

    public void OnClickStart()
    {
        //DongScene �ε�����.
        SceneManager.LoadScene("MainMenu");
    }

    void logoTextControl()
    {
        //iTween�� �̿��ؼ� �������� ����.
        //�ؽ����̺� ������ ������. 
        //������ Ű���� ���Ϳ�, Ÿ��Ű�� 0.5f, ����Ÿ��Ű�� �����ƿ��ٿ��
        Hashtable logoHash = iTween.Hash(
            "scale", Vector3.one,
            "time", 3,
            "easetype", iTween.EaseType.easeOutBounce,
            "oncompletetarget", gameObject,
            "oncomplete", nameof(PrintText));
        //oncompletetarget ������ ������Ʈ�� �Լ�ȣ��
        //oncomplete���ȣ��
        //nameof�� �ڵ����� �ؽ�Ʈ�� �ٲ��ݴϴ�.

        iTween.ScaleTo(logoTextFactory.gameObject, logoHash);
        //lastSecond�� second�� ����

        isLogoControl = false;
    }

    void PrintText()
    {
        print("�ΰ� ��¡������� ��Ʈ�� �غ��ϴ�");
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


}//class ��
