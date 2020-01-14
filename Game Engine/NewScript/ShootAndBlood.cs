using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAndBlood : MonoBehaviour
{
    //是否绘制血量
    bool showBlood = false;
    //是否正在受到攻击
    bool suffDamage = false;
    //获得血量贴图
    public Texture tex_red;
    public Texture tex_black;
    //获取数字图片资源
    object[] texmube;
    //血量上限
    public int MAX_HP = 100;    //敌人血量上限
    //当前血量
    int HP;
    //玩家武器攻击间隔，来确定隔多少帧掉一次血
    public int WeaponF = 20;   //武器频率
    public int WeaponDamage = 5;    //武器威力
    int currentFrames;
    //主角模块
    GameObject hero;
    //获取血量
    GameObject blood;

    // Start is called before the first frame update
    void Start()
    {
        HP = MAX_HP;
        hero = GameObject.Find("Hero");
        texmube = Resources.LoadAll("number");
        blood = GameObject.Find("Blood");
        currentFrames = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {    
        //控制攻击频率
        if (currentFrames < WeaponF) currentFrames++;

        if (suffDamage && currentFrames == WeaponF) {
            if (HP > 0)
            {
                HP -= WeaponDamage;    //注：以后要设置一个WeaponDamage属性，所减血量同武器伤害
                hero.GetComponent<BloodAndScore>().shootOnce();
                //设置一个出血效果
                int bloodOutput = 5;
                while (bloodOutput-- > 0)
                {
                    GameObject clone = Instantiate(blood, this.transform.position, this.transform.rotation);
                    clone.GetComponent<BulletInit>().Init();
                    GameObject.Destroy(clone, 2);
                }
                GetComponent<EnemyMove>().setBeingHurt(true);
                //transform.Translate(Vector3.forward * (-0.1f));//击打后坐

                //GetComponent<EnemyMove>().setBeingHurt(false);
                currentFrames = 0;  //重新计时
                if (HP <= 0)
                {
                    hero.GetComponent<BloodAndScore>().addScore(150);
                    //设置一个出血效果
                    bloodOutput = 5;
                    while (bloodOutput-- > 0)
                    {
                        GameObject clone = Instantiate(blood, this.transform.position, this.transform.rotation);
                        clone.GetComponent<BulletInit>().Init();
                        GameObject.Destroy(clone, 2);
                    }

                    //敌人死亡（如果有死亡动画还要加上）
                    GameObject.Destroy(gameObject);
                }
            }
            else
            {
                hero.GetComponent<BloodAndScore>().addScore(150);
                //设置一个出血效果
                int bloodOutput = 5;
                while (bloodOutput-- > 0)
                {
                    GameObject clone = Instantiate(blood, this.transform.position, this.transform.rotation);
                    clone.GetComponent<BulletInit>().Init();
                    GameObject.Destroy(clone, 2);
                }

                //敌人死亡（如果有死亡动画还要加上）
                GameObject.Destroy(gameObject);
                
            }
        }

    }

    void OnGUI()
    {
        if (showBlood) {
            int blood_width = tex_red.width * HP / MAX_HP;
            GUI.DrawTexture(new Rect(5, 5, tex_black.width, tex_black.height), tex_black);
            GUI.DrawTexture(new Rect(5, 5, blood_width, tex_red.height), tex_red);
            Tools.DrawImageNumber(5+tex_black.width+5, 5, HP, texmube);

        }
    }

    void OnMouseDown() {
        //开始掉血
        if(hero.GetComponent<BloodAndScore>().getTempAmmo() != 0)
        {
            suffDamage = true;
            GetComponent<EnemyMove>().setHatred();
        }
    }

    void OnMouseUp() {
        //停止掉血
        suffDamage = false;
    }

    void OnMouseOver() {
        //开始绘制血条
        showBlood = true;
    }

    void OnMouseExit() {
        //停止绘制血条
        showBlood = false;
        suffDamage = false;
    }


}
