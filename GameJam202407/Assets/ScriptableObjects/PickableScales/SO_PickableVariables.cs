using UnityEngine;
[CreateAssetMenu(fileName = "PickableVariables", menuName = "ScriptableObjects/PickableVariables", order = 1)]
public class SO_PickableVariables : ScriptableObject
{
    [Header("Variables when the object is hold")]
    public Vector3 HoldScale = Vector3.one;
    public Vector3 HoldRotation = Vector3.one;

    [Header("Variables when the object is dropped")]
    public Vector3 DropppedScale = Vector3.one;
    public Vector3 DroppedRotation = Vector3.one;
}
