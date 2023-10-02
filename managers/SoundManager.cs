using Godot;
using System;

public class SoundManager : Node
{
    private AudioStreamPlayer _audioStreamPlayerMusic;
    private AudioStreamPlayer _audioStreamPlayerBgSound;
    private AudioStreamPlayer _audioStreamPlayerMachineBaseShort;
    private AudioStreamPlayer _audioStreamPlayerMachineFood;
    private AudioStreamPlayer _audioStreamPlayerMachineWater;
    private AudioStreamPlayer _audioStreamPlayerPigScream;
    private AudioStreamPlayer _audioStreamPlayerPigSpawn1;
    private AudioStreamPlayer _audioStreamPlayerPigSpawn2;
    private AudioStreamPlayer _audioStreamPlayerPigSpawn3;
    private AudioStreamPlayer _audioStreamPlayerClick;
    private AudioStreamPlayer _audioStreamPlayerClick2;
    private AudioStreamPlayer _audioStreamPlayerClick3;
    private bool _isMusicMuted = false;
    private bool _areEffectMuted = false;

    public override void _Ready()
    {
        _audioStreamPlayerMusic = GetNode<AudioStreamPlayer>($"AudioStreamPlayerMusic");
        _audioStreamPlayerBgSound = GetNode<AudioStreamPlayer>($"AudioStreamBgSound");
        _audioStreamPlayerMachineBaseShort = GetNode<AudioStreamPlayer>($"AudioStreamPlayerMachineBaseShort");
        _audioStreamPlayerMachineFood = GetNode<AudioStreamPlayer>($"AudioStreamPlayerMachineFood");
        _audioStreamPlayerMachineWater = GetNode<AudioStreamPlayer>($"AudioStreamPlayerMachineWater");
        _audioStreamPlayerPigScream = GetNode<AudioStreamPlayer>($"AudioStreamPlayerPigScream");
        _audioStreamPlayerPigSpawn1 = GetNode<AudioStreamPlayer>($"AudioStreamPlayerPigSpawn1");
        _audioStreamPlayerPigSpawn2 = GetNode<AudioStreamPlayer>($"AudioStreamPlayerPigSpawn2");
        _audioStreamPlayerPigSpawn3 = GetNode<AudioStreamPlayer>($"AudioStreamPlayerPigSpawn3");
        _audioStreamPlayerClick = GetNode<AudioStreamPlayer>($"AudioStreamPlayerClick");
        _audioStreamPlayerClick2 = GetNode<AudioStreamPlayer>($"AudioStreamPlayerClick2");
        _audioStreamPlayerClick3 = GetNode<AudioStreamPlayer>($"AudioStreamPlayerClick3");

    }

    public void Init()
    {
        _audioStreamPlayerMusic.Bus = "Music";
        _audioStreamPlayerBgSound.Bus = "Effects";
        _audioStreamPlayerMachineBaseShort.Bus = "Effects";
        _audioStreamPlayerMachineFood.Bus = "Effects";
        _audioStreamPlayerMachineWater.Bus = "Effects";
        _audioStreamPlayerPigScream.Bus = "Effects";
        _audioStreamPlayerPigSpawn1.Bus = "Effects";
        _audioStreamPlayerPigSpawn2.Bus = "Effects";
        _audioStreamPlayerPigSpawn3.Bus = "Effects";
    }

    public void PlayMusic()
    {
        _audioStreamPlayerMusic.Play();
        //_audioStreamPlayerBgSound.Play();
    }

    public void PlayMachine()
    {
        _audioStreamPlayerMachineBaseShort.Play();
    }

    public void PlayMachineWater()
    {
        _audioStreamPlayerMachineWater.Play();
    }

    public void PlayMachineFood()
    {
        _audioStreamPlayerMachineFood.Play();
    }

    public void PlayPigScream()
    {
        if (!_audioStreamPlayerPigScream.Playing)
            _audioStreamPlayerPigScream.Play();
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

    public void PlayClick()
    {
        var res = RandomGeneratorService.Random.RandiRange(1, 3);
        switch (res)
        {
            case 1: _audioStreamPlayerClick.Play(); break;
            case 2: _audioStreamPlayerClick2.Play(); break;
            case 3: _audioStreamPlayerClick3.Play(); break;
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
