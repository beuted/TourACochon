using Godot;
using System;

public class MuteButton : TextureButton
{
    private SoundManager _soundManager;
    private Texture _buttonNormalTexture;
    private Texture _buttonMuteTexture;
    private TextureRect _textureRect;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _textureRect = GetNode<TextureRect>("TextureRect");

        _buttonNormalTexture = ResourceLoader.Load<Texture>("res://assets/graphics/button-normal.png");
        _buttonMuteTexture = ResourceLoader.Load<Texture>("res://assets/graphics/button-mute.png");
        _soundManager = (SoundManager)GetNode($"/root/{nameof(SoundManager)}"); // Singleton
    }

    void OnClick()
    {
        if (_soundManager.ToggleMuteMusic())
            _textureRect.Texture = _buttonNormalTexture;
        else
            _textureRect.Texture = _buttonMuteTexture;
    }

}