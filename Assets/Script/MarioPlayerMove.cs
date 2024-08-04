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
    //이동과 점프 변수 선언
    int jumpForce = 23;
    float speed = 17;
    int whatDir = 1;

    //위치 관련

    public GameObject attackPosition;

    public float pushDir = 0.05f;

    GameObject player;

    //Player의 리지드 바디
    Rigidbody rb;

    public int enHP = 0;

    //Enemy 리지드 바디 호출
    private Rigidbody enemyrb;

    public Camera mainCamera;

    Animator animator;
    Animator enAnimator;

    //애니메이션 관련 트리거
    public bool actionTrigger = false;
    public bool blocking = false;
    public bool isEnBlocking = false;

    public int jumpDouble = 0;

    //현재 Y 회전 축 알아내는 변수. 앞 뒤 구분을 위해서 존재
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

        //적의 애니메이터 불러옴
        enAnimator = enemyGun.GetComponent<Animator>();

        coolDowns["SlideAttack"] = false;
        coolDowns["Block"] = false;
        coolDowns["JumpAttack"] = false;
        coolDowns["JumpAttack2"] = false;
    }

    void FixedUpdate()
    {

        if (rb.velocity.y < 2)
        { // 낙하 중일 때만 적용
            rb.velocity += Vector3.down * 4f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //좌우 이동 (사실 앞뒤 이동)
        h = Input.GetAxis("Horizontal");

        //나아가야할 방향 계산식
        dirH = Vector3.right * h * speed;

        enHP = UiManager.instance.enHP;

        if(UiManager.instance.gameEnd == false)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

            // 점프 입력이 되어 있지 않을 때만 공격/방어/필살기 등 가능
            if (!Input.GetButton("Up") && jumpDouble == 0 && animator.GetBool("Jump") == false)
            {
                //슬라이딩 공격 1
                if (Input.GetButton("Down") && Input.GetButtonDown("Fire1") && coolDowns["SlideAttack"] == false)
                {
                    animator.SetTrigger("SlideAttack");

                    coolDowns["SlideAttack"] = true;

                    rb.AddForce(new Vector3(whatDir * 35, 0, 0), ForceMode.VelocityChange);

                    StartCoroutine(CoolDown("SlideAttack", 1.5f));
                }
                //약 공격
                else if (Input.GetButtonDown("Fire1"))
                {
                    //애니메이션 호출
                    animator.SetTrigger("LightAttack");
                    animator.ResetTrigger("StrongAttack");

                    StopAction();
                }

                //강 공격
                if (Input.GetButtonDown("Fire2"))
                {
                    //애니메이션 호출
                    animator.SetTrigger("StrongAttack");
                    animator.ResetTrigger("LightAttack");

                    StopAction();
                }

                //필살기
                if (Input.GetButtonDown("Fire4") && UiManager.instance.chGauge >= 1 && actionTrigger == false)
                {
                    StopAction();

                    //애니메이션 호출
                    animator.SetTrigger("Ulti");

                    SoundManager.instance.StartUltiAudio();
                }
            }

            //방어
            if (Input.GetButtonDown("Fire3") && blocking == false && coolDowns["Block"] == false)
            {
                animator.SetTrigger("Block");

                coolDowns["Block"] = true;

                //애니메이션 호출
                StartCoroutine(CoolDown("Block", 2.0f));

                StopAction();
            }

            //디버그 전용 기능
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
                    Debug.Log("무적 치트ON" + powerOverwhelming);
                }
                else if (powerOverwhelming == true)
                {
                    powerOverwhelming = false;
                    Debug.Log("무적 치트OFF" + powerOverwhelming);
                }
            }
            //디버그 여기까지

            //이동 로직
            if (!IsAnimationPlaying(animator, "Damaged") && blocking == false)
            {
                //좌우 이동 중 1 or -1 일 때와 actionTrigger가 작동중이지 않을 때 (공격, 방어, 필살기)
                if (h != 0 && actionTrigger == false && animator.GetBool("ActionCheck") == false)
                {
                    //이동 방향을 확인해서 앞 뒤 전환
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

                    //velocity는 time.deltatime 안해도 된다함.
                    //점프 이동 동시 입력 시 방해 받는 문제는 없애기 위해서 y와 z에 현재 가해지는 힘을 추가로 넣음
                    //이거 없으면 이동 중에 점프하면 0.1 높이로 뛰고, 점프 중 이동하면 허공답보함.
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
                        //점프 중 이동 속도 절반
                        rb.velocity = new Vector3(dirH.x / 2, rb.velocity.y, rb.velocity.z);
                    }

                    //달리기 파티클이 재생중이 아닐 시
                    if (!EffectManager.instance.sysRun.isPlaying)
                    {
                        EffectManager.instance.RunEffectPlay();
                    }
                }
                //좌우 입력이 없을 시 달리기 파티클 중지
                if (h == 0)
                {
                    EffectManager.instance.RunEffectStop();
                }
                //정지, 이동시 달리기 효과음 중지
                if (h == 0 || jumpDouble != 0 && actionTrigger == false)
                {
                    runAudio.Stop();
                }

                //애니메이터에서 Idle, Run 전환을 위한 코드, Speed가 0.1 이하가 되면 Idle로 전환.
                //Mathf.Abs는 주어진 값의 절대값을 반환하는 유니티의 수학 함수입니다. h * speed 값이 양수든 음수든 상관없이 절대값을 사용하여 Speed 파라미터에 할당합니다.
                animator.SetFloat("Speed", Mathf.Abs(h * speed));
            }

            //jump 키 눌렀을 때 작동
            if (Input.GetButtonDown("Jump") && blocking == false)
            {
                //점프 횟수 0, 1이면 작동
                if (jumpDouble < 2)
                {
                    animator.ResetTrigger("StrongAttack");
                    animator.ResetTrigger("LightAttack");
                    animator.SetBool("ActionCheck", false);
                    //1단 점프
                    if (jumpDouble == 0)
                    {
                        //첫 점프에서 애니메이션 작동
                        animator.SetBool("Jump", true);
                        animator.SetTrigger("Jump2");

                        //Y 방향으로 AddForce
                        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

                        SoundManager.instance.JumpAudio();
                    }

                    //2단 점프
                    else if (jumpDouble == 1)
                    {
                        //2단 점프는 절반 높이로 설정
                        rb.AddForce(new Vector3(0, jumpForce * 0.5f, 0), ForceMode.Impulse);

                        animator.SetTrigger("JumpDouble");

                        SoundManager.instance.JumpAudio();
                    }
                    //점프 횟수 카운트
                    jumpDouble++;
                }
            }

            //점프 공격 1
            if (jumpDouble < 3 && Input.GetButton("Up") && Input.GetButtonDown("Fire1") && blocking == false && coolDowns["JumpAttack"] == false)
            {
                animator.SetTrigger("JumpAttack");

                animator.SetBool("Jump", true);

                coolDowns["JumpAttack"] = true;

                jumpDouble += 3;

                rb.AddForce(new Vector3(whatDir * 10, jumpForce * 0.9f, 0), ForceMode.Impulse);

                StartCoroutine(CoolDown("JumpAttack", 1.5f));
            }

            //점프 공격 2
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

    //데미지 처리 공통 과정 (damage는 입히는 피해량, distance는 공격 사거리, liftForce는 위(y)로 띄우는 힘, pushForce는 x로 밀치는 힘)
    private void AttackStep(int damage, float distance, float liftForce, float pushForce)
    {
        // 캐릭터의 위치와 Forward 방향 설정
        Vector3 origin = attackPosition.transform.position;
        Vector3 direction = attackPosition.transform.right;

        // 반경 설정
        float attackRadius = 1.0f;

        //타격음 재생
        SoundManager.instance.AttackAudio();

        //적이 방어중이면 처리 종료
        if (isEnBlocking == true) return;

        //다중 레이캐스트 발사
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(origin, attackRadius, direction, distance);

        for(int i = 0; i<hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Enemy"))
            {
                // 충돌한 오브젝트를 저장
                GameObject enemy = hits[i].transform.gameObject;

                //저장된 오브젝트가 적인지 태그 확인
                if (enemy.tag == "Enemy")
                {
                    //데미지 누적(약 공격)
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

                    //enHP는 누적된 피해량, pushDir은 기본 공격력, pushForce는 스킬마다 값이 틀림.
                    //정면 방향으로, 누적 피해, 밀쳐내기 강도에 따라서 밀려나는 거리 정해짐
                    Vector3 dir = direction.normalized * UiManager.instance.enHP * pushForce * pushDir;

                    //약간 공중(y)으로 띄우는 효과
                    dir.y = UiManager.instance.enHP * liftForce * pushDir;

                    //공격 맞은 적의 리지드 바디 불러옴
                    enemyrb = enemy.GetComponent<Rigidbody>();

                    //적의 애니메이션 재생
                    enAnimator.SetTrigger("Hurt");

                    //계산된 dir에 맞춰서 적을 밀쳐냄
                    enemyrb.AddForce(dir, ForceMode.Impulse);

                    //넉백시 구름 이펙트 생성
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
                        //궁극기 제외하고 게이지 계산
                        UiManager.instance.GaugeRefresh(1 * damage * 0.01f, 0.5f * damage * 0.01f);
                    }
                    
                    cameraController.CameraShake();

                    break;
                }
            }
        }
        // Debug용으로 SphereCast를 Scene 뷰에서 시각적으로 표시
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
        //궁극기
        AttackStep(100, 14.0f, 2.0f, 2.5f);

        SoundManager.instance.UltiAudio();

        EffectManager.instance.UltiEffect();

        cameraController.UltiCameraDone();

        //궁극기 게이지 초기화
        UiManager.instance.GaugeRefresh(-1, 0);
    }

    public void IsUltiCameraSetUp()
    {
        cameraController.UltiCameraStart();
    }

    //애니메이션 이벤트에서 공격 시작과 끝을 구분하는 함수. 애니메이션 시작과 끝에 함수를 호출 하는 이벤트를 삽입하였음.
    //actionTrigger가 true가 되면 연관 된 애니메이션 스크립트에 영향을 줌.
    public void IsAnimationStart()
    {
        //애니메이션 시작
        actionTrigger = true;

        animator.SetBool("ActionCheck", true);
    }

    public void IsAnimationDone()
    {
        //애니메이션 종료
        actionTrigger = false;

        animator.SetBool("ActionCheck", false);
    }

    public void IsBlockStart()
    {
        //방어 시작
        IsAnimationStart();

        SoundManager.instance.HitAudio();

        EffectManager.instance.BlockEffect();

        blocking = true;

        animator.SetBool("ActionCheck", true);
    }

    public void IsBlockDone()
    {
        //방어 종료
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

                //피격
                animator.SetTrigger("Damaged");

                // 적의 공격방향 right 받아오자.
                Vector3 direction = enemy.transform.right;

                //방향크기 = 적의공격방향 * 누적체력 * 캐릭터 기본값 * 공격타입 보정값
                Vector3 dir = direction.normalized * UiManager.instance.chHP * pushDir * pushForce;

                rb.AddForce(dir, ForceMode.Impulse);

                cameraController.CameraShake();

                //게이지 계산
                UiManager.instance.GaugeRefresh(0.05f, 0.05f);

                float knockDir = Vector3.Distance(player.transform.position, dir);

                //넉백시 구름 이펙트 생성
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

        //점프 애니메이션 재생
        animator.SetBool("Jump", false);

        animator.ResetTrigger("JumpAttack");
        animator.ResetTrigger("JumpDouble");

        //액션 종료
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

        //점프 횟수 초기화
        jumpDouble = 0;
    }

    void StopAction()
    {
        animator.SetBool("ActionCheck", true);
    }

    //애니메이션 체크 함수
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