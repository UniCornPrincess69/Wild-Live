using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestToggleLoader : TestGuiLoader
{
    public SOBool Value { get; set; }

    public Func<bool> ToggleDel { get; set; }

    public override void InvokeGuiElem() => ToggleDel.Invoke();

}
