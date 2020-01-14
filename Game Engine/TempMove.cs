using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMove : MonoBehaviour
{
    private float CameraSpeed = 8.0f;
    private Vector3 offset;
    private Vector3 lookPosition;
    //武器的攻击间隔
    private int CoolDown;
    //武器实际冷却
    private int countCoolDownFrames;
    //武器换弹时间
    private int reloadingFrames;
    public int ReloadingTime = 180;

    public Vector3 GetLookPos()
    {
        return this.lookPosition;
    }

    public bool VisitMode = false;   //为true时可以无限跳跃
    public GUISkin mySkin;          //设置GUI皮肤
    public Texture Aim;
    private bool couldJump = true;

    private bool PaintMenu = false; //为true时处于绘制菜单状态
    //获取胜利、失败图像资源
    public Texture tex_win;
    public Texture tex_lose;
    //胜利、失败状态
    private bool PaintWinMenu = false;
    private bool PaintLoseMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        GUI.skin = mySkin;
        offset = Camera.main.transform.position - this.transform.position;  //获得偏移量
        lookPosition = -offset;
        CoolDown = GameObject.Find("Enemy").GetComponent<ShootAndBlood>().WeaponF;
        countCoolDownFrames = CoolDown;
        reloadingFrames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (countCoolDownFrames < CoolDown) countCoolDownFrames++;  //武器间隔控制
        if (reloadingFrames > 0) {
            reloadingFrames--;     //换弹时间控制
            if (reloadingFrames == 0) GetComponent<BloodAndScore>().resetAmmoAmount();
        }

        Vector3 CameraPosition = this.transform.position + offset;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, CameraPosition, CameraSpeed * Time.deltaTime);    //Lerp

        //控制Cube移动
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * 5);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.transform.Translate(Vector3.forward * Time.deltaTime * 5);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * (-5));
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.transform.Translate(Vector3.forward * Time.deltaTime * (-5));
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * (5));
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.transform.Translate(Vector3.left * Time.deltaTime * 5);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * (-5));
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.transform.Translate(Vector3.left * Time.deltaTime * (-5));
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(GetComponent<BloodAndScore>().getTempAmmo() <= GetComponent<BloodAndScore>().AMMO_MAX)
            {
                SetReloading();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(this.VisitMode)
            {
                this.GetComponent<Rigidbody>().AddForce(0, 250, 0);
            }
            else
            {
                if(this.couldJump) this.GetComponent<Rigidbody>().AddForce(0, 250, 0);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.PaintMenu = !PaintMenu;
            EnemyMove.setPause(this.PaintMenu);//令敌人进入STATE_PAUSE状态或解除
        }
        if (Input.GetMouseButton(0))    //按着左键，发射子弹
        {   //有一个后坐力
            //Vector3 tempOffset = Camera.main.transform.position - this.transform.position;
            //Vector3 BackPos = new Vector3(tempOffset.x * 0.035f, 0, tempOffset.z * 0.035f);
            if(reloadingFrames == 0)
            {
                if (countCoolDownFrames >= CoolDown)
                {
                    this.transform.Translate(Vector3.forward * (-0.1f));
                    countCoolDownFrames = 0;
                    GetComponent<BloodAndScore>().shootOnce();
                }
            }

            //GetComponent<Rigidbody>().AddForce(BackPos * 3);
        }
        if (Input.GetMouseButton(1))    //按着右键，切换人称
        {
            Camera.main.GetComponent<ViewControl>().dist = 0f;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Camera.main.GetComponent<ViewControl>().dist = 7.0f;
        }

        //this.transform.LookAt(lookPosition);
        Quaternion.LookRotation(lookPosition);
    }

    void OnCollisionStay(Collision obj)
    {
        if(obj.gameObject.name == "Terrain")
        {
            this.couldJump = true;
        }
    }
    void OnCollisionExit(Collision obj)
    {
        if(obj.gameObject.name == "Terrain")
        {
            this.couldJump = false;
        }
    }

    void OnGUI()
    {
        if (PaintLoseMenu)
        {
            EnemyMove.setPause(true);//令敌人进入STATE_PAUSE状态或解除
            this.PaintMenu = true;
            GUI.DrawTexture(new Rect(0, 0, tex_lose.width, tex_lose.height), tex_lose);

            if (GUI.Button(new Rect(tex_lose.width+50, Screen.height * 0.25f - 50, 300, 100), "Restart"))
            {
                Cursor.visible = false;
                this.PaintMenu = false; //关闭GUI界面
                EnemyMove.setPause(false);
                PaintLoseMenu = false;
                SceneManager.LoadScene("GameScene");
            }
            if (GUI.Button(new Rect(tex_lose.width + 50, Screen.height * 0.75f - 50, 300, 100), "Exit"))
            {
                PaintLoseMenu = false;
                SceneManager.LoadScene("SampleScene");
            }

        }
        else if (PaintWinMenu)
        {
            EnemyMove.setPause(true);//令敌人进入STATE_PAUSE状态或解除
            this.PaintMenu = true;
            GUI.DrawTexture(new Rect(0, 0, tex_win.width, tex_win.height), tex_win);

            if (GUI.Button(new Rect(tex_win.width + 150, Screen.height * 0.25f - 50, 300, 100), "Next Level"))
            {
                Cursor.visible = false;
                this.PaintMenu = false; //关闭GUI界面
                EnemyMove.setPause(false);
            }
            if (GUI.Button(new Rect(tex_win.width + 150, Screen.height * 0.5f - 50, 300, 100), "Restart"))
            {
                Cursor.visible = false;
                this.PaintMenu = false; //关闭GUI界面
                EnemyMove.setPause(false);
                PaintWinMenu = false;
                SceneManager.LoadScene("GameScene");
            }
            if (GUI.Button(new Rect(tex_win.width + 150, Screen.height * 0.75f - 50, 300, 100), "Exit"))
            {
                PaintWinMenu = false;
                SceneManager.LoadScene("SampleScene");
            }
        }
        else
        {
            if (PaintMenu)
             {
                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height * 0.25f - 50, 300, 100), "Resume"))
                {
                    Cursor.visible = false;
                    this.PaintMenu = false; //关闭GUI界面
                    EnemyMove.setPause(false);
                }
                if(GUI.Button(new Rect(Screen.width/2 - 150, Screen.height * 0.5f - 50, 300, 100), "Restart"))
                {
                    Cursor.visible = false;
                    this.PaintMenu = false; //关闭GUI界面
                    EnemyMove.setPause(false);
                    SceneManager.LoadScene("GameScene");
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height * 0.75f - 50, 300, 100), "Exit"))
                {
                    SceneManager.LoadScene("SampleScene");
                }
        }
        else
        {
            GUI.DrawTexture(new Rect(Input.mousePosition.x - (Aim.width/2), Screen.height - Input.mousePosition.y - (Aim.height/2), Aim.width, Aim.height), Aim);
        }
        }

    }

    public void setState(bool state)
    {
        this.PaintMenu = state;
    }
    public bool getState()
    {
        return this.PaintMenu;
    }
    public bool getJumpState()
    {
        return this.couldJump;
    }
    public void Win()
    {
        this.PaintWinMenu = true;
        EnemyMove.setPause(true);
    }
    public void Lose()
    {
        this.PaintLoseMenu = true;
        EnemyMove.setPause(true);
    }
    public void SetReloading()
    {
        reloadingFrames = ReloadingTime;
    }
}
