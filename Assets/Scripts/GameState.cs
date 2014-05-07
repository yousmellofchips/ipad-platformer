using UnityEngine;
using System.Collections;

/// <summary>
/// Game state - represents the current mode / state of execution the game is in.
/// </summary>
public enum GameState {
	Init,
	TitleScreen,
	HiScores,
	Credits,
	StartGame,
	InitLevel,
	InGame,
	LoseLife,
	LevelCompleted,
	GameOver,
	GameCompleted
}
