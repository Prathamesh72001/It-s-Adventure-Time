using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Text HudcoinText;
    public Text HuddiamondText;
    public Text HudbulletText;
    public Text HudkeyText;
    public Text HudhealthText;
    public Text CompletecoinText;
    public Text CompletediamondText;
    public Text CompletehealthText;
    public Sprite OneStarSprite;
    public Sprite TwoStarSprite;
    public Sprite ThreeStarSprite;
    public Image StarImage;
    public int score=0;
    public float health=5f;
    public int diamond=0;
    public int key=0;
    public int bullet=0;
    public int max_score;
    public float max_health;
    public int max_diamond;
    public int min_score;
    public float min_health;
    public int min_diamond;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        init();    
    }

    void init(){
        HudhealthText.text="X " + health;
        HudcoinText.text = "X " + score;
        HuddiamondText.text = "X " + diamond;
        HudkeyText.text = "X " + key;
        HudbulletText.text = "X " + bullet;
    }

    public void ChangeScore(int coinValue)
    {
        score += coinValue;
        HudcoinText.text = "X " + score;
    }

    public void ChangeDiamond(int diamondValue)
    {
        diamond += diamondValue;
        HuddiamondText.text = "X " + diamond;
    }

    public void ChangeKey(int keyValue)
    {
        key += keyValue;
        HudkeyText.text = "X " + key;
    }

     public void IncreaseBullet(int bulletValue)
    {
        bullet += bulletValue;
        HudbulletText.text = "X " + bullet;
    }

    public void DecreaseBullet(int bulletValue)
    {
        bullet -= bulletValue;
        HudbulletText.text = "X " + bullet;
    }

    public void IncreaseHealth(float healthValue)
    {
        health += healthValue;
        HudhealthText.text = "X " + health;
    }

    public void DecreaseHealth(float healthValue)
    {
        
        health -= healthValue;
        if(health==0){
            Player.instance.GameOverCode();
        }else{
        HudhealthText.text = "X " + health;
        }
    }

    public void GameCompleteCode(){
        if(key>0){
        Time.timeScale = 0;
                        ButtonsManager.instance.PauseMusic();

                        ButtonsManager.instance.GameComplete();
                        Player.instance.HudPanel.SetActive(false);
        if(health>=max_health && diamond>=max_diamond && score>=max_score){
            StarImage.sprite=ThreeStarSprite;
            ButtonsManager.instance.GameCompleteCode(3);

        }
        else if(health<min_health && diamond<min_diamond && score<min_score){
            StarImage.sprite=OneStarSprite;
            ButtonsManager.instance.GameCompleteCode(1);

        }
        else{
            StarImage.sprite=TwoStarSprite;
            ButtonsManager.instance.GameCompleteCode(2);

        }
        CompletehealthText.text="X " + health;
        CompletecoinText.text = "X " + score;
        CompletediamondText.text = "X " + diamond;
         Player.instance.GameCompletePanel.SetActive(true);
        }
    }

}
