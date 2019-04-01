using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Draws in data for configuring game and
/// assists with constants in the game to avoid magic numbers
/// </summary>
public class ConfigurationData : MonoBehaviour
{
    #region Fields

    // used to get files for data
    const string configurationFileName = "ConfigurationData.csv";

    // other game variables


    #endregion

    #region Properties
    /// <summary>
    /// template property
    /// </summary>
    public string RandomString
    {
        get { return "<TEXT>"; }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// this pulls in data from a csv to be used as variables 
    /// in the game to avoid magic numbers in the code
    /// </summary>
    public ConfigurationData()
    {
        // sets a safe stream reader
        StreamReader inputFile = null;

        // trys to read in the file
        try
        {
            // takes the filepath and file name to create the file reader
            inputFile = File.OpenText(Path.Combine(Application.streamingAssetsPath, configurationFileName));

            // reads in names just in case
            string names = inputFile.ReadLine();

            // reads in the varaibles for the game
            string values = inputFile.ReadLine();

            // set configuration data fields with input values
            SetConfigurationDataFields(values);

        }
        catch(Exception e)
        {
            print(e.Message);
        }
        finally
        {
            // closes the file if it exists
            if (inputFile != null)
            {
                inputFile.Close();
            }
        }
    }

    #endregion

    #region methods

    /// <summary>
    /// takes the string pulled from the variables line in the csv
    /// pareses the values to get the varaible values for the game
    /// </summary>
    /// <param name="csvValues"></param>
    void SetConfigurationDataFields(string csvValues)
    {
        // assuming the order of these variables is correct
        // pull them in this order. For future development, consider a dictionary!

        // splits up all of the values to be converted to integers
        string[] varaibles = csvValues.Split(',');
    }
    #endregion
}
