using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    //单人游玩
    public void Button_Single()
    {
        GameObject.Find("Canvas/Singleplayer/Button_Single").SetActive(false);
        GameObject.Find("Canvas/Singleplayer/Button_Login_Single").SetActive(true);
        GameObject.Find("Canvas/Singleplayer/Button_Return").SetActive(true);

        GameObject.Find("Canvas/Multiplayer/Button_Multiple").SetActive(false);

        GameObject.Find("Canvas/GameQuit/Button_Quit").SetActive(false);
    }

    //多人游玩
    public void Button_Mulitple()
    {
        GameObject.Find("Canvas/Singleplayer/Button_Single").SetActive(false);

        GameObject.Find("Canvas/Multiplayer/Button_Multiple").SetActive(false);

        GameObject.Find("Canvas/Multiplayer/NameSet").SetActive(true);

        GameObject.Find("Canvas/GameQuit/Button_Quit").SetActive(false);
    }


    //加入游戏(单人)
    public void Button_Login_Single()
    {
        SceneManager.LoadScene(2);//Round-01
    }

    //加入游戏(多人)
    public void Button_Login_Multiple()
    {

    }

    //返回菜单
    public void Button_Return()
    {
        GameObject.Find("Canvas/Singleplayer/Button_Single").SetActive(true);
        GameObject.Find("Canvas/Singleplayer/Button_Login_Single").SetActive(false);
        GameObject.Find("Canvas/Singleplayer/Button_Return").SetActive(false);

        GameObject.Find("Canvas/Multiplayer/Button_Multiple").SetActive(true);
        GameObject.Find("Canvas/Multiplayer/NameSet").SetActive(false);

        GameObject.Find("Canvas/GameQuit/Button_Quit").SetActive(true);     
    }


    //创建完角色 下一步 创建房间
    public void Button_Next_RoomSet()
    {
        GameObject.Find("Canvas/Multiplayer/RoomSet").SetActive(true); 
        GameObject.Find("Canvas/Multiplayer/NameSet").SetActive(false);       
        //GameObject.Find("Canvas/Multiplayer/PlayerSet").SetActive(false);
    }

    //返回创建角色名
    public void Button_Return_NameSet()
    {
        GameObject.Find("Canvas/Multiplayer/NameSet").SetActive(true);
        GameObject.Find("Canvas/Multiplayer/RoomSet").SetActive(false);
        //GameObject.Find("Canvas/Multiplayer/PlayerSet").SetActive(false);
    }

}
