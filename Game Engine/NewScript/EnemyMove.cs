using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    //定义敌人四种状态
    public const int STATE_STAND = 0;   //站立状态
    public const int STATE_WALK = 1;    //行走状态
    public const int STATE_RUN = 2;     //奔跑状态
    public const int STATE_PAUSE = 3;   //暂停状态
    public static bool STATE_PAUSE_SETTING = false; //为true时控制敌人不许动
    public const int STATE_ATTACK = 4;  //攻击状态
    public const int STATE_BEINGATTACKED = 5;   //被攻击状态


    //记录敌人当前状态
    int enemyState = 0;
    //获取玩家对象
    GameObject hero;

    //记录上一次敌人思考时间
    float lastUptime;
    //思考的时间间隔
    public const int AI_THINK_TIME = 2;
    //敌人视野范围
    public const int AI_VIEW_DISTANCE = 30;
    //敌人攻击范围
    public const int AI_ATTACK_DISTANCE = 5;
    //敌人攻击力
    public int EnemyAttack = 10;
    //敌人仇恨
    bool isHatred = false;
    //此bool为true时表示物体正在被打
    bool isHurted = false;
    int hurtedFrame = 0;
    GameObject blood;

    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.Find("Hero");
        enemyState = STATE_STAND;
        blood = GameObject.Find("Blood");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hurtedFrame > 0) hurtedFrame--;

        if (EnemyMove.STATE_PAUSE_SETTING)
        {
            this.enemyState = STATE_PAUSE;
        }
        else {
            if (Vector3.Distance(transform.position, hero.transform.position) <= AI_VIEW_DISTANCE || isHatred)
            {
                //播放奔跑动画（如果有的话）
                //改变敌人状态
                enemyState = STATE_RUN;
                //设置敌人正面朝向
                transform.LookAt(hero.transform);
                if (Vector3.Distance(transform.position, hero.transform.position) <= AI_ATTACK_DISTANCE && (int)Random.Range(0, 20) <= 1)
                {
                    //对主角造成伤害（调用主角的某个接口使得生命值降低）
                    int tempAttackFrame = 190;
                    while (tempAttackFrame-- >= 0) ;    //停下播放动画
                                                        //发送受伤消息，用于调试
                                                        //hero.SendMessage("HeroHurt"); //(写角色控制脚本的记得写消息处理)
                                                        //Debug.Log("HeroHurt");
                    hero.GetComponent<BloodAndScore>().sufferDamage(EnemyAttack);
                    int bloodOutput = 5;
                    while (bloodOutput-- > 0)
                    {
                        GameObject clone = Instantiate(GameObject.Find("MyBlood"), this.transform.position, this.transform.rotation);
                        GameObject.Destroy(clone, 2);
                    }
                }
            }
            else
            {
                if (Time.time - lastUptime >= AI_THINK_TIME)
                {  //开始进行一次思考
                    lastUptime = Time.time;
                    //根据随机数切换战力和行走状态
                    int way = Random.Range(0, 2);

                    if (way == 0)    //敌人进入站立状态
                    {
                        //播放站立时的动画
                        //切换状态
                        enemyState = STATE_STAND;
                    }
                    else if (way == 1)  //敌人进入行走状态
                    {
                        //敌人随机旋转一定角度
                        Quaternion rotate = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
                        //设定1s内完成旋转动作，使得旋转比较自然
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 1000);
                        //播放行走动画
                        //改变敌人状态
                        enemyState = STATE_WALK;
                    }
                }
            }
        }

        switch(enemyState)
        {
            case STATE_STAND:
                break;
            case STATE_WALK:
                transform.Translate(Vector3.forward * Time.deltaTime);
                break;
            case STATE_RUN:
                if(Vector3.Distance(transform.position, hero.transform.position) >= AI_ATTACK_DISTANCE / 2)
                {
                    if (!this.isHurted)
                    {
                        transform.Translate(Vector3.forward * Time.deltaTime * 5);
                    }
                    else
                    {   //设置一个出血效果
                        int bloodOutput = 5;
                        while (bloodOutput-- > 0)
                        {
                            GameObject clone = Instantiate(blood, this.transform.position, this.transform.rotation);
                            clone.GetComponent<BulletInit>().Init();
                            GameObject.Destroy(clone, 2);
                        }

                        //其实就是把移动权交给了ShootAndBlood
                        if (hurtedFrame > 0)
                            transform.Translate(Vector3.forward * (-0.1f));//击打后坐
                        else this.isHurted = false;
                    }
                    
                }
                break;
            case STATE_PAUSE:
                break;
        }
    }

    public void setHatred()
    {
        this.isHatred = true;
    }

    public void setBeingHurt(bool temp)
    {
        this.isHurted = temp;
        hurtedFrame += 10;  //有10帧的后退效果
    }

    public static void setPause(bool temp)
    {
        EnemyMove.STATE_PAUSE_SETTING = temp;
    }
}
