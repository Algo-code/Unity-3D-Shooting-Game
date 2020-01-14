using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float red = Random.Range(0.0f, 1.0f);
        float green = Random.Range(0.0f, 1.0f);
        float blue = Random.Range(0.0f, 1.0f);
        GetComponent<Renderer>().material.color = new Color(red, green, blue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        float red = Random.Range(0.0f, 1.0f);
        float green = Random.Range(0.0f, 1.0f);
        float blue = Random.Range(0.0f, 1.0f);
        GetComponent<Renderer>().material.color = new Color(red, green, blue);
    }

    public void InitWithRed()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
}
