using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UI.Image;

public class MarioPlayerMove : MonoBehaviour
{
    //�̵��� ���� ���� ����
    int jumpForce = 23;
    float speed = 17;
    int whatDir = 1;

    //��ġ ����

    public GameObject attackPosition;

    public float pushDir = 0.05f;

    GameObject player;

    //Player�� ������ �ٵ�
    Rigidbody rb;

    public int enHP = 0;

    //Enemy ������ �ٵ� ȣ��
    private Rigidbody enemyrb;

    public Camera mainCamera;

    Animator animator;
    Animator enAnimator;

    //�ִϸ��̼� ���� Ʈ����
    public bool actionTrigger = false;
    public bool blocking = false;
    public bool isEnBlocking = false;

    public int jumpDouble = 0;

    //���� Y ȸ�� �� �˾Ƴ��� ����. �� �� ������ ���ؼ� ����
    public float h = 0;

    CameraController cameraController;

    AudioSource runAudio;

    Vector3 dirH;

    GameObject enemy;

    GameObject enemyGun;

    bool coolDown = false;

    bool powerOverwhelming = false;

    Dictionary<string, bool> coolDowns = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
        player = UiManager.instance.player;
        enemy = UiManager.instance.enemy;

        cameraController = Camera.main.GetComponent<CameraController>();

        animator = GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody>();
        runAudio = GetComponent<AudioSource>();

        enemyGun = GameObject.Find("EnemyGun");

        //���� �ִϸ����� �ҷ���
        enAnimator = enemyGun.GetComponent<Animator>();

