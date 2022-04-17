using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform Cam;//将要挂载相机的参数
    public float moveRate;//背景图移动速率
    private float startPoint_X, startPoint_Y;//开始点位的 X、Y 坐标
    public bool lockY;//默认false,是否锁定 Y 方向移动

    void Start()
    {
        //开始时获得当前对象(图片背景)的坐标参数
        startPoint_X = transform.position.x;
        startPoint_Y = transform.position.y;
    }

    void Update()
    {
        //如果锁定 Y 轴，就只在 X 轴方向上移动相机，产生视觉差的效果
        if(lockY) {
            transform.position = new Vector2(startPoint_X + Cam.position.x * moveRate, transform.position.y);
        }
        else {
            transform.position = new Vector2(startPoint_X + Cam.position.x * moveRate, startPoint_Y + Cam.position.y * moveRate);
        }
    }
}
