using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//史莱姆类 继承自 敌人类
public class Enemy_Slime : Enemy
{
    [Header("游戏对象组件")]
    private CircleCollider2D coll_circle;//碰撞体
    public LayerMask ground;//地面图层
    public Transform LEFT, RIGHT;//左右边界子对象的transform
    public Transform BOTTOM;//用于判断是否在地面


    [Header("怪物参数")]
    private float left_x, right_x;//用于开始获取设置好的怪物移动范围横坐标
    private int face = -1;//怪物朝向, 初始向左
    private float speed_x = 2.0f;//水平方向上的移动速度




    protected override void Start()
    {
        base.Start();

        //获取组件
        //rb = GetComponent<Rigidbody2D>();
        //coll_circle = GetComponent<CircleCollider2D>();

        transform.DetachChildren();//与子对象分离
        //获取左右移动边界的横坐标，然后销毁子对象
        left_x = LEFT.position.x;
        right_x = RIGHT.position.x;

        Destroy(LEFT.gameObject);
        Destroy(RIGHT.gameObject);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Move();
    }

    private void Move()
    {
        
        if(Physics2D.OverlapCircle(BOTTOM.position, 0.1f, ground))
        {
            //LockY();

            if(face == -1)
            {
                rb.velocity = new Vector2(speed_x * face, rb.velocity.y);
                
                if(this.transform.position.x - left_x < 0.1f)
                {
                    this.transform.localScale = new Vector3(-1, 1, 1);
                    //rb.velocity = new Vector2(speed_x, rb.velocity.y);
                    face = -face;
                }
            } 
            else if(face == 1)
            {
                rb.velocity = new Vector2(speed_x * face, rb.velocity.y);
                if(this.transform.position.x - right_x > -0.1f)
                {
                    this.transform.localScale = new Vector3(1, 1, 1);
                    //rb.velocity = new Vector2(-speed_x, rb.velocity.y);
                    face = -face;
                }
            }           
        }
    }


    //锁定 Y 轴
    void LockY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    } 
}
