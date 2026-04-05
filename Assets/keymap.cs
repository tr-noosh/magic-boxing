using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "keyMap", menuName = "ScriptableObjects/keyMap", order = 2)]
public class keymap : ScriptableObject
{
    public KeyCode left_punch = KeyCode.D;
    public KeyCode right_punch = KeyCode.K;

    public KeyCode left_dodge = KeyCode.C;
    public KeyCode right_dodge = KeyCode.M;
    public KeyCode low_dodge = KeyCode.Space;

    public bool jabHold = false;

    public KeyCode left_jab = KeyCode.S;
    public KeyCode right_jab = KeyCode.L;

}



