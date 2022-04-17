using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public void Collected()
    {
        if(this.tag == "Gold")
        {
            FindObjectOfType<PlayerMovement>().Count_Gold();
        }
        
        else if(this.tag == "Heart")
        {
            FindObjectOfType<PlayerAttack>().Count_Heart();
        }
        this.gameObject.SetActive(false);//"销毁"
    }
}
