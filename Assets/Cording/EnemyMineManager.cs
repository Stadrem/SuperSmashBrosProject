using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMineManager : MonoBehaviour
{
    public GameObject mineFactory;
    public ParticleSystem mineSpawnEffect;

    public float spawnTime = 4;
    public bool isMineManagerUpdate = true; 
    public bool isMineActive = false;
     

    EnemyMove enemyMove;

    float currTime = 0;
    
    GameObject mine;
    GameObject enemy;
    GameObject enemyAnim;
    Vector3 mineUpPos;
    Quaternion minRot;

    
    // Start is called before the first frame update
    void Start()
    {
       enemy = GameObject.Find("Enemy");
       enemyAnim = GameObject.Find("EnemyGun");
       mineUpPos = new Vector3(0, 0, 0.5f);
       minRot = new Quaternion(0, 90, 0, 0);
       enemyMove = enemy.GetComponent<EnemyMove>();
     

    }

    // Update is called once per frame
    void Update()
    {
        if (UiManager.instance.gameEnd) return;
        if (!isMineManagerUpdate) return;
        currTime += Time.deltaTime;
      
        if (currTime > spawnTime)
        {
            enemyMove.isEnemyMove = false;
            isMineActive = true;
        }
            
        if (isMineActive)
        {
            Animator animator;
            animator = enemyAnim.GetComponent<Animator>();
            animator.SetTrigger("Knee");

            ParticleSystem mineSpawnEff = Instantiate(mineSpawnEffect);
            mineSpawnEff.transform.position = transform.position;
            Destroy(mineSpawnEff,2);

            mine = Instantiate(mineFactory);
            mine.transform.position = transform.position;
            mine.transform.rotation = enemy.transform.rotation * minRot;

            isMineActive = false; 
            currTime = 0;
         }
            //enemyMove.isEnemyMove = true;




        
        
    }
}
