using Godot;
using System;

public class SelectTileButton : Control
{
    [Export]
    public MachineType MachineType;

    public bool Selected = false;

    public int NbMachineAvailable = 4;

    private TileBuilderManager _tileBuilderManager;
    private ColorRect _colorRect;
    private Label _nbMachineLabel;
    private Sprite _sprite;

    public override void _Ready()
    {
        // Autoloads
        _tileBuilderManager = (TileBuilderManager)GetNode($"/root/{nameof(TileBuilderManager)}"); // Singleton

        _tileBuilderManager.Connect("selected_machine_changed", this, nameof(SelectedMachineChanged));
        _tileBuilderManager.Connect("nb_machine_changed", this, nameof(NbMachineChanged));


        _colorRect = GetNode<ColorRect>("ColorRect");
        _nbMachineLabel = GetNode<Label>("Nb");
        _sprite = GetNode<Sprite>("Sprite");

        _colorRect.Visible = false;

        _sprite.Frame = (int)MachineType;

        // Init the button
        NbMachineChanged(MachineType, 5);
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

    public void NbMachineChanged(MachineType machineSelected, int nbMachineAvailable)
    {
        if (machineSelected == MachineType)
        {
            NbMachineAvailable = nbMachineAvailable;
            _nbMachineLabel.Text = $"({nbMachineAvailable})";
        }

        if (NbMachineAvailable <= 0)
        {
            _sprite.Modulate = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            _sprite.Modulate = new Color(1f, 1f, 1f, 1f);
        }
    }

}
