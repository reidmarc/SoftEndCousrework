using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_Coursework.Classes
{
    
    public class MessageClass
    {        
        public string Header { get; set; }
        
        public string Sender { get; set; }

        public string Subject { get; set; }

        public string MessageText { get; set; }  
    }
}
