using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawner : NetworkBehaviour
{
    public NetworkPrefabRef PlayerPrefab;

    Dictionary<PlayerRef, NetworkObject> m_PlayerList = new Dictionary<PlayerRef, NetworkObject>();

    public override void Spawned()
    {
        var events = Runner.GetComponent<NetworkEvents>();
        events.PlayerLeft.AddListener( PlayerLeft );

        if( Runner.Config.Simulation.Topology == SimulationConfig.Topologies.Shared )
        {
            Runner.Spawn( PlayerPrefab, null, null, Runner.LocalPlayer );
        }
        else
        {
            RpcSpawnPlayer();
        }
    }

    [Rpc( RpcSources.All, RpcTargets.StateAuthority, InvokeResim = true )]
    void RpcSpawnPlayer( RpcInfo info = default )
    {
        var go = Runner.Spawn( PlayerPrefab, null, null, info.Source );
        if( m_PlayerList.ContainsKey( info.Source ) )
        {
            m_PlayerList[ info.Source ] = go;
        }
        else
        {
            m_PlayerList.Add( info.Source, go );
        }
    }

    void PlayerLeft( NetworkRunner runner, PlayerRef player )
    {
        if( m_PlayerList.ContainsKey( player ) )
        {
            runner.Despawn( m_PlayerList[ player ] );
            m_PlayerList.Remove( player );
        }
    }
}
