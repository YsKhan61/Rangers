using BTG.Factory;
using BTG.Utilities;
using System;
using UnityEngine;


namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "Effect Factory Container", menuName = "ScriptableObjects/Factory/EffectFactoryContainerSO")]
    public class EffectFactoryContainerSO : FactoryContainerSO<EffectView>
    {
    }
}