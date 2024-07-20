using UnityEngine;
[CreateAssetMenu(fileName = "CharacterBehavior", menuName = "ScriptableObjects/BehaviorMap", order = 1)]
public class BehaviorMap : ScriptableObject
{
    public bool CustomeRigidbody = false;
    public bool Movement = false;
    public bool JumpBuffering = false;
    public bool Jumping = false;
    public bool ViewControll = false;
    public bool Interaction = false;
    public bool PickUp = false; 
    public bool Sprinting = false;
}