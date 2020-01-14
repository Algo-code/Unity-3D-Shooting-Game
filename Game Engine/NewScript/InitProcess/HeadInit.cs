using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(1f, 0.92f, 0.016f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
