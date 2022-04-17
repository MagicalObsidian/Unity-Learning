using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    void Awake()
    {
        //绑定 player 的 Transform 即镜头跟随角色
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    } 
    // Update is called once per frame
    void Update()
    {
        //镜头跟随角色移动
        this.transform.position = new Vector3(player.position.x, player.position.y, -10f);
    }
}
