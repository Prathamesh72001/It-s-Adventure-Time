using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public Button[] LevelButtons;
    // Start is called before the first frame update
    void Start()
    {
        int levelReached=PlayerPrefs.GetInt("LevelReached",1);
        for(int i=0;i<LevelButtons.Length;i++){
            if(i+1>levelReached){
            LevelButtons[i].interactable=false;
            }
        }
    }

    public void LevelComplete(){
        int levelReached=PlayerPrefs.GetInt("LevelReached",1)+2;
        PlayerPrefs.SetInt("LevelReached",levelReached);
        ButtonsManager.instance.LevelButtons(levelReached);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
