using System;
using UnityEngine;

public class Final : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        player.GetPlayerAnimations().FinalAnimation();
        GameManager.instance.GetAudioManager().PlayAudioWithFade(AudioManager.MusicList.Endgame);
    }
}
