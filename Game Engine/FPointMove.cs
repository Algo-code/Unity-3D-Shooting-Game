using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//BUG
public class FPointMove : MonoBehaviour
{
    public GameObject target;   //获得Cube对象
    Transform targetObj; //指向Cube主角 
    public float dist = -1.0f;  //默认距离
    //旋转轴向控制
    float x;
    float y;
    //旋转角度限制
    float yMin = 0;
    float yMax = 80.0f;
    //视角移动速度
    float xSpeed = 250.0f;
    float ySpeed = 120.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
        {
            targetObj = target.transform;
        }
        Vector2 angles = transform.eulerAngles; //获得本物体欧拉角
        x = angles.x;
        y = angles.y;
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (targetObj != null)
        {
            if (!target.GetComponent<TempMove>().getState())
            {
                if (Cursor.visible) Cursor.visible = false;
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f; //计算旋转视角x
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f; //计算旋转视角y
                y = ClampAngle(y, yMin, yMax);
                Quaternion rotation = Quaternion.Euler(y, x, 0);
                Vector3 position = rotation * new Vector3(0, 0, -dist) + targetObj.position;
                transform.rotation = rotation;
                transform.position = position;
                target.transform.rotation = Quaternion.Euler(y, x, 0);   //控制物体转向
                target.GetComponent<TempMove>().setState(false);
            }
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        //避免向后可以自由旋转360°
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}
