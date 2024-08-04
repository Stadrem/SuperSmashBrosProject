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
                // �� ĳ���� ������ �߰� ������ ����մϴ�.
                middlePoint = (character1.position + character2.position) / 2.0f;

                // �� ĳ���� ������ �Ÿ��� ����մϴ�.
                float distance = Vector3.Distance(character1.position, character2.position);

                // �Ÿ��� ���� ī�޶��� Z�� ���� �����մϴ�.
                cameraDistance = Mathf.Lerp(minDistance, maxDistance, distance / maxDistance);
                /*Mathf.Lerp�� ���� ����(linear interpolation)�� �����ϴ� ����Ƽ�� ���� �Լ��Դϴ�.
                �� �Լ��� �� �� ���̸� Ư�� ������ ���� �����Ͽ� �߰� ���� ��ȯ�մϴ�.
                ����: Mathf.Lerp(a, b, t)
                a: ���� ��(���⼭�� minDistance)
                b: �� ��(���⼭�� maxDistance)
                t: 0�� 1 ������ ������, ���� ������ ��Ÿ���ϴ�.
                */
                //�� �ڵ�� distance�� ���� ī�޶��� Z�� ���� �����Ͽ� minDistance�� maxDistance ������ ���� ���� �����մϴ�. �̸� ���� ī�޶� ������ ���� ������ �Ÿ� ������ ���� �����̰� �� �� �ֽ��ϴ�.

                //����
                middlePoint.y += 2;

                // ī�޶��� ��ġ�� �߰� ������ ������ �Ÿ��� �°� �����մϴ�.
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