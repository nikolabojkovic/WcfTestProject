using ObsidianDeveloperTest.handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Hosting;

namespace ObsidianDeveloperTest
{
    [ServiceContract(Namespace = "AjaxService")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FundPerformanceService
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        
        [OperationContract]
        [WebGet]
        public string GetFundPerformance()
        {
            string filePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + @"data\PerformanceData.csv");
            List<string> results = readCsvFile(filePath);

            DealyReturns dealyReturns = parceCsvFileToList(results);

            dealyReturns.Performances = sortBydate(dealyReturns.Performances);

            dealyReturns.Performances = calculateTotalReturn(dealyReturns.Performances);
            // Serialize the results as JSON
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(dealyReturns.GetType());
            MemoryStream memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, dealyReturns);

            // Return the results serialized as JSON
            string json = Encoding.Default.GetString(memoryStream.ToArray());
            return json;
        }

        private IList<Performance> calculateTotalReturn(IList<Performance> performances)
        {
            decimal currentTotalReturn = 1 ;

            foreach(var item in performances)
            {
                currentTotalReturn *= (1 + Convert.ToDecimal(item.MonthlyReturn));
                item.TotalReturn = currentTotalReturn - 1;
            }

            return performances;
        }

        private IList<Performance> sortBydate(IList<Performance> performances)
        {
            var performanceSorted = from s in performances
                                    orderby s.PerformanceDate
                                    select s;


            return performanceSorted.ToList();
        }

        private DealyReturns parceCsvFileToList(List<string> csvFileContent)
        {
            DealyReturns dealyReturns = new DealyReturns();
            dealyReturns.Performances = new List<Performance>();

            foreach (var item in csvFileContent)
            {
                var columns = item.Split(',');
                DateTime date = Convert.ToDateTime(columns[0]);
                string number = columns[1];
                dealyReturns.Performances.Add(new Performance
                {
                    PerformanceDate = date,
                    MonthlyReturn = number
                });
            }

            return dealyReturns;
        }

        private List<string> readCsvFile(string csvFile)
        {
            List<string> result = new List<string>();
            string line = string.Empty;
            bool skipHeader = true;

            StreamReader reader = null;

            try {
                reader = new StreamReader(csvFile);
            }
            catch (Exception ex) {
               // return ex.Message;
            }

            while ((line = reader.ReadLine()) != null)
            {
                if (skipHeader)
                {
                    skipHeader = false;
                }
                else
                {
                    result.Add(line);
                }
            }

            return result;
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
