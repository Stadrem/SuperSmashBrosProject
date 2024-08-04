using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    Transform character1;
    Transform character2;
    public float minDistance = 5.0f;
    public float maxDistance = 10.0f;
    public float cameraDistance = 5.0f;
    private Vector3 middlePoint;
    public bool cameraTwoFocus = true;
    public Camera subCamera;
    GameObject player;
    GameObject enemy;

    float shakeAmount = 0.07f;
    float shakeTime = 0.0f;
    bool shake = false;

    void Start()
    {
        player = GameObject.Find("Player");
        character1 = player.transform;

        enemy = GameObject.Find("Enemy");
        character2 = enemy.transform;
    }
    void Update()
    {
        if(UiManager.instance.gameEnd == false)
        {
            if (cameraTwoFocus == true)
            {
                // 두 캐릭터 사이의 중간 지점을 계산합니다.
                middlePoint = (character1.position + character2.position) / 2.0f;

                // 두 캐릭터 사이의 거리를 계산합니다.
                float distance = Vector3.Distance(character1.position, character2.position);

                // 거리에 따라 카메라의 Z축 값을 조정합니다.
                cameraDistance = Mathf.Lerp(minDistance, maxDistance, distance / maxDistance);
                /*Mathf.Lerp는 선형 보간(linear interpolation)을 수행하는 유니티의 수학 함수입니다.
                이 함수는 두 값 사이를 특정 비율에 따라 보간하여 중간 값을 반환합니다.
                형식: Mathf.Lerp(a, b, t)
                a: 시작 값(여기서는 minDistance)
                b: 끝 값(여기서는 maxDistance)
                t: 0과 1 사이의 값으로, 보간 비율을 나타냅니다.
                */
                //이 코드는 distance에 따라 카메라의 Z축 값을 조정하여 minDistance와 maxDistance 사이의 값을 선형 보간합니다. 이를 통해 카메라가 일정한 범위 내에서 거리 비율에 따라 움직이게 할 수 있습니다.

                //보정
                middlePoint.y += 2;

                // 카메라의 위치를 중간 지점과 설정된 거리에 맞게 조정합니다.
                transform.position = new Vector3(middlePoint.x, middlePoint.y, -cameraDistance);
            }

            if (shake == true)
            {
                if (shakeTime < 0.08f)
                {
                    cameraTwoFocus = false;
                    transform.position += Random.insideUnitSphere * shakeAmount;
                    shakeTime += Time.deltaTime;
                }
                else
                {
                    shakeTime = 0.0f;
                    cameraTwoFocus = true;
                    shake = false;
                }
            }
        }
    }

    public void UltiCameraStart()
    {
        cameraTwoFocus = false;

        Vector3 worldPosition = subCamera.transform.position;

        transform.position = worldPosition;

        transform.LookAt(character1);

        transform.position = new Vector3(transform.position.x - 1, transform.position.y + 1, transform.position.z + 1);
    }

    public void UltiCameraDone()
    {
        cameraTwoFocus = true;

        transform.rotation = Quaternion.Euler(7.7f, 0, 0);
    }

    public void CameraShake()
    {
        shake = true;
    }
}