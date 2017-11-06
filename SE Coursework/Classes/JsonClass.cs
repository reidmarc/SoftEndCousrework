using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SE_Coursework.Classes
{
    public class JsonClass
    {    

        public void Serialize(List<MessageClass> list)
        {
            try
            {
                // serialize JSON to a string and then write string to a file
                File.WriteAllText(@".\EustonLeisureMessages.json", JsonConvert.SerializeObject(list, Formatting.Indented));

                // Message informing the user that the file has been saved successfully
                MessageBox.Show("JSON File saved.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public List<MessageClass> Deserialize()
        {

            // read file into a string and deserialize JSON to a type
            List<MessageClass> storedListOfMessages = JsonConvert.DeserializeObject<List<MessageClass>>(File.ReadAllText(@".\EustonLeisureMessages.json"));           

            return storedListOfMessages;
        }     
    }
}
