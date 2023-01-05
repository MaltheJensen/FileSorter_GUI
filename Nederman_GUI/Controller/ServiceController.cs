using Nederman_GUI.Dto;
using Nederman_GUI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Nederman_GUI.ModelView
{
    public class ServiceController
    {
        HttpClient client;
        public ServiceController()
        {
            client = new HttpClient();
        }

        public Company GetCompany(string companyName)
        {
            string path = "http://localhost:7071/api/GetCompanyByName?name=";
            Company company = null;
            
            HttpResponseMessage response = client.GetAsync(path + companyName).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content.ReadAsStringAsync().Result;
                company = JsonConvert.DeserializeObject<Company>(jsonString);
                SaveFiles(company);
            }
            return company;
        }
        private void SaveFiles(Company company)
        {
            string pathParameter = AppDomain.CurrentDomain.BaseDirectory + @"\Files\";

            for (int i = 0; i < company.JobInstrutionsFileName.Count; i++)
            {
                var filePath = pathParameter + company.JobInstrutionsFileName[i];
                File.WriteAllBytes(filePath, company.JobInstrutions[i]);
            }
            for (int i = 0; i < company.RapportsFileName.Count; i++)
            {
                var filePath = pathParameter + company.RapportsFileName[i];
                File.WriteAllBytes(filePath, company.Rapports[i]);
            }
        }

        public void OpenFile(string fileName) 
        {
            string pathParameter = AppDomain.CurrentDomain.BaseDirectory + @"\Files\" + fileName;

            System.Diagnostics.Process.Start(pathParameter);
        }

        public string SaveNewReport(string companyName) 
        {
            string baseReportPath = AppDomain.CurrentDomain.BaseDirectory + @"Files\BaseReport\MASTER_SERVICERAPPORTER.xls";
            string newFileName = companyName + "_" + DateTime.Today.ToString("dd/MM/yyyy") + ".xls";
            string newFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Files\" + newFileName;
            
            File.Copy(baseReportPath, newFilePath, true);

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(newFilePath);

            xlApp.Visible = true;

            while (xlApp.Visible == true) { }

            var report = File.ReadAllBytes(newFilePath);
            NewReportDto newReportDto = new NewReportDto{ CompanyName = companyName,
                                                          ReportName = newFileName,
                                                          Report = report};
            PostNewReport(newReportDto);

            return newFileName;
        }

        private bool PostNewReport(NewReportDto newReportDto) 
        {
            bool createdOk;

            string useRestUrl = null;
            string jsonString = null;
            HttpResponseMessage response = null;

            useRestUrl = "http://localhost:7071/api/SaveNewReport";

            try
            {
                var uri = new Uri(string.Format(useRestUrl));
                jsonString = JsonConvert.SerializeObject(newReportDto);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                response = client.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    createdOk = true;
                }
                else
                {
                    createdOk = false;
                }
            }
            catch
            {
                createdOk = false;
            }
            return createdOk;           
        }
    }
}
