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

    public void Init()
    {
        foreach (MachineType machineType in Enum.GetValues(typeof(MachineType)))
        {
            _nbMachineAvailables[machineType] = 5;
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
        var sucess = _mapManager.PlaceMachine(pos, _selectedType.Value);

        if (!sucess)
            return;

        _nbMachineAvailables[_selectedType.Value]--;

        EmitSignal(nameof(nb_machine_changed), _selectedType.Value, _nbMachineAvailables[_selectedType.Value]);
    }

    public void DestroyMachine(MachineType machineType, Vector2i pos)
    {

        //TODO: call map manager to destroy the machine

        _nbMachineAvailables[machineType]++;

        EmitSignal(nameof(nb_machine_changed), machineType, _nbMachineAvailables[machineType]);
    }
}