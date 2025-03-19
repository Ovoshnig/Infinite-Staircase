using System;
using UnityEngine;

[Serializable]
public class PlayerSettings
{
    [field: SerializeField, Min(0f)] public float WalkSpeed { get; private set; } = 3.5f;
    [field: SerializeField, Min(0f)] public float RunSpeed { get; private set; } = 8f;
    [field: SerializeField, Min(1f)] public float SlewSpeed { get; private set; } = 5f;
    [field: SerializeField, Min(1f)] public float JumpForce { get; private set; } = 5f;
    [field: SerializeField, Min(1f)] public float GravityForce { get; private set; } = 9.81f;
}
