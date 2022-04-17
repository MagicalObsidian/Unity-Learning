using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public Toggle isWarrior;
    public Toggle isSamruai;
    public Text Text_PlayerChoose;

    //检查选中的 Toggle
    public void ActiveToggle()
    {
        if(isWarrior) {
            Debug.Log("Player selected Warrior");
            Text_PlayerChoose.text = "Player_Hero_Warrior";

        }
        else if(isSamruai) {
            Debug.Log("Player selected Samruai");
            Text_PlayerChoose.text = "Player_Hero_Samruai";
        }
    }

    //提交选中
    public void OnSubmit()
    {
        ActiveToggle();
    }


}
