using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public Button[] LevelButtons;
    
    public Sprite ZeroStarSprite;
    public Sprite OneStarSprite;
    public Sprite TwoStarSprite;
    public Sprite ThreeStarSprite;
    public static LevelManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance==null)
            instance=this;
        
        var level=PlayerPrefs.GetInt("levelReached",1);
        Debug.Log("levelReached"+level);
        for(int i=0;i<level;i++){
            LevelButtons[i].interactable=true;
            LevelButtons[i].transform.GetChild(0).gameObject.SetActive(true);
            LevelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
            LevelButtons[i].transform.GetChild(2).gameObject.SetActive(true);
            LevelData l=SaveSystem.Load(i+1);
            if(l!=null){
                switch(l.stars){
                    case 1:{
                        LevelButtons[i].transform.GetChild(2).gameObject.GetComponent<Image>().sprite=OneStarSprite;
                        break;
                    }
                    case 2:{
                        LevelButtons[i].transform.GetChild(2).gameObject.GetComponent<Image>().sprite=TwoStarSprite;
                        break;
                    }
                    case 3:{
                        LevelButtons[i].transform.GetChild(2).gameObject.GetComponent<Image>().sprite=ThreeStarSprite;
                        break;
                    }
                    default:{
                        LevelButtons[i].transform.GetChild(2).gameObject.GetComponent<Image>().sprite=ZeroStarSprite;
                        break;
                    }
                }
            }else{
                LevelButtons[i].transform.GetChild(2).gameObject.GetComponent<Image>().sprite=ZeroStarSprite;
            }
        }
    }

}
