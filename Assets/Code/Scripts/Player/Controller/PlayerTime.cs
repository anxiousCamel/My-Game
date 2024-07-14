using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTime : MonoBehaviour
{
    private PlayerData_Input Input;
    private PlayerData_Collider Collider;
    private PlayerData_Mechanics Mechanics;
    private PlayerData_Physics Physic;

    void Start()
    {
        Mechanics.Time.startTimeScale = Time.timeScale;
        Mechanics.Time.startTimeFixedDelta = Time.fixedDeltaTime;
    }

    private void Awake()
    {
        Input = GetComponent<PlayerData_Input>();
        Collider = GetComponent<PlayerData_Collider>();
        Mechanics = GetComponent<PlayerData_Mechanics>();
        Physic = GetComponent<PlayerData_Physics>();
    }

    void Update()
    {
        if (Mechanics.Target.canMoveTarget)
        {
            UnityEngine.Time.timeScale = Mechanics.Time.slowTimeScale;
            UnityEngine.Time.fixedDeltaTime = Mechanics.Time.startTimeFixedDelta * Mechanics.Time.slowTimeScale;
        }
        else
        {
            UnityEngine.Time.timeScale = Mechanics.Time.startTimeScale;
            UnityEngine.Time.fixedDeltaTime = Mechanics.Time.startTimeFixedDelta;
        }
    }
}
