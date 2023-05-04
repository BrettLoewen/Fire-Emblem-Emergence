using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public static class DataManager
{
    public static Customization[] GetCustomizationOptions()
    {
        return Resources.LoadAll<Customization>("Unit Customization");
    }
}