using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Text coinText;
    public Text diamondText;
    public Text bulletText;
    public Text keyText;
    public Text healthText;
    int score;
    float health=5f;
    int diamond;
    int key;
    int bullet;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        IncreaseHealth(0f);
    }

    public void ChangeScore(int coinValue)
    {
        score += coinValue;
        coinText.text = "X " + score;
    }

    public void ChangeDiamond(int diamondValue)
    {
        diamond += diamondValue;
        diamondText.text = "X " + diamond;
    }

    public void ChangeKey(int keyValue)
    {
        key += keyValue;
        keyText.text = "X " + key;
    }

     public void IncreaseBullet(int bulletValue)
    {
        bullet += bulletValue;
        bulletText.text = "X " + bullet;
    }

    public void DecreaseBullet(int bulletValue)
    {
        bullet -= bulletValue;
        bulletText.text = "X " + bullet;
    }

    public void IncreaseHealth(float healthValue)
    {
        health += healthValue;
        healthText.text = "X " + health;
    }

    public void DecreaseHealth(float healthValue)
    {
        
        health -= healthValue;
        if(health==0){
            Player.instance.GameOverCode();
        }else{
        healthText.text = "X " + health;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
