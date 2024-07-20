using UnityEngine;
[CreateAssetMenu(fileName = "ViewVariables", menuName = "ScriptableObjects/ViewVariables", order = 1)]
public class SO_ViewVariables : ScriptableObject
{
    public bool InvertLookDirection = false;    
    [Range(0.1f,100)]public float Sensetivity = 0.1f;
    [Range(0, 1f)]public float LerpValue = 0.9f;
    [Range(-90,0f)]public float LowerdViewClamp;
    [Range(0, 90)] public float UpperdViewClamp;
}
