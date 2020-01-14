
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //设定游戏状态
    public const int STATE_MAINMENU = 0;
    public const int STATE_STARTGAME = 1;
    public const int STATE_OPTION = 2;
    public const int STATE_HELP = 3;
    public const int STATE_EXIT = 4;

    //设定GUI皮肤
    public GUISkin mySkin;

    //背景图片
    public Texture textureBackGround;
    //开始菜单
    public Texture textureStartInfo;
    //帮助界面
    public Texture textureHelpInfo;

    //背景音乐
    public AudioSource music;
    //当前游戏状态
    private int gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = STATE_MAINMENU;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        switch (gameState)
        {
            case STATE_MAINMENU:
                RenderMainMenu();
                break;
            case STATE_STARTGAME:
                RenderStart();
                break;
            case STATE_OPTION:
                RenderOption();
                break;
            case STATE_HELP:
                RenderHelp();
                break;
            case STATE_EXIT:
                //退出
                break;
        }
    }

    void RenderMainMenu()
    {
        GUI.skin = mySkin;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), textureBackGround);

        if(GUI.Button(new Rect(0, 30, 623, 153), "", "start"))
        {
            //准备进入游戏
            gameState = STATE_STARTGAME;
        }

        if(GUI.Button(new Rect(0, 180, 623, 153), "", "option"))
        {
            gameState = STATE_OPTION;
        }

        if(GUI.Button(new Rect(0, 320, 623, 153), "", "help"))
        {
            //帮助菜单
            gameState = STATE_HELP;
        }

        if(GUI.Button(new Rect(0, 470, 623, 153), "", "exit"))
        {
            //退出
            Application.Quit();
        }
    }

    void RenderStart()
    {
        GUI.skin = mySkin;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), textureStartInfo);
        if(GUI.Button(new Rect(0, 500, 403, 78), "", "back"))
        {
            gameState = STATE_MAINMENU;
        }
        SceneManager.LoadScene("GameScene");
    
    }

    void RenderOption()
    {
        GUI.skin = mySkin;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), textureBackGround);

        //音乐设置
        if(GUI.Button(new Rect(0, 0, 403, 75), "", "music_on"))
        {
            if (!music.isPlaying)   //避免禁忌二重奏情况
            {
                music.Play();
            }
        }

        if(GUI.Button(new Rect(0,  200,  403,  75), "", "music_off"))
        {
            music.Stop();
        }

        if(GUI.Button(new Rect(0, 500, 403, 78), "", "back"))
        {
            gameState = STATE_MAINMENU;
        }
    }

    void RenderHelp()
    {
        GUI.skin = mySkin;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), textureBackGround);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), textureHelpInfo);
        if(GUI.Button(new Rect(0, 500, 403, 78), "", "back"))
        {
            gameState = STATE_MAINMENU;
        }
    }

}
