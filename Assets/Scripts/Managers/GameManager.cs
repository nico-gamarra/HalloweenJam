using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private Animator animator;
    
    private AudioManager _audioManager;
    
    private void OnEnable()
    {
        DeathEvent.OnPlayerDeath += RestartLevel;
    }

    private void OnDisable()
    {
        DeathEvent.OnPlayerDeath -= RestartLevel;
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _audioManager = GetComponent<AudioManager>();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FadeInAnimation()
    {
        animator.Play("FadeIn");
    }
    
    public void FadeOutAnimation()
    {
        animator.Play("FadeOut");
    }

    public AudioManager GetAudioManager() => _audioManager;
}
