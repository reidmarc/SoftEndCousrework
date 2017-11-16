//////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////// Class JsonClass ///////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////// Code Written By: 03001588 //////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////

// Description:
// This class provides the methods Serialize and Deserialize, which allows the application to read from
// and write to json files.

#region Usings

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

#endregion

namespace SE_Coursework.Classes
{
    public class JsonClass
    {
        #region Serialize

        /// <summary>
        /// This method stores the List that is passed in as a JSON file at the location of the path that is passed in.
        /// </summary>
        /// <param name="list">This list contains the MessageClass objects which are formatted in the JSON format</param>
        /// <param name="path">This the path for where to store the JSON file created</param>
        public void Serialize(List<MessageClass> list, string path)
        {
            try
            {
                // serialize JSON to a string and then write string to a file
                File.WriteAllText(path, JsonConvert.SerializeObject(list, Formatting.Indented));

                // Message informing the user that the file has been saved successfully
                MessageBox.Show("JSON File saved.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// This method reads the JSON file and stores the contents of the JSON file in a List
        /// </summary>
        /// <returns>Returns the list that the contents of the JSON file have been stored in</returns>
        public List<MessageClass> Deserialize()
        {            
            // read file into a string and deserialize JSON to a type
            List<MessageClass> storedListOfMessages = JsonConvert.DeserializeObject<List<MessageClass>>(File.ReadAllText(@".\EustonLeisureMessages.json"));

            return storedListOfMessages;
        }

        #endregion
    }
}
