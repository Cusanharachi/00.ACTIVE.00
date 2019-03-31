using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// provides access to configuration data for the game
/// </summary>
public class ConfigurationUtillities : MonoBehaviour
{
    // gets a static configuration data 
    // so everyone shares a single file
    static ConfigurationData configurationData;

    #region Properties

    /// <summary>
    /// reference property for a random string
    /// </summary>
    public static string RandomString
    {
        get { return configurationData.RandomString; }
    }

    #endregion
}
