using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PaintMenuInGame : MonoBehaviour
{
    public GUISkin mySkin;
    bool PaintMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        GUI.skin = mySkin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (this.PaintMenu)
        {
            if (GUI.Button(new Rect(0, 0, 338, 112), "", "closeButton"))
            {
                this.PaintMenu = false; //关闭GUI界面
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height * 0.25f - 50, 300, 100), "Resume"))
            {
                this.PaintMenu = false; //关闭GUI界面
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height * 0.75f - 50, 300, 100), "Exit"))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public void setState(bool state)
    {
        this.PaintMenu = false;
    }

    public bool getState()
    {
        return this.PaintMenu;
    }
}
