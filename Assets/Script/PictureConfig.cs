using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PictureConfig",menuName = "Create Picture Config")]
public class PictureConfig: ScriptableObject
{
    public List<Sprite> list;
    public List<Sprite> listFood, listAnimal, listFruit;
}
