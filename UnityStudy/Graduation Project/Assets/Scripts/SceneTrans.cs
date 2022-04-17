using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneTrans : MonoBehaviour
{
    public GameObject btn_NextLevel;//获取 进入下一关 的按钮UI

    private void OnTriggerEnter2D(Collider2D other) //触发
    {
        if(other.tag == "Player")
        {
            btn_NextLevel.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other) //离开停止
    {
        if(other.tag == "Player")
        {
            btn_NextLevel.SetActive(false);
        }
    }   


}
