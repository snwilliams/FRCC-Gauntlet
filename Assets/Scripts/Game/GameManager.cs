﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerSpawns))]
public class GameManager : MonoBehaviour {
    public CameraFollowController CameraFollow;

    public PlayerSelectorData PlayerSelector;
    public PlayerSpawns PlayerSpawns;
    public HudManager HudManager { get; set; }
    public MissleManager MissleManager { get; set; }

    public PlayerControllerDef[] ControllerDefs;

    private AveragePlayerPosition _averagePlayerPos;

	// Use this for initialization
	void Start () {
        HudManager = FindObjectOfType<HudManager>();
        MissleManager = FindObjectOfType<MissleManager>();

        HookupCameraFollowSystem();
        SpawnPlayers();
	}

    void SpawnPlayers() {
        for (int i = 0; i < 4; i++)
        {
            var playerDef = PlayerSelector.GetPlayerDef(i);
            if (playerDef != null)
            {
                playerDef.ControllerDef = ControllerDefs[i];

                // create a player at the right spawn
                var spawn = PlayerSpawns.Spawns[i];
                var player = Instantiate(
                    playerDef.PlayerPrefab,
                    spawn.position,
                    spawn.rotation
                );

                player.GetComponentInChildren<PlayerAnimationEvents>().MissleManager = MissleManager;

                _averagePlayerPos.PlayerTransforms.Add(player.transform);
            }
        }
    }

    void HookupCameraFollowSystem() {
        var go = new GameObject("AveragePlayerPosition");
        _averagePlayerPos = go.AddComponent<AveragePlayerPosition>();
        CameraFollow.target = go.transform;
    }
}
