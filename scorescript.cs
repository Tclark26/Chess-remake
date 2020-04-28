using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scorescript : MonoBehaviour
{

    public static int scorevalue = 0;
    Text score;


    void Start() { 
        score = GetComponent<Text>();
    }

    void Update() { 
        score.text = "Score:" + scorevalue;
    }
}
    
        
    
    

