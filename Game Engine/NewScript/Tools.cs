using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    /*
        x, y: 数字坐标
        number: 要绘制的数字
        texmube: 图片资源
    */
    public static void DrawImageNumber(int x, int y, int number, object[] texmube) {
        //将数字转化为char数组
        char[] arrays = number.ToString().ToCharArray();
        Texture tex = (Texture)texmube[0];
        int width = tex.width;
        int height = tex.height;
        //遍历打印
        foreach (char c in arrays)
        {
            int i = int.Parse(c.ToString());
            //绘制图片数字
            GUI.DrawTexture(new Rect(x, y, width, height), (Texture)texmube[i]);
            x += width;
        }
    }
}
