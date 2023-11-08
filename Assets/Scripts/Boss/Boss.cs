using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public static Boss instance;
    Rigidbody2D rb;
    Animator anim;
    public float health=100f;
    [SerializeField] private Slider healthBar; 
    [SerializeField] private Text healthText; 

    public Transform player;
    public bool isFlipped=false;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        
        healthBar.maxValue=health;
        healthBar.value=health;
        healthText.text= health+"";
    }

    public void LookAtPlayer(){
        Vector3 flipped=transform.localScale;
        flipped.z*=-1f;

        if(transform.position.x>player.position.x&& isFlipped){
            transform.localScale=flipped;
            transform.Rotate(0f,180f,0f);
            isFlipped=false;
        }
        else if(transform.position.x<player.position.x&& !isFlipped){
            transform.localScale=flipped;
            transform.Rotate(0f,180f,0f);
            isFlipped=true;
        }
    }

    public void Hurt(float healthValue){
        health -= healthValue;
        if(health<=0f){
            StartCoroutine(GettingDie());
        }else{
        healthBar.value=health;
        healthText.text=health+"";
        StartCoroutine(GettingHurt());
        }
    }

    IEnumerator GettingHurt(){
        anim.SetBool("is_hurt",true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("is_hurt",false);
    }

    IEnumerator GettingDie(){
        anim.SetBool("is_die",true);
        yield return new WaitForSeconds(0.3f);
        Player.instance.BossHealthBar.SetActive(false);
        Player.instance.BossBattleEndArea.SetActive(false);    
        Player.instance.BossBattleStartArea.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        Player.instance.BossBattleCamera.SetActive(false);
        ButtonsManager.instance.EndBossBattle();
        Destroy(gameObject);
    }

    
}
