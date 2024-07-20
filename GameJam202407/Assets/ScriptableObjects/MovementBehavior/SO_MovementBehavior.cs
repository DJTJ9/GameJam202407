using UnityEngine;
[CreateAssetMenu(fileName = "MovementVariables", menuName = "ScriptableObjects/MovementVariables", order = 1)]
public class SO_MovementBehavior : ScriptableObject
{
    [Header("Max Speed Variables")]
    [Range(0, 5)] public float GroundNorthSpeed; 
    [Range(0, 5)] public float GroundEastSpeed;  
    [Range(0, 5)] public float GroundSouthSpeed; 
    [Range(0, 5)] public float GroundWestSpeed;

    [Header("Sprinting Variable")]
    [Range(5,8)]public float SprintNorthSpeed;
    [Range(5,8)]public float SprintEastSpeed;
    [Range(5,8)]public float SprintSouthSpeed;
    [Range(5,8)]public float SprintWestSpeed;

    [Header("Air Force")]
    [Range(0, 10)] public float AirNorthForce;
    [Range(0, 10)] public float AirEastForce;
    [Range(0, 10)] public float AirSouthForce;
    [Range(0, 10)] public float AirWestForce;

    [Header("Lerp Variables")]
    [Range(0.01f, 1)] public float NorthSouthLerp;
    [Range(0.01f, 1)] public float EastWestLerp;
    [Range(0.01f, 1)] public float NorthSouthSprintLerp;
    [Range(0.01f, 1)] public float WestEastSprintLerp;

    [Header("Max Speed Variables")]
    public float MaxGroundSpeed;
    public float MaxSprintingSpeed;
    public float MaxAirSpeeds;
}
