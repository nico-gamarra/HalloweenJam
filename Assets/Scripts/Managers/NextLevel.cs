using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(Fade());
    }
    
    private IEnumerator Fade()
    {
        GameManager.instance.FadeOutAnimation();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextLevelName);
    }
}
