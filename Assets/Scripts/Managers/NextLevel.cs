using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    
    private void OnTriggerExit2D(Collider2D other)
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
