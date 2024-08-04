using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIReStart : MonoBehaviour
{
    public Image buttonImageFactory;

    GameObject imgLayout;
    Image buttonLayoutImage;

    // Start is called before the first frame update
    void Start()
    {
        //buttonLayoutImage.gameObject.SetActive(false);
        buttonLayoutImage = buttonImageFactory.GetComponent<Image>();
        //������Ʈ�� enabled�� ��Ȱ��ȭ
        //buttonLayoutImage.enabled = false;
        //��Ȱ��ȭ �Ǿ����� Ʈ���Ű� �۵����� �������� �������� ����.
        buttonLayoutImage.color = new Color(0, 0, 0, 0);

       // OnClickReStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickReStart()
    {
        Time.timeScale = 1;
        //DongScene �ε�����.
        SceneManager.LoadScene("Dong");
    }
    public void OnLayoutImage()
    {
        // buttonLayoutImage.enabled = true;
        //buttonLayoutImage.color = Color.yellow;
        buttonLayoutImage.color = new Color(1, 0.6f, 0, 1);
    }
    public void OffLayoutImage()
    {
        buttonLayoutImage.color = new Color(0, 0, 0, 0);
    }
}
