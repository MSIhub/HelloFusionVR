
using Fusion;

/// <summary>
/// Prototyping component for spawning Player avatars.
/// </summary>
[SimulationBehaviour(Stages = SimulationStages.Forward, Modes =  SimulationModes.Server | SimulationModes.Host)]
public class PlayerSpawnerPrototype : SpawnerPrototype<PlayerSpawnPointPrototype>, IPlayerJoined, IPlayerLeft, ISceneLoaded {

#if UNITY_EDITOR

  protected virtual void Reset() {
    var protoPlayer = FusionPrototypingPrefabs.BasicPlayer;
    if (protoPlayer)
      PlayerPrefab = protoPlayer.GetComponent<NetworkObject>();
  }


  [BehaviourButtonAction("Add Player Spawn Point Manager", false, true, nameof(_spawnPointManagerMissing))]
  private void InspectorWarnMissingSpawnPointManager() {
    AddBehaviour<PlayerSpawnPointManagerPrototype>();
  }

#endif

}





