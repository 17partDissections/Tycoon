using UnityEngine;
using System;
using System.Collections.Generic;
[Serializable] public struct WorkerStruct
{
    public float WorkingTime;
    public Transform SpawnTransform;
    public List<FabricAbstraction> Fabrics;
    public Transform ShowcaseTransform;
}
