using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainType", menuName = "TBS/TerrainType")]
public class TerrainType : ScriptableObject
{
    
    [field:SerializeField] public int ID { get; private set; }
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public string Description { get; private set; }
    [field:SerializeField] public Color Colour { get; private set; }
    [field:SerializeField] public Transform Prefab { get; private set; }
    [field:SerializeField] public Sprite Icon { get; private set; }

    [field:SerializeField] public Material Material { get; private set; }

    [field: SerializeField] public bool possibleAction;
    [field: SerializeField] public bool finishedAction;

}