using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public abstract void Save<T>(T o);

    public abstract T Load<T>();
}
