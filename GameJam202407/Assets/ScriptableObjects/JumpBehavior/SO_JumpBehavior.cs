using UnityEngine;
[CreateAssetMenu(fileName = "JumpVariables", menuName = "ScriptableObjects/JumpVariables", order = 1)]
public class SO_JumpBehavior : ScriptableObject
{
    [Header("Normal Jump Variables")]
    [Range(1, 25)] public float JumpStrength = 5;
    [Range(1, 25)] public float JumpMaxSpeed = 20f;
    [Header("Double Jump Variables")]
    public bool EnableDoubleJump = false;
    [Range(1, 20)] public float DoubleJumpStrength = 4;
    [Range(1, 20)] public float MaxDoubleJumpSpeed = 15;
    [Header("Time Variables")]
    [Range(0,1)]public float jumpBufferTime = 0.1f;
    [Range(0,1)]public float jumpCooldown = 0.2f;
}