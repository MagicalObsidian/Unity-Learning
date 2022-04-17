using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    
    [Header("攻击参数")]
    public int damage;//角色的攻击伤害
    public float startTime;//攻击开始时间
    public float lastTime;//攻击持续时间
    private bool isAttack;//是否在攻击
    public bool isAttackPressed;//是否按下攻击键

    [Header("角色生命")]
    public int health;//角色血量

    
    [Header("游戏对象组件")]
    private Animator anim;
    private PolygonCollider2D coll_poly;//攻击碰撞体
    private Rigidbody2D rb;
    private BoxCollider2D coll_box;//碰撞体
    public Button btn_Attack;
    public Text heart_num;//显示角色生命值

    //public GameObject gameover;

    private void Awake()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        coll_poly = GetComponent<PolygonCollider2D>();
        coll_box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        btn_Attack = GameObject.FindGameObjectWithTag("Attack").GetComponent<Button>();//获取当前场景下的 攻击 按钮
        heart_num = GameObject.FindGameObjectWithTag("Heart").GetComponent<Text>();//获取当前场景下的显示生命值的文本
    }


    // Start is called before the first frame update
    void Start()
    {
        btn_Attack.onClick.AddListener(P_Attack);//绑定按钮 攻击 事件
        health = 5;//角色默认初始血量为 5
    }

    // Update is called once per frame
    void Update()
    {
        Attack();

        heart_num.text = health.ToString();//显示当前生命值
    }

    void FixedUpdate()
    {
        Death();
    }

    //角色攻击
    public void Attack()
    {
        if(!isAttack) //未处于攻击状态(这样防止一按攻击键角色就开始攻击动画)
        {
            if(Input.GetButton("Attack") || isAttackPressed)
            {            
                anim.SetTrigger("attack");//转入攻击动画
                isAttack = true;
                isAttackPressed = false;
                StartCoroutine(StartAttack());
            }
        }
    }

    //开始攻击的协程
    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(startTime);
        coll_poly.enabled = true;
        yield return new WaitForSeconds(lastTime);
        coll_poly.enabled = false;//关闭攻击碰撞体
        isAttack = false;
    }

    //角色受到伤害
    public void SufferDamage(int damage)
    {
        anim.SetBool("hit", true);
        health -= damage;
        StartCoroutine(getHit());
    }

    //角色死亡
    public void Death()
    {
        if(health <= 0)
        {
            //anim.SetTrigger("death");
            anim.Play("death");
            StartCoroutine(goToDie());   
        }
    }

    IEnumerator getHit()
    {
        yield return new WaitForSeconds(0.2f);//受伤动画持续 0.2 秒
        anim.SetBool("hit", false);//受到攻击并结算后回到 idle 状态
    }
    IEnumerator goToDie()
    {
        yield return new WaitForSeconds(1.5f);//等待指定的秒钟后继续执行代码(这里死亡动画的时间大概是1秒)
        //Destroy(this.gameObject);//销毁该对象
        /* 角色死亡后显示游戏结束 */
        GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        this.gameObject.SetActive(false);
    }


    //碰撞体触发器进行攻击判定
    public void OnTriggerEnter2D(Collider2D other) 
    {
        //造成伤害
        /* 触发后扫描碰撞体 Tag为 Enemy 的就减少生命值 */
        if(other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().SufferDamage(damage);//攻击敌人使其受到伤害
        }

        /* 攻击其他玩家 */
        else if(other.tag == "Player")
        {
            other.GetComponent<PlayerAttack>().SufferDamage(damage);
        }

        /* 捡到金币 */
        else if(other.tag == "Gold")
        {
            other.GetComponent<Animator>().Play("Got");
        }

        /* 捡到生命 */
        else if(other.tag == "Heart")
        {
            other.GetComponent<Animator>().Play("Got");
        }

        //角色掉落后死亡并重置游戏(或者显示游戏结束)
        else if(other.tag == "DeadLine") {
            //GetComponent<AudioSource>().enabled = false;//禁用所有音乐组件
            Invoke("Restart", 2.0f);//重载页面 延迟2秒
        }

    }

    //碰撞体碰撞判定
    public void OnCollisionEnter2D(Collision2D other)
    {
        //角色碰撞到敌人则受到伤害
        if(other.gameObject.tag == "Enemy")
        {
            /* 反弹效果? */
            if(transform.position.x != other.gameObject.transform.position.x) 
            {
                int rebound = transform.position.x - other.gameObject.transform.position.x < 0 ? -1 : 1;
                rb.velocity = new Vector2(5 * rebound, rb.velocity.y);
                this.SufferDamage(damage);
            }   
        }
    }



    //重载页面
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//获得当前 scene 的名字
    }

    //捡拾生命
    public void Count_Heart()
    {
        health += 1;
    }

    //按下攻击键
    private void P_Attack()
    {
        isAttackPressed = true;
    }
}
