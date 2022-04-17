using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
     [Header("按钮")]
    public GameObject Button_GameReady;
    public GameObject Button_GamePause;
    public GameObject gameover;//游戏结束 UI
    public GameObject Button_Quit;
    public GameObject Button_NextLevel;
    public GameObject Button_Warrior_Chs;
    public GameObject Button_Samurai_Chs; 
    //public Joystick input_Joystick;//当前场景绑定的操纵杆

    

    public string HeroType = "default";

    
    //选择战士的按钮事件
    public void Warrior_Chs()
    {
        HeroType = "Player_Hero_Warrior";
    }

    //选择武者的按钮事件
    public void Samurai_Chs()
    {
        HeroType = "Player_Hero_Samurai";
    }

    //选择好角色后进入游戏
    public void EnterGame()
    {
        GameObject.Find("GameManager/HeroChoose").SetActive(false);

        if(HeroType == "default") 
        {
            Debug.Log("Please Choose a Hero!");
        }
        else {
            PhotonNetwork.Instantiate(HeroType, new Vector3(1, 1, 0), Quaternion.identity, 0);//实例化游戏对象
            //BindInputs();
        }
    }

    IEnumerator BindInputs()
    {
        yield return new WaitForSeconds(2);
        GetButton_Jump();
        GetButton_Attack();
    }

    //绑定实例化后对象的操纵杆水平移动参数
/* (没有用)    public void GetJoystickInput()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().horizontalMove = input_Joystick.Horizontal;
    } */

    //绑定 Jump
    public bool GetButton_Jump()
    {
        return true;
    }

    //绑定 Attack
    public bool GetButton_Attack()
    {
        return true;
    }



    //游戏进行
    public void GameReady()
    {
        Button_Quit.SetActive(false);
        Time.timeScale = 1f;
    }

    //游戏暂停
    public void GamePause()
    {
        //GameObject.Find("Canvas/Menu_Pause/Button_Quit").SetActive(true);
        Button_Quit.SetActive(true);
        Time.timeScale = 0f;
    }

    //游戏结束
    public void GameOver()
    {
        gameover.SetActive(true);
    }

    //进入下一关
    public void NextLevel()//绑定 下一关 按钮
    {
        //加载场景,可以直接用场景编号
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene(1);
    }


    //游戏退出
    public void GameQuit()
    {
        Application.Quit();
    }

    //返回主菜单
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);//MainMenu
    }
}
