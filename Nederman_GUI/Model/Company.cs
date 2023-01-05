using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nederman_GUI.Model
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public List<string> JobInstrutionsFileName { get; set; }
        public List<byte[]> JobInstrutions { get; set; }
        public List<string> RapportsFileName { get; set; }
        public List<byte[]> Rapports { get; set; }     
    }
}
