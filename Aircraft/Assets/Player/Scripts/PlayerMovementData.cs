using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Player/Movement", order = 2)]
public class PlayerMovementData : ScriptableObject
{
    public float MoveSpeed = 2.0f;

    public Sprite RobotNorth;
    public Sprite RobotNorthWest;
    public Sprite RobotNorthEast;
    public Sprite RobotSouth;
    public Sprite RobotSouthWest;
    public Sprite RobotSouthEast;
    public Sprite RobotWest;
    public Sprite RobotEast;
}