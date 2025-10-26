using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Paneles del menú")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Nombre de la escena del juego")]
    [SerializeField] private string gameSceneName = "Level_1";

    private void Start()
    {
        GameManager.instance.GetAudioManager().PlayMusic(AudioManager.MusicList.Menu);
    }

    // --- Botón JUGAR ---
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
        GameManager.instance.GetAudioManager().PlayMusic(AudioManager.MusicList.Game);
    }

    // --- Botón CRÉDITOS ---
    public void ShowCredits()
    {
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    // --- Botón VOLVER ---
    public void BackToMenu()
    {
        creditsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    // --- Botón SALIR ---
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}