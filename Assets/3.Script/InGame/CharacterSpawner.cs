using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class CharacterSpawner : BehaviourSingleton<CharacterSpawner>
{
    protected override bool IsDontDestroy() => false;
}