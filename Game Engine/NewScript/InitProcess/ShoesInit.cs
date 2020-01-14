using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoesInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float red = 1.0f, blue = 1.0f, green = 1.0f;

        //不能是白色
        while (red + green + blue >= 2.9f)
        {
            red = Random.Range(0.0f, 1.0f);
            green = Random.Range(0.0f, 1.0f);
            blue = Random.Range(0.0f, 1.0f);
        }
        GetComponent<Renderer>().material.color = new Color(red, green, blue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
