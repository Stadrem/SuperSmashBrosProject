using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryExit : MonoBehaviour
{
    public Image buttonImageFactory;

    GameObject imgLayout;
    Image buttonLayoutImage;

    // Start is called before the first frame update
    void Start()
    {
        //buttonLayoutImage.gameObject.SetActive(false);
        buttonLayoutImage = buttonImageFactory.GetComponent<Image>();
        //컴포넌트는 enabled로 비활성화
        //buttonLayoutImage.enabled = false;
        //비활성화 되었을때 트리거가 작동하지 않음으로 투명으로 하자.
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
        SceneManager.LoadScene("MainMenu");
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
