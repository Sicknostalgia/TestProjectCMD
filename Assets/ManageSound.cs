using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ami.BroAudio;
using UniRx;

public class ManageSound : MonoBehaviour
{
    public SoundID gunSound;
    public SoundID enemyHop;
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Inspance.OnAttackPressed.Subscribe(PlayAttackSFX).AddTo(this);

    }

    void PlayAttackSFX(Vector2 audio)
    {
        BroAudio.Play(gunSound);
    }
    public void HopSound()
    {
        BroAudio.Play(enemyHop);
    }
}
