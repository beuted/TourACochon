using Godot;
using System;

public class SoundManager : Node
{
    private AudioStreamPlayer _audioStreamPlayerMusic;

    private bool _isMusicMuted = false;
    private bool _areEffectMuted = false;

    public override void _Ready()
    {
        _audioStreamPlayerMusic = GetNode<AudioStreamPlayer>($"AudioStreamPlayerMusic"); // Singleton
    }

    public void Init()
    {
        _audioStreamPlayerMusic.Bus = "Music";
    }

    public void PlayMusic()
    {
        GD.Print("Playing music");
        _audioStreamPlayerMusic.Play();
    }

    public bool ToggleMuteMusic()
    {
        if (!_isMusicMuted)
        {
            AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), true);
        }
        else
        {
            AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), false);
        }

        _isMusicMuted = !_isMusicMuted;

        return !_isMusicMuted;
    }

    public void ToggleMuteEffects()
    {
        if (!_areEffectMuted)
        {
            AudioServer.SetBusMute(AudioServer.GetBusIndex("Effects"), true);
        }
        else
        {
            AudioServer.SetBusMute(AudioServer.GetBusIndex("Effects"), false);
        }

        _areEffectMuted = !_areEffectMuted;
    }
}
