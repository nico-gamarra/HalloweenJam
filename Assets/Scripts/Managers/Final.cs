using System;
using UnityEngine;

public class Final : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        player.GetPlayerAnimations().FinalAnimation();
        GameManager.instance.GetAudioManager().PlayAudioWithFade(AudioManager.MusicList.Endgame);
    }
}
