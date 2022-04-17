using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetLaunch : MonoBehaviourPunCallbacks
{

    [Header("输入框信息")]
    public InputField roomName;
    public InputField playerName;

    
    

    //启用多人游戏网络连接设置
    public void Multiple_Connect() 
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    //重写连接服务器的方法
    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        Debug.Log("Connect Server Success");
        PhotonNetwork.JoinLobby();
    }

    //获取用户名
    /* 绑定 Button_Next_RoomSet */
    public void GetInput_PlayerName()
    {
        PhotonNetwork.NickName = playerName.text;
        Debug.Log("Get PlayerName Success :" + PhotonNetwork.NickName);
        //下一步进入创建房间的界面
        if(PhotonNetwork.InLobby)
        {
            GameObject.Find("Canvas/Multiplayer/RoomSet/RoomList").SetActive(true);
        }
    }


 
    //创建或加入房间
    public void Room_Create()
    { 
        RoomOptions opt = new RoomOptions { MaxPlayers = 2};
        PhotonNetwork.JoinOrCreateRoom(roomName.text, opt, default);
        Debug.Log("Get RoomName Success :" + roomName.text);
    }

    //进入房间
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);//进入 PVP 场景
    }
}
