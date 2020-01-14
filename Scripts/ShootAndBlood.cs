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
    int MAX_HP = 100;
    //当前血量
    int HP;
    //玩家武器攻击间隔，来确定隔多少帧掉一次血
    int WeaponF = 20;   //根据需求调整
    int currentFrames;
    //主角模块
    GameObject hero;

    // Start is called before the first frame update
    void Start()
    {
        HP = MAX_HP;
        hero = GameObject.Find("Hero");
        texmube = Resources.LoadAll("number");
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
                HP -= 5;    //注：以后要设置一个WeaponDamage属性，所减血量同武器伤害
                GetComponent<EnemyMove>().setBeingHurt(true);
                //transform.Translate(Vector3.forward * (-0.1f));//击打后坐
                //GetComponent<EnemyMove>().setBeingHurt(false);
                currentFrames = 0;  //重新计时
                if(HP <= 0) GameObject.Destroy(gameObject);
            }
            else
            {
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
        suffDamage = true;
        GetComponent<EnemyMove>().setHatred();
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
