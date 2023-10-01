using Godot;
using System;
using System.Linq;

public class SelectTileButton : Control
{
    [Export]
    public MachineType MachineType;

    public bool Selected = false;

    public int NbMachineAvailable = 4;

    private TileBuilderManager _tileBuilderManager;
    private GameProgressManager _gameProgressManager;

    private ColorRect _colorRect;
    private Label _nbMachineLabel;
    private Sprite _sprite;

    public override void _Ready()
    {
        // Autoloads
        _tileBuilderManager = (TileBuilderManager)GetNode($"/root/{nameof(TileBuilderManager)}"); // Singleton
        _gameProgressManager = (GameProgressManager)GetNode($"/root/{nameof(GameProgressManager)}"); // Singleton

        _tileBuilderManager.Connect("selected_machine_changed", this, nameof(SelectedMachineChanged));
        _tileBuilderManager.Connect("nb_machine_changed", this, nameof(NbMachineChanged));
        _gameProgressManager.Connect("input_started_changed", this, nameof(InputStartedChanged));


        _colorRect = GetNode<ColorRect>("ColorRect");
        _nbMachineLabel = GetNode<Label>("Nb");
        _sprite = GetNode<Sprite>("Sprite");

        _colorRect.Visible = false;

        _sprite.Frame = (int)MachineType;
    }

    public void OnClick()
    {
        _tileBuilderManager.SelectMachine(MachineType);

        // Stop propagation
        GetTree().Root.SetInputAsHandled();
    }

    public void SelectedMachineChanged(MachineType machineSelected)
    {

        if (machineSelected == MachineType)
        {
            _colorRect.Visible = true;
            Selected = true;
        }
        else
        {
            _colorRect.Visible = false;
            Selected = false;
        }
    }

    public void NbMachineChanged()
    {
        // Recompute the button visual status
        foreach (var key in _tileBuilderManager.NbMachineAvailables.Keys.ToList())
        {
            if (key == MachineType)
            {
                NbMachineAvailable = _tileBuilderManager.NbMachineAvailables[key];
                _nbMachineLabel.Text = $"({NbMachineAvailable})";
                if (NbMachineAvailable <= 0 || _gameProgressManager.InputStarted)
                {
                    _sprite.Modulate = new Color(1f, 1f, 1f, 0.5f);
                }
                else
                {
                    _sprite.Modulate = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
    }

    public void InputStartedChanged()
    {
        // Same computation as NbMachineChanged
        NbMachineChanged();
    }

}
