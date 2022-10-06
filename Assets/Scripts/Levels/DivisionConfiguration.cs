using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DivisionConfiguration
{
    public GameObject[] enemyPrefabs;
    public float moveSpeed;
    public int numberOfEnemies;
    public float timeBetweenSpawns;
    public bool isFormMoving;
    public bool isChasingPlayer;
}
[System.Serializable]
public class RotationSettings
{
    public float rotationSpeed;
    public bool isRotating;
}

[System.Serializable]
public class SmoothDeltaSettings
{
    public float smoothDelta;
    public bool smoothMovement;
}
[System.Serializable]
public class SpawnsSettings
{
    public bool endlessMove;
}

[System.Serializable]
public class FormMoveSettings
{
    public float magitude;
    public float frequency;
    public bool isMovingHorizontal;
    public bool isMovingVertical;
}

[System.Serializable]
public class EnemyAISettings
{
    public int AiChanceToReact;
    public float aiSpeed;
}

[System.Serializable]
public class VerticalMoveSettings
{
    public float speed;
    public float speedRF;
}
[System.Serializable]
public class AIChasingPlayerSettings
{
    public float chasingSpeed;
}


