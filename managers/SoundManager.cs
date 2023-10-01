using Godot;
using System;

public class SoundManager : Node
{
    private AudioStreamPlayer _audioStreamPlayerMusic;
    private AudioStreamPlayer _audioStreamPlayerMachineBaseShort;
    private AudioStreamPlayer _audioStreamPlayerPigSpawn1;
    private AudioStreamPlayer _audioStreamPlayerPigSpawn2;
    private AudioStreamPlayer _audioStreamPlayerPigSpawn3;
    private bool _isMusicMuted = false;
    private bool _areEffectMuted = false;

    public override void _Ready()
    {
        _audioStreamPlayerMusic = GetNode<AudioStreamPlayer>($"AudioStreamPlayerMusic");
        _audioStreamPlayerMachineBaseShort = GetNode<AudioStreamPlayer>($"AudioStreamPlayerMachineBaseShort");
        _audioStreamPlayerPigSpawn1 = GetNode<AudioStreamPlayer>($"AudioStreamPlayerPigSpawn1");
        _audioStreamPlayerPigSpawn2 = GetNode<AudioStreamPlayer>($"AudioStreamPlayerPigSpawn2");
        _audioStreamPlayerPigSpawn3 = GetNode<AudioStreamPlayer>($"AudioStreamPlayerPigSpawn3");
    }

    public void Init()
    {
        _audioStreamPlayerMusic.Bus = "Music";
        _audioStreamPlayerMachineBaseShort.Bus = "Effects";
        _audioStreamPlayerPigSpawn1.Bus = "Effects";
        _audioStreamPlayerPigSpawn2.Bus = "Effects";
        _audioStreamPlayerPigSpawn3.Bus = "Effects";
    }

    public void PlayMusic()
    {
        GD.Print("Playing music");
        _audioStreamPlayerMusic.Play();
    }

    public void PlayMachine()
    {
        _audioStreamPlayerMachineBaseShort.Play();
    }

    public void PlayPigSpawn()
    {
        var res = RandomGeneratorService.Random.RandiRange(1, 3);
        switch (res)
        {
            case 1: _audioStreamPlayerPigSpawn1.Play(); break;
            case 2: _audioStreamPlayerPigSpawn2.Play(); break;
            case 3: _audioStreamPlayerPigSpawn3.Play(); break;
        }
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
