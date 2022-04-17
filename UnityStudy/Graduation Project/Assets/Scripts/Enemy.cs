using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敌人类作为基类
public class Enemy : MonoBehaviour
{
    [Header("游戏对象组件")]
    protected Rigidbody2D rb;//刚体对象
    protected Animator anim;//动画对象
    //protected AudioSource deathSource;

    [Header("怪物参数")]
    public int health;//敌人生命
    //public int sufferDamage;//敌人受到的伤害

    //虚函数供子类继承
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();//获取刚体组件
        anim = GetComponent<Animator>();//获取动画组件
    }

    protected virtual void FixedUpdate()
    {
        Death();
    }


    //敌人受伤
    public void SufferDamage(int damage)
    {
        anim.SetBool("hit", true);
        health -= damage;
        StartCoroutine(getHit());
    }


    //敌人死亡
    public void Death()
    {
        if(health <= 0)
        {
            Lock();//死亡时敌人停止移动
            anim.SetTrigger("death");
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
        yield return new WaitForSeconds(1);//等待指定的秒钟后继续执行代码(这里死亡动画的时间大概是1秒)
        Destroy(this.gameObject);//销毁该对象
    }

    //锁定游戏对象
    void Lock()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

}
