using UnityEngine;

namespace Light
{
    [CreateAssetMenu(fileName = "Light", menuName = "ScriptableObjects/Light", order = 1)]
    public class LightColor : ScriptableObject
    {
        [SerializeField] public Color offColor;
        [SerializeField] public Color onColor;
    }
}
