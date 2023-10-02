using Godot;
using System;
using System.Collections.Generic;

public class TileBuilderManager : Node2D
{
    [Signal] public delegate void selected_machine_changed();
    [Signal] public delegate void nb_machine_changed();

    public Dictionary<MachineType, int> NbMachineAvailables = new Dictionary<MachineType, int>();
    private MachineType? _selectedType = null;

    private MapManager _mapManager;
    private SoundManager _soundManager;

    public override void _Ready()
    {
        // Autoloads
        _mapManager = (MapManager)GetNode($"/root/{nameof(MapManager)}"); // Singleton
        _soundManager = (SoundManager)GetNode($"/root/{nameof(SoundManager)}"); // Singleton
    }

    public void Init(Dictionary<MachineType, int> nbMachineAvailables)
    {
        NbMachineAvailables = nbMachineAvailables;

        EmitSignal(nameof(nb_machine_changed));
    }

    public void SelectMachine(MachineType machineType)
    {
        _selectedType = machineType;
        EmitSignal(nameof(selected_machine_changed), machineType);
    }

    public void PlaceMachine(Vector2i pos)
    {
        if (!_selectedType.HasValue || NbMachineAvailables[_selectedType.Value] <= 0)
        {
            GD.Print("PlaceMachine failed: no selected tile our out of tile of this type");
            return;
        }

        // Actually place the machine on the map
        var success = _mapManager.PlaceMachine(pos, _selectedType.Value);

        if (!success)
            return;

        _soundManager.PlayBuildOrDestroySomething();

        NbMachineAvailables[_selectedType.Value]--;

        EmitSignal(nameof(nb_machine_changed));
    }

    public void DestroyMachine(Vector2i pos)
    {
        if (!_mapManager.TryGetTileType(pos, out var tileType))
        {
            GD.Print("DestroyMachine failed: no machine at this position");
            return;
        }

        var machineType = tileType.GetMachineType();
        var success = _mapManager.DestroyMachine(pos);

        if (!success)
            return;

        _soundManager.PlayBuildOrDestroySomething();

        NbMachineAvailables[machineType]++;

        EmitSignal(nameof(nb_machine_changed));
    }

    internal void HighlightPotentialMachine(Vector2i tilePos)
    {
        if (!_selectedType.HasValue || NbMachineAvailables[_selectedType.Value] <= 0)
        {
            // No machine selected or out of this type of machine
            return;
        }

        _mapManager.HighlightPotentialMachine(tilePos, _selectedType.Value);
    }
}
