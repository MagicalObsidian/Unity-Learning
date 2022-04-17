using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject roomName_prefab;//显示当前房间信息的按钮
    public Transform gridLayout;//显示滑动窗口的布局


    //重写更新当前房间列表信息的方法
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //base.OnRoomListUpdate(roomList);

        //移除房间信息
        for(int i = 0; i < gridLayout.childCount; i++)
        {
            //如果当前页面中的房间按钮的文本 是 之前已经存在的
            if(gridLayout.GetChild(i).gameObject.GetComponentInChildren<Text>().text == roomList[i].Name)
            {
                Destroy(gridLayout.GetChild(i).gameObject);//删除

                if(roomList[i].PlayerCount == 0) //如果当前房间人数为 0
                {
                    roomList.Remove(roomList[i]);//从 roomList 移除
                }
            }
        }

        //每次 Update 加入更新的列表信息
        foreach(var room in roomList)
        {
            //实例化新房间按钮
            GameObject newRoom = Instantiate(roomName_prefab, gridLayout.position, Quaternion.identity);
            //获取其子对象 Text 并设置当前房间列表循环的房间名
            newRoom.GetComponentInChildren<Text>().text = room.Name;
            //设置信访件按钮父级的 Tansform
            newRoom.transform.SetParent(gridLayout);
        }

    }

}
