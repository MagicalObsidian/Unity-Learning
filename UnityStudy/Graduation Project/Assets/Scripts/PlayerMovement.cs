using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    [Header("游戏对象组件")]
    private SpriteRenderer sprite;//精灵
    private Rigidbody2D rb;//刚体
    private BoxCollider2D coll_box;//碰撞体
    private CircleCollider2D coll_circle;
    private Animator anim;//动画
    public Transform groundCheckLeft, groundCheckRight;//地面检测(判断角色是否在地面上)
    public LayerMask ground;//地面图层
    public Text playerName;//当前角色名字
    public Joystick joystick;//操纵杆
    public Button btn_Jump;//跳跃按钮
    public Text gold_num;//显示已拾取的金币数量


    [Header("移动参数")]
    public float horizontalMove;

    [Header("地面检测参数")]
    public float footOffset = 0.7f;//角色双脚间的距离
    public float groundDistance = 0.2f;//与地面的距离
    public bool isGround;//是否在地面


    [Header("跳跃参数")]
    public float speed ;//速度
    public float jumpForce;//跳跃力
    public bool jumpPressed;//是否按下跳跃键
    public bool isDoubleJump;//是否二段跳

    private int jumpNum;//当前跳跃次数
    private int allowJumpTimes = 2;//允许跳跃的最大次数

    [Header("按钮")]
    public bool isJumpPressed;//是否按下跳跃键

    [Header("收集")]
    public int gold;//收集金币的数量


    [Header("网络")]
    public bool isNet = false;//是否联机
    
    private void Awake() 
    {
        //获取当前游戏对象组件
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        //coll_box = GetComponent<BoxCollider2D>();
        coll_circle = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>(); 
        //获取当前场景下的 操纵杆 并绑定
        joystick = GameObject.FindGameObjectWithTag("GameController").GetComponent<VariableJoystick>();
        btn_Jump = GameObject.FindGameObjectWithTag("Jump").GetComponent<Button>();
        gold_num = GameObject.FindGameObjectWithTag("Gold").GetComponent<Text>();//获取当前场景下的显示金币数量的文本
        

        LaunchNet();//判断当前是否处于联机场景下
        if(isNet)
        {
            ShowPlayerName();
        }
        
    }
    
    private void Start()
    {
        btn_Jump.onClick.AddListener(P_Jump);//绑定按钮 跳跃 事件
        jumpNum = 0;//初始跳跃次数设为 0
        gold = 0;//金币数为 0
    }

    // Update is called once per frame
    void Update()
    {
        if(isNet)
        {
            PlayerNetCheck();
        }
        else
        {
            gold_num.text = gold.ToString();
        }

        if(Input.GetButtonDown("Jump") || isJumpPressed)//每帧检测按键
        {
            jumpPressed = true; 
            isJumpPressed = false;
        }    
    }

    private void FixedUpdate() 
    {
        if(isNet)
        {
            PlayerNetCheck();
        }

        //每物理帧执行带有物理的方法
        GroundCheck();
        Move();
        Jump();
        AnimSwitch();
    }

    //地面碰撞检测
    void GroundCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, ground);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, ground);
        isGround = Physics2D.OverlapCircle(groundCheckLeft.position, 0.1f, ground)
                && Physics2D.OverlapCircle(groundCheckRight.position, 0.1f, ground);
        /* if(leftCheck || rightCheck)
        {
            isGround = true;
        } else {
            isGround = false;
        } */
        if(isGround)
        { 
            jumpNum = 0;
        }
    }

    //角色移动
    private void Move()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");//获取输入的水平方向值
        //判断有没有 joystick 输入的值
        horizontalMove = horizontalMove == 0 ? joystick.Horizontal : horizontalMove;//获取输入的水平方向值(触摸屏操纵杆)

        /* 多人联机下 要等实例化对象后，再在 控制 UI 中绑定到对象上 */
        Move_fun(horizontalMove);
    }
    
    //角色移动具体物理实现方法
    private void Move_fun(float hm)//水平方向上的值 -1, 0, 1 (左、不动、右)
    {
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if(horizontalMove != 0.0f)
        {
            //transform.localScale = new Vector3(horizontalMove, 1, 1);
            if(horizontalMove < 0) {
                sprite.flipX = true;//X 轴水平翻转
            } else {
                sprite.flipX = false;
            }
        }
    }
    
    //角色跳跃
    private void Jump()
    {
        if (jumpPressed)
        {
            jumpPressed = false;
            //不能跳跃
            if (allowJumpTimes <= 0)
                return;
            //跳跃前在地面
            else if (isGround && jumpNum == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            //空中再次跳跃即二段跳
            else if (jumpNum < allowJumpTimes - 1)
            {
                //anim.SetBool("DoubleJump", true);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpNum++;
            }
        }
    }

    //动画切换
    private void AnimSwitch()
    {
        anim.SetBool("idle", true);
        anim.SetFloat("run", Mathf.Abs(rb.velocity.x));

        if(isGround)
        {
            anim.SetBool("fall", false);
        }
        else if(rb.velocity.y > 0)
        {
            anim.SetBool("jump", true);
        }
        else if(rb.velocity.y < 0)
        {
            anim.SetBool("fall", true);
            anim.SetBool("jump", false);
            //anim.SetBool("DoubleJump", false);
        }
    }

    //重载页面
    private void Restart()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//获得当前 scene 的名字
    }


    /* 启用角色联机模块 */
    public void LaunchNet()
    {
        if(SceneManager.GetActiveScene().name == "PVP")
        {
            isNet = true;
        }
    }
    //判断联机是否控制当前角色
    private void PlayerNetCheck()
    {
        //如果连接服务器后观察的不是当前的角色就返回(每帧执行)
        if(!photonView.IsMine && PhotonNetwork.IsConnected) 
        {
            return; 
        }
    }
    //显示联机角色名字
    private void ShowPlayerName()
    {
        if(photonView.IsMine) {
            playerName.text = PhotonNetwork.NickName;
        } else {
            playerName.text = photonView.Owner.NickName; 
        } 
    }



    //重写射线判断方法(未使用)
    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        /* 射线(射线点，射线方向，射线长度，检测和地面的碰撞) */
        Vector2 pos = this.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);
        Debug.DrawRay(pos + offset, rayDirection * length);
        return hit;
    }

    //捡拾金币
    public void Count_Gold()
    {
        gold += 1;
    }

    //判断按钮按下
    private void P_Jump()
    {
        isJumpPressed = true;
    }

}
