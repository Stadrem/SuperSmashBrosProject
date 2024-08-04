using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommandUiBack : MonoBehaviour
{
    public Image backImage;

    public void OnClickBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnOver()
    {
        backImage.color = Color.gray;
    }

    public void OnExit()
    {
        backImage.color = Color.black;
    }
}
