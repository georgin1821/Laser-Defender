using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DivisionConfiguration
{
    public GameObject[] enemyPrefabs;

    public int numberOfEnemies;
    public float timeBetweenSpawns;
    public float moveSpeed;
    public float rotationSpeed;

    public bool smoothMovement;
    public bool isRotating;
    public bool endlessMove;

}
[System.Serializable]
public class FormMoveSettings
{
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


