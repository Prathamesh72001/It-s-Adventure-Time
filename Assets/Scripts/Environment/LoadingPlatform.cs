using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPlatform : MonoBehaviour
{
    public bool isLoading;
    BoxCollider2D boxCollider2D;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject unloading;
    public float delay = 3;
    float timer;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            isLoading = !isLoading;
            if (isLoading)
            {
                loading.SetActive(false);
                unloading.SetActive(true);
                boxCollider2D.isTrigger = true;
            }
            else
            {
                loading.SetActive(true);
                unloading.SetActive(false);
                boxCollider2D.isTrigger = false;
            }

        }
       

    }

    IEnumerator ChangeState(bool isLoading)
    {
        yield return new WaitForSecondsRealtime(3);
        if (isLoading)
        {
            loading.SetActive(false);
            unloading.SetActive(true);
            boxCollider2D.isTrigger = true;
        }else
        {
            loading.SetActive(true);
            unloading.SetActive(false);
            boxCollider2D.isTrigger = false;
        }
    }
}
