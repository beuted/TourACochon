using Godot;
using System;
using System.Collections.Generic;

public class TileBuilderManager : Node2D
{
    [Signal] public delegate void selected_machine_changed();
    [Signal] public delegate void nb_machine_changed();

    private Dictionary<MachineType, int> _nbMachineAvailables = new Dictionary<MachineType, int>();
    private MachineType? _selectedType = null;

    private MapManager _mapManager;


    public override void _Ready()
    {
        // Autoloads
        _mapManager = (MapManager)GetNode($"/root/{nameof(MapManager)}"); // Singleton
    }

    public void Init(Dictionary<MachineType, int> nbMachineAvailables)
    {
        _nbMachineAvailables = nbMachineAvailables;

        foreach (var key in _nbMachineAvailables.Keys)
        {
            EmitSignal(nameof(nb_machine_changed), key, _nbMachineAvailables[key]);
        }
    }

    public void SelectMachine(MachineType machineType)
    {
        _selectedType = machineType;
        EmitSignal(nameof(selected_machine_changed), machineType);
    }

    public void PlaceMachine(Vector2i pos)
    {
        if (!_selectedType.HasValue || _nbMachineAvailables[_selectedType.Value] <= 0)
        {
            GD.Print("PlaceMachine failed: no selected tile our out of tile of this type");
            return;
        }

        // Actually place the machine on the map
        var sucess = _mapManager.PlaceMachine(pos, _selectedType.Value, Direction.Right);

        if (!sucess)
            return;

        _nbMachineAvailables[_selectedType.Value]--;

        EmitSignal(nameof(nb_machine_changed), _selectedType.Value, _nbMachineAvailables[_selectedType.Value]);
    }

    public void DestroyMachine(Vector2i pos)
    {
        if (!_mapManager.TryGetTileType(pos, out var tileType))
        {
            GD.Print("DestroyMachine failed: no machine at this position");
            return;
        }

        var machineType = tileType.GetMachineType();
        var sucess = _mapManager.DestroyMachine(pos);

        if (!sucess)
            return;

        _nbMachineAvailables[machineType]++;

        EmitSignal(nameof(nb_machine_changed), machineType, _nbMachineAvailables[machineType]);
    }
}
