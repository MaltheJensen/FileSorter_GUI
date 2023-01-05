using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nederman_GUI.Dto
{
    class NewReportDto
    {
        public string CompanyName { get; set; }

        public string ReportName { get; set; }

        public byte[] Report { get; set; }
    }
}