        coolDowns["SlideAttack"] = false;
        coolDowns["Block"] = false;
        coolDowns["JumpAttack"] = false;
        coolDowns["JumpAttack2"] = false;
    }

    void FixedUpdate()
    {

        if (rb.velocity.y < 2)
        { // ���� ���� ���� ����
            rb.velocity += Vector3.down * 4f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�¿� �̵� (��� �յ� �̵�)
        h = Input.GetAxis("Horizontal");

        //���ư����� ���� ����
        dirH = Vector3.right * h * speed;

        enHP = UiManager.instance.enHP;

        if(UiManager.instance.gameEnd == false)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

            // ���� �Է��� �Ǿ� ���� ���� ���� ����/���/�ʻ�� �� ����
            if (!Input.GetButton("Up") && jumpDouble == 0 && animator.GetBool("Jump") == false)
            {
                //�����̵� ���� 1
                if (Input.GetButton("Down") && Input.GetButtonDown("Fire1") && coolDowns["SlideAttack"] == false)
                {
                    animator.SetTrigger("SlideAttack");

                    coolDowns["SlideAttack"] = true;

                    rb.AddForce(new Vector3(whatDir * 35, 0, 0), ForceMode.VelocityChange);

                    StartCoroutine(CoolDown("SlideAttack", 1.5f));
                }
                //�� ����
                else if (Input.GetButtonDown("Fire1"))
                {
                    //�ִϸ��̼� ȣ��
                    animator.SetTrigger("LightAttack");
                    animator.ResetTrigger("StrongAttack");

                    StopAction();
                }

                //�� ����
                if (Input.GetButtonDown("Fire2"))
                {
                    //�ִϸ��̼� ȣ��
                    animator.SetTrigger("StrongAttack");
                    animator.ResetTrigger("LightAttack");

                    StopAction();
                }

                //�ʻ��
                if (Input.GetButtonDown("Fire4") && UiManager.instance.chGauge >= 1 && actionTrigger == false)
                {
                    StopAction();

                    //�ִϸ��̼� ȣ��
                    animator.SetTrigger("Ulti");

                    SoundManager.instance.StartUltiAudio();
                }
            }

            //���
            if (Input.GetButtonDown("Fire3") && blocking == false && coolDowns["Block"] == false)
            {
                animator.SetTrigger("Block");

                coolDowns["Block"] = true;

                //�ִϸ��̼� ȣ��
                StartCoroutine(CoolDown("Block", 2.0f));

                StopAction();
            }

            //����� ���� ���
            if (Input.GetKeyDown(KeyCode.Z))
            {
                UiManager.instance.HpRefresh(0, -50);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                UiManager.instance.HpRefresh(0, +50);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                UiManager.instance.GaugeRefresh(1, 1);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                UiManager.instance.HpRefresh(+50, 0);
            }

            if(Input.GetKeyDown(KeyCode.B))
            {
                if(powerOverwhelming == false)
                {
                    powerOverwhelming = true;
                    Debug.Log("���� ġƮON" + powerOverwhelming);
                }
                else if (powerOverwhelming == true)
                {
                    powerOverwhelming = false;
                    Debug.Log("���� ġƮOFF" + powerOverwhelming);
                }
            }
            //����� �������

            //�̵� ����
            if (!IsAnimationPlaying(animator, "Damaged") && blocking == false)
            {
                //�¿� �̵� �� 1 or -1 �� ���� actionTrigger�� �۵������� ���� �� (����, ���, �ʻ��)
                if (h != 0 && actionTrigger == false && animator.GetBool("ActionCheck") == false)
                {
                    //�̵� ������ Ȯ���ؼ� �� �� ��ȯ
                    if (h > 0)
                    {
                        player.transform.rotation = Quaternion.Euler(0, 0, 0);
                        whatDir = 1;
                    }
                    else if (h < 0)
                    {
                        player.transform.rotation = Quaternion.Euler(0, 180, 0);
                        whatDir = -1;
                    }

                    //velocity�� time.deltatime ���ص� �ȴ���.
                    //���� �̵� ���� �Է� �� ���� �޴� ������ ���ֱ� ���ؼ� y�� z�� ���� �������� ���� �߰��� ����
                    //�̰� ������ �̵� �߿� �����ϸ� 0.1 ���̷� �ٰ�, ���� �� �̵��ϸ� ����亸��.
                    if (jumpDouble == 0 || Input.GetAxis("Horizontal") != 0)
                    {
                        rb.velocity = new Vector3(dirH.x, rb.velocity.y, rb.velocity.z);

                        if (!runAudio.isPlaying && jumpDouble == 0)
                        {
                            runAudio.Play();
                        }
                    }
                    else
                    {
                        //���� �� �̵� �ӵ� ����
                        rb.velocity = new Vector3(dirH.x / 2, rb.velocity.y, rb.velocity.z);
                    }

                    //�޸��� ��ƼŬ�� ������� �ƴ� ��
                    if (!EffectManager.instance.sysRun.isPlaying)
                    {
                        EffectManager.instance.RunEffectPlay();
                    }
                }
                //�¿� �Է��� ���� �� �޸��� ��ƼŬ ����
                if (h == 0)
                {
                    EffectManager.instance.RunEffectStop();
                }
                //����, �̵��� �޸��� ȿ���� ����
                if (h == 0 || jumpDouble != 0 && actionTrigger == false)
                {
                    runAudio.Stop();
                }

                //�ִϸ����Ϳ��� Idle, Run ��ȯ�� ���� �ڵ�, Speed�� 0.1 ���ϰ� �Ǹ� Idle�� ��ȯ.
                //Mathf.Abs�� �־��� ���� ���밪�� ��ȯ�ϴ� ����Ƽ�� ���� �Լ��Դϴ�. h * speed ���� ����� ������ ������� ���밪�� ����Ͽ� Speed �Ķ���Ϳ� �Ҵ��մϴ�.
                animator.SetFloat("Speed", Mathf.Abs(h * speed));
            }

            //jump Ű ������ �� �۵�
            if (Input.GetButtonDown("Jump") && blocking == false)
            {
                //���� Ƚ�� 0, 1�̸� �۵�
                if (jumpDouble < 2)
                {
                    animator.ResetTrigger("StrongAttack");
                    animator.ResetTrigger("LightAttack");
                    animator.SetBool("ActionCheck", false);
                    //1�� ����
                    if (jumpDouble == 0)
                    {
                        //ù �������� �ִϸ��̼� �۵�
                        animator.SetBool("Jump", true);
                        animator.SetTrigger("Jump2");

                        //Y �������� AddForce
                        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

                        SoundManager.instance.JumpAudio();
                    }

                    //2�� ����
                    else if (jumpDouble == 1)
                    {
                        //2�� ������ ���� ���̷� ����
                        rb.AddForce(new Vector3(0, jumpForce * 0.5f, 0), ForceMode.Impulse);

                        animator.SetTrigger("JumpDouble");

                        SoundManager.instance.JumpAudio();
                    }
                    //���� Ƚ�� ī��Ʈ
                    jumpDouble++;
                }
            }

            //���� ���� 1
            if (jumpDouble < 3 && Input.GetButton("Up") && Input.GetButtonDown("Fire1") && blocking == false && coolDowns["JumpAttack"] == false)
            {
                animator.SetTrigger("JumpAttack");

                animator.SetBool("Jump", true);

                coolDowns["JumpAttack"] = true;

                jumpDouble += 3;

                rb.AddForce(new Vector3(whatDir * 10, jumpForce * 0.9f, 0), ForceMode.Impulse);

                StartCoroutine(CoolDown("JumpAttack", 1.5f));
            }

            //���� ���� 2
            if (jumpDouble < 3 && Input.GetButton("Up") && Input.GetButtonDown("Fire2") && blocking == false && coolDowns["JumpAttack2"] == false)
            {
                animator.SetTrigger("JumpAttack2");

                animator.SetBool("Jump", true);

                coolDowns["JumpAttack2"] = true;

                jumpDouble += 3;

                rb.AddForce(new Vector3(0, jumpForce * 0.8f, 0), ForceMode.Impulse);

                StartCoroutine(CoolDown("JumpAttack2", 1.5f));
            }
        }
        else
        {
            h = 0;

            dirH = Vector3.right * 0;

            runAudio.Stop();
        }
    }

    //������ ó�� ���� ���� (damage�� ������ ���ط�, distance�� ���� ��Ÿ�, liftForce�� ��(y)�� ���� ��, pushForce�� x�� ��ġ�� ��)
    private void AttackStep(int damage, float distance, float liftForce, float pushForce)
    {
        // ĳ������ ��ġ�� Forward ���� ����
        Vector3 origin = attackPosition.transform.position;
        Vector3 direction = attackPosition.transform.right;

        // �ݰ� ����
        float attackRadius = 1.0f;

        //Ÿ���� ���
        SoundManager.instance.AttackAudio();

        //���� ������̸� ó�� ����
        if (isEnBlocking == true) return;

        //���� ����ĳ��Ʈ �߻�
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(origin, attackRadius, direction, distance);

        for(int i = 0; i<hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Enemy"))
            {
                // �浹�� ������Ʈ�� ����
                GameObject enemy = hits[i].transform.gameObject;

                //����� ������Ʈ�� ������ �±� Ȯ��
                if (enemy.tag == "Enemy")
                {
                    //������ ����(�� ����)
                    if (UiManager.instance.enHP < 300)
                    {
                        UiManager.instance.HpRefresh(0, damage);
                    }
                    else
                    {
                        UiManager.instance.enHP += 0;
                    }

                    Vector3 newPosition = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1);

                    SoundManager.instance.HitAudio();

                    EffectManager.instance.HitEffect(enemy.transform.position);

                    //enHP�� ������ ���ط�, pushDir�� �⺻ ���ݷ�, pushForce�� ��ų���� ���� Ʋ��.
                    //���� ��������, ���� ����, ���ĳ��� ������ ���� �з����� �Ÿ� ������
                    Vector3 dir = direction.normalized * UiManager.instance.enHP * pushForce * pushDir;

                    //�ణ ����(y)���� ���� ȿ��
                    dir.y = UiManager.instance.enHP * liftForce * pushDir;

                    //���� ���� ���� ������ �ٵ� �ҷ���
                    enemyrb = enemy.GetComponent<Rigidbody>();

                    //���� �ִϸ��̼� ���
                    enAnimator.SetTrigger("Hurt");

                    //���� dir�� ���缭 ���� ���ĳ�
                    enemyrb.AddForce(dir, ForceMode.Impulse);

                    //�˹�� ���� ����Ʈ ����
                    if (dir.x > 12 || dir.y > 7)
                    {
                        EffectManager.instance.KnockEStart();
                    }

                    if(dir.x > 20 || dir.y > 26)
                    {
                        UiManager.instance.FinishEffect(player.transform);
                    }


                    if(damage < 100)
                    {
                        //�ñر� �����ϰ� ������ ���
                        UiManager.instance.GaugeRefresh(1 * damage * 0.01f, 0.5f * damage * 0.01f);
                    }
                    
                    cameraController.CameraShake();

                    break;
                }
            }
        }
        // Debug������ SphereCast�� Scene �信�� �ð������� ǥ��
        Debug.DrawRay(origin, direction * distance, Color.red, 5.0f);
    }

    public void IsLightAttack()
    {
        AttackStep(5, 2.0f, 0.2f, 0.25f);
    }

    public void IsStrongAttack()
    {
        AttackStep(9, 3.0f, 0.7f, 0.55f);
    }

    public void IsRangeAttack()
    {
        EffectManager.instance.RangeEffect();
        AttackStep(7, 8.0f, 1.8f, 0.45f);
    }
    public void IsJumpAttack()
    {
        AttackStep(2, 2.0f, 0.4f, 0.25f);
    }

    public void IsJumpAttack2()
    {
        AttackStep(9, 2.5f, 0.5f, 2.5f);
    }

    public void IsLandingAttack()
    {
        AttackStep(4, 2.0f, 2.5f, 0.1f);
    }

    public void IsSlideAttack()
    {
        AttackStep(7, 2.0f, 1.0f, 0.1f);
    }

    public void IsUltiStart()
    {
        //�ñر�
        AttackStep(100, 14.0f, 2.0f, 2.5f);

        SoundManager.instance.UltiAudio();

        EffectManager.instance.UltiEffect();

        cameraController.UltiCameraDone();

        //�ñر� ������ �ʱ�ȭ
        UiManager.instance.GaugeRefresh(-1, 0);
    }

    public void IsUltiCameraSetUp()
    {
        cameraController.UltiCameraStart();
    }

    //�ִϸ��̼� �̺�Ʈ���� ���� ���۰� ���� �����ϴ� �Լ�. �ִϸ��̼� ���۰� ���� �Լ��� ȣ�� �ϴ� �̺�Ʈ�� �����Ͽ���.
    //actionTrigger�� true�� �Ǹ� ���� �� �ִϸ��̼� ��ũ��Ʈ�� ������ ��.
    public void IsAnimationStart()
    {
        //�ִϸ��̼� ����
        actionTrigger = true;

        animator.SetBool("ActionCheck", true);
    }

    public void IsAnimationDone()
    {
        //�ִϸ��̼� ����
        actionTrigger = false;

        animator.SetBool("ActionCheck", false);
    }

    public void IsBlockStart()
    {
        //��� ����
        IsAnimationStart();

        SoundManager.instance.HitAudio();

        EffectManager.instance.BlockEffect();

        blocking = true;

        animator.SetBool("ActionCheck", true);
    }

    public void IsBlockDone()
    {
        //��� ����
        IsAnimationDone();

        blocking = false;

        animator.SetBool("ActionCheck", false);
    }

    public void Damaged(int damageValue, float pushForce)
    {
        if(UiManager.instance.gameEnd == false)
        {
            EffectManager.instance.HitEffect(transform.position);

            SoundManager.instance.DamagedAudio();

            if (blocking == false && powerOverwhelming == false)
            {
                float pushDir = 0.1f;

                if (jumpDouble == 1)
                {
                    jumpDouble += 2;
                }

                h = 0;

                StopAction();

                UiManager.instance.HpRefresh(damageValue, 0);

                //�ǰ�
                animator.SetTrigger("Damaged");

                // ���� ���ݹ��� right �޾ƿ���.
                Vector3 direction = enemy.transform.right;

                //����ũ�� = ���ǰ��ݹ��� * ����ü�� * ĳ���� �⺻�� * ����Ÿ�� ������
                Vector3 dir = direction.normalized * UiManager.instance.chHP * pushDir * pushForce;

                rb.AddForce(dir, ForceMode.Impulse);

                cameraController.CameraShake();

                //������ ���
                UiManager.instance.GaugeRefresh(0.05f, 0.05f);

                float knockDir = Vector3.Distance(player.transform.position, dir);

                //�˹�� ���� ����Ʈ ����
                if (knockDir > 15)
                {
                    EffectManager.instance.KnockPStart();
                }
            }
        }
    }

    public void FloorCheck()
    {
        SoundManager.instance.FloorAudio();

        //���� �ִϸ��̼� ���
        animator.SetBool("Jump", false);

        animator.ResetTrigger("JumpAttack");
        animator.ResetTrigger("JumpDouble");

        //�׼� ����
        actionTrigger = false;

        if(!IsAnimationPlaying(animator, "Block"))
        {
            blocking = false;
        }

        if (jumpDouble == 2)
        {
            EffectManager.instance.LandingEffect();

            IsLandingAttack();
        }

        //���� Ƚ�� �ʱ�ȭ
        jumpDouble = 0;
    }

    void StopAction()
    {
        animator.SetBool("ActionCheck", true);
    }

    //�ִϸ��̼� üũ �Լ�
    bool IsAnimationPlaying(Animator anim, string state)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(state) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }


    IEnumerator CoolDown(string name, float time)
    {
        yield return new WaitForSeconds(time);

        coolDowns[name] = false;
    }
}