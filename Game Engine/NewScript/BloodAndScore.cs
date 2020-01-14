using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAndScore : MonoBehaviour
{
    //最大生命值和当前血量
    public const int MAX_HP = 100;
    int HP;
    //设定当前分数
    int Scores = 0;
    public Texture tex_star;
    int ChallengeScores = 0;
    public Texture tex_challenge;   //通过当前关卡初始化获得
    //获取血条贴图资源
    public Texture tex_red;
    public Texture tex_black;
    //获取数字图片资源
    object[] texmube;
    //武器子弹图片
    public Texture tex_ammo;
    //子弹数量上限
    public int AMMO_MAX = 30;
    //当前子弹数量
    int tempAmmo;

    //获取血量
    GameObject blood;

    // Start is called before the first frame update
    void Start()
    {
        HP = MAX_HP;
        tempAmmo = AMMO_MAX;
        texmube = Resources.LoadAll("number");
        blood = GameObject.Find("MyBlood");
        ChallengeScores = ChallengeOneInit.Scores;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        //打印自己的血量
        int blood_width = tex_red.width * HP / MAX_HP;
        GUI.DrawTexture(new Rect(Screen.width - tex_black.width - 25, Screen.height - tex_black.height-5, tex_black.width, tex_black.height), tex_black);
        GUI.DrawTexture(new Rect(Screen.width - tex_black.width - 25, Screen.height - tex_black.height - 5, blood_width, tex_red.height), tex_red);
        Tools.DrawImageNumber(Screen.width - tex_black.width -75, Screen.height - tex_black.height -5, HP, texmube);
        //打印分数
        GUI.DrawTexture(new Rect(Screen.width - tex_black.width - 75, Screen.height - tex_black.height - 5 - tex_star.height, tex_star.width, tex_star.height), tex_star);
        Tools.DrawImageNumber(Screen.width - tex_black.width - 75 + tex_star.width + 5, Screen.height - tex_black.height - 5 - (tex_star.height / 2), Scores, texmube);
        //打印关卡应得分数
        GUI.DrawTexture(new Rect(Screen.width - tex_black.width+75, Screen.height - tex_black.height - 5 - tex_challenge.height, tex_challenge.width, tex_challenge.height), tex_challenge);
        Tools.DrawImageNumber(Screen.width - tex_black.width + 75 + tex_challenge.width + 5, Screen.height - tex_black.height - 5 - tex_challenge.height / 2, ChallengeScores, texmube);
        //打印子弹数量
        GUI.DrawTexture(new Rect(Screen.width - tex_ammo.width, Screen.height - tex_black.height - tex_challenge.height - 5 - tex_ammo.height, tex_ammo.width, tex_ammo.height), tex_ammo);
        Tools.DrawImageNumber(Screen.width - tex_ammo.width -50, Screen.height - tex_black.height - tex_challenge.height - 5 - tex_ammo.height, tempAmmo, texmube);
    }

    public void sufferDamage(int damage)
    {
        if(damage <= HP)
        {
            HP -= damage;
            //设置一个红色出血效果
            int bloodOutput = 300;
            while (bloodOutput-- > 0)
            {
                GameObject clone = Instantiate(blood, this.transform.position, this.transform.rotation);
                GameObject.Destroy(clone, 2);
            }
            if (HP == 0)
            {
                //设置失败信息
                GetComponent<TempMove>().Lose();
            }
        }
        else
        {
            //设置失败信息
            GetComponent<TempMove>().Lose();
        }
    }

    public void addScore(int scores)
    {
        this.Scores += scores;
        if(this.Scores >= ChallengeScores)
        {
            //设置胜利状态
            GetComponent<TempMove>().Win();
        }
    }

    public void shootOnce()
    {
        if(tempAmmo == 0)
        {
            //设置为换弹状态
            GetComponent<TempMove>().SetReloading();
        }
        else
        {
            tempAmmo--;
            if (tempAmmo == 0) GetComponent<TempMove>().SetReloading();
        }
    }

    public void resetAmmoAmount()
    {
        this.tempAmmo = AMMO_MAX;
    }

    public int getTempAmmo()
    {
        return tempAmmo;
    }
}
