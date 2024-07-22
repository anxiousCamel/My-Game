using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerData_Input : MonoBehaviour
{
    [Space(5)] public Times Time = new();
    [Space(5)] public CheckInputs CheckInput = new();

    [System.Serializable]
    public class CheckInputs
    {
        // Move
        public bool facingRight = true;
        public int direction;
        public Vector2 moveDirection;

        // Jump
        public bool inputJump;
        public bool keepPressingJump;

        // Carry
        public bool inputObjectInteraction;
        public bool keepPressingObjectInteraction;
        public bool releasedPressingObjectInteraction;

        // Inventory
        public bool inputInventory;
    }

    [System.Serializable]
    public class Times
    {
        // Jump
        public float lastInputJump;

        // Carry
        public float lastInputObjectInteraction;
        public float lastInputUpObjectInteraction;
        public float durationInputObjectInteraction;

        // Inventory
        public float lastInputInventory;
    }
}
