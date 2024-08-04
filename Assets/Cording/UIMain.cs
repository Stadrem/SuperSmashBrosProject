using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public Image battleLayOut;
    public Image controlLayOut;
    public Image commandLayOut;
    public Image exitLayOut;
    public AudioClip[] menuVoice;

    bool isCurrTime = false;
    bool isClickBattleButton = false;
    float currTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        battleLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 0);
        controlLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 0);
        commandLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 0);
        exitLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(isCurrTime)
        {
            MainTimer();
        }

        if(isClickBattleButton)
        {
            OnClickBattleButton();
        }

    }
    public void OnBattleLayout()
    {
        battleLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 1);
        VoiceActive(0);
    }
    public void OffBattleLayout()
    {
        battleLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 0);
    }
    public void OnControlLayout()
    {
        controlLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 1);
        VoiceActive(1);
    }
    public void OffControlLayout()
    {
        controlLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 0);
    }
    public void OnCommandLayout()
    {
        commandLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 1);
        VoiceActive(2);
    }
    public void OffCommandLayout()
    {
        commandLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 0);
    }
    public void OnExitLayout()
    {
        exitLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 1);
        VoiceActive(3);
    }
    public void OffExitLayout()
    {
        exitLayOut.GetComponent<Image>().color = new Color(1, 0.6f, 0, 0);
    }

    void MainTimer()
    {
        
        if(isCurrTime)
        {
            currTime += Time.deltaTime;
        }
            
    }

    public void OnClickBattleButton ()
    {
        //print(11111);
        isClickBattleButton = true;
        isCurrTime = true;
        battleLayOut.GetComponent<Image>().color = Color.green;
        
        if(currTime > 1)
        {
           
            isCurrTime = false;
            currTime = 0;
            isClickBattleButton = false;
            SceneManager.LoadScene("Dong");
        }
        
    }

    public void OnClickExitButton()
    {
        //print(11111);
        exitLayOut.GetComponent<Image>().color = Color.green;
        //앱종료
        Application.Quit();
        //에디터에서 플레이 종료
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void OnClickControl()
    {
        controlLayOut.GetComponent<Image>().color = Color.green;

        SceneManager.LoadScene("Control");
    }

    public void OnClickCommand()
    {
        commandLayOut.GetComponent<Image>().color = Color.green;

        SceneManager.LoadScene("Command");

       
    }

    void VoiceActive(int num)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(menuVoice[num]);
    }

}
