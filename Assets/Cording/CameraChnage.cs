using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChnage : MonoBehaviour
{
    Camera mainCamera;
    Camera secondaryCamera;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("��� ������ ���÷��� ��: " + Display.displays.Length);
        //mainCamera.targetDisplay = 0;

        // �� ��° ���÷��̰� ������ Ȱ��ȭ�ϰ� ī�޶� �Ҵ�
        if (Display.displays.Length > 1)
        {
            secondaryCamera.targetDisplay = 1;
            Display.displays[1].Activate();
        }

        /*for (int i = 0; i < 2; i++)
        {
            Display.displays[i].Activate();
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToDisplay(int displayIndex)
    {
        if (displayIndex >= 0 && displayIndex < Display.displays.Length)
        {
            mainCamera.targetDisplay = displayIndex;
            Display.displays[displayIndex].Activate();
        }
        else
        {
            Debug.LogWarning("�߸��� ���÷��� �ε���: " + displayIndex);
        }
    }




}
