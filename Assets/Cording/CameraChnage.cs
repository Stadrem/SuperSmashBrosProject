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
        Debug.Log("사용 가능한 디스플레이 수: " + Display.displays.Length);
        //mainCamera.targetDisplay = 0;

        // 두 번째 디스플레이가 있으면 활성화하고 카메라 할당
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
            Debug.LogWarning("잘못된 디스플레이 인덱스: " + displayIndex);
        }
    }




}
