using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    Animator anim;
    bool isChestOpened=false;
    [SerializeField] private GameObject chestPanel; 
    public int coinCount;
    public int healthCount;
    public int bulletCount;
    public int keyCount;
    public int diamondCount;
    public Text cointText; 
    public Text healthText;
    public Text bulletText;
    public Text keyText;
    public Text diamondText;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cointText.text=""+coinCount;
        healthText.text=""+healthCount;
        bulletText.text=""+bulletCount;
        keyText.text=""+keyCount;
        diamondText.text=""+diamondCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("player") && !isChestOpened){
            isChestOpened=true;
            StartCoroutine(startAnimation());
        }
    }

    IEnumerator startAnimation(){
        Player.instance.isHurt=true;
    PlayerManager.instance.ChangeScore(coinCount);
    PlayerManager.instance.ChangeKey(keyCount);
    PlayerManager.instance.IncreaseBullet(bulletCount);
    PlayerManager.instance.IncreaseHealth(healthCount);
    PlayerManager.instance.ChangeDiamond(diamondCount);
    anim.SetBool("opening", true);
    yield return new WaitForSeconds(1.125f);

            Player.instance.HudPanel.SetActive(false);

    anim.SetBool("opening", false);
    anim.SetBool("open", true);

    chestPanel.SetActive(true);

         ButtonsManager.instance.PauseMusic();

     ButtonsManager.instance.ChestOpened();

    yield return new WaitForSeconds(3f);
    chestPanel.SetActive(false);
    Player.instance.HudPanel.SetActive(true);
    ButtonsManager.instance.PlayMusic();
    Player.instance.isHurt=false;
  }
}
