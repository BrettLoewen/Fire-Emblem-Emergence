using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public static class DataManager
{
    private const string UNIT_CUTOMIZATION_PATH = "Unit Customization";

    public static Customization[] GetCustomizationOptions()
    {
        return Resources.LoadAll<Customization>(UNIT_CUTOMIZATION_PATH);
    }

    public static Customization GetCustomizationFromString(string customizationObjectName)
    {
        return Resources.Load<Customization>($"{UNIT_CUTOMIZATION_PATH}/{customizationObjectName}");
    }
}