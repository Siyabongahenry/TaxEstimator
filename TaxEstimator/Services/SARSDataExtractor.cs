using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;
using TaxEstimator.Models;

namespace TaxEstimator.Services
{
    public class SARSDataExtractor : ISARSDataExtractor
    {
        private readonly int currentTaxYear = DateTime.Now.Year +1;
        private readonly HtmlDocument _document = new();
        private List<HtmlNode> _taxTableNodes = new();


        private readonly Dictionary<int, List<TaxBracket>> _taxBracketsDic = new() {

            {2024,
                new List<TaxBracket>{
                    new TaxBracket{From =1,To = 237100,Base =0,Margin = 18 } ,
                    new TaxBracket{From =237101,To = 370500,Base =42678,Margin = 26 },
                    new TaxBracket{From =370501,To = 512800,Base =77362,Margin = 31 },
                    new TaxBracket{From =512801,To = 673000,Base =121475,Margin = 36 },
                    new TaxBracket{From =673001,To = 857900,Base =179147,Margin = 39 },
                    new TaxBracket{From =857901,To = 1817000,Base =251258,Margin = 41 },
                    new TaxBracket{From =1817001,To = 10000000000,Base =644489,Margin = 45 },
                }
            },
            {2023,
                new List<TaxBracket>{
                    new TaxBracket{From =1,To = 226000,Base =0,Margin = 18 } ,
                    new TaxBracket{From =226001,To = 353100,Base =40680,Margin = 26 },
                    new TaxBracket{From =353101,To = 488700,Base =73726,Margin = 31 },
                    new TaxBracket{From =488701,To = 641400,Base =115762,Margin = 36 },
                    new TaxBracket{From =641401,To = 817600,Base =170734,Margin = 39 },
                    new TaxBracket{From =817601,To = 1731600,Base =239452,Margin = 41 },
                    new TaxBracket{From =1731601,To = 10000000000000,Base =614192,Margin = 45 },
                }
            }
        };

        private readonly Dictionary<int, Rebate> rebateDic =new (){
            {
                2024,
                new Rebate()
                {
                   Primary=17235,
                   Secondary = 9444,
                   Tertiary =3145
                }
            },
            {
                2023,
                new Rebate()
                {
                   Primary=16425,
                   Secondary = 9000,
                   Tertiary =2997
                }
            }
        };

        public Dictionary<int, Threshold> thresholdsDic = new()
        {
            {
                2024,
                new Threshold()
                {
                    LowAge = 95750,
                    MidAge = 148217,
                    OldAge = 165689

                }
            },
            {
                2023,
                new Threshold()
                {
                    LowAge = 91250,
                    MidAge = 141250,
                    OldAge = 157900

                }
            }
        };

        public SARSDataExtractor()
        {

        }
        //first method to be call after instatiating the class
        //fetching data from sars website
        public async Task<bool> RetrieveSARSDataAsync()
        {
            if (_taxTableNodes.Count > 0) return true;

            const string url = "https://www.sars.gov.za/tax-rates/income-tax/rates-of-tax-for-individuals/";

            var response =await CallUrl(url);

            _document.LoadHtml(response);

            //find all tables from the html document
            _taxTableNodes = _document.DocumentNode.SelectNodes("//table").ToList();

            return _taxTableNodes.Count > 0;
        }
        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }

        public  TaxBracket GetIncomeTaxBracket(int taxYear, decimal annualIncome)
        {
            if(_taxBracketsDic.Count == 0)
            {
                AddTaxBrackets();
            }

            var taxBracket = _taxBracketsDic[taxYear].Find(ti=>annualIncome >=ti.From && annualIncome <= ti.To);
           
            return taxBracket ==null?new TaxBracket():taxBracket;
        }

        public decimal GetEmployeeTaxRebate(int taxYear, int age)
        {
            if(!rebateDic.ContainsKey(taxYear))
            {
                SetRebateDictionery();
            }

            Rebate rebate = rebateDic[taxYear];

            if (age < 65) return rebate.Primary;


            if(age < 75) return rebate.Primary + rebate.Secondary;

            return rebate.Primary + rebate.Secondary + rebate.Tertiary;
        }

        public decimal GetEmployeeThreshold(int taxYear,int age)
        {
            if(thresholdsDic.ContainsKey(taxYear))
            {
                SetThresholdsDictionery();
            }

            Threshold threshold = thresholdsDic[taxYear];

            if(age < 65) return threshold.LowAge;

            if (age < 75) return threshold.MidAge;

            return threshold.OldAge;

        }
        private void SetRebateDictionery()
        {
            //rebates table number from the list of sars table
            int rebatesTableNo = _taxTableNodes.Count - 2;

            //get the table rows as a list
            List<HtmlNode> rebateRowNodes = _taxTableNodes[rebatesTableNo]
                                    .SelectNodes(".//tr")
                                    .Skip(2)//skip head and year column
                                    .ToList();

            //array containing first,second and third row of rebate table data as list
            List<decimal>[] rowsData = getRowsData(rebateRowNodes);

            for(int i = 0; i < rowsData[0].Count;i++)
            {
                Rebate rebate = new Rebate()
                {
                    Year = currentTaxYear - 1,
                    Primary = rowsData[0][i], //first row column i
                    Secondary = rowsData[1][i],//second row column i
                    Tertiary = rowsData[2][i] //third row columni
                };

                rebateDic.Add(rebate.Year,rebate);
            }
        }
        public void SetThresholdsDictionery()
        {
            //rebates table number from the list of sars table
            int thresholdsTableNo = _taxTableNodes.Count - 1;

            //get the table rows as a list
            List<HtmlNode> thresholdsRowNodes = _taxTableNodes[thresholdsTableNo]
                                    .SelectNodes(".//tr")
                                    .Skip(2)//skip head and year column
                                    .ToList();

            //array containing first,second and third row of rebate table data as list
            List<decimal>[] rowsData = getRowsData(thresholdsRowNodes);

            for (int i = 0; i < rowsData[0].Count; i++)
            {
                Threshold threshold = new Threshold()
                {
                    Year = currentTaxYear - 1,
                    LowAge = rowsData[0][i], //first row column i
                    MidAge = rowsData[1][i],//second row column i
                    OldAge = rowsData[2][i] //third row columni
                };

                thresholdsDic.Add(threshold.Year, threshold);
            }
        }
        private List<decimal>[] getRowsData(List<HtmlNode> rowNodes)
        {
            //first row of rebate of threshold
            var row1 = rowNodes[0]
                            .SelectNodes(".//td")
                            .Skip(1)//skip rebate/threshold first column
                            .ToList();

            List<decimal> firstRowData = row1.Select(
                    td=>decimal.Parse(Regex.Match(Regex.Replace(td.InnerText, @"\s+", ""), @"\d+\.?\d").Value)
                ).ToList();

            //second row of rebate or threshold
            var row2 = rowNodes[1]
                            .SelectNodes(".//td")
                            .Skip(1)//skip rebate first column
                            .ToList();
            List<decimal> secondRowData = row2.Select(
                    td => decimal.Parse(Regex.Match(Regex.Replace(td.InnerText, @"\s+", ""), @"\d+\.?\d").Value)
                ).ToList();

            //third row of rebate or threshold
            var row3 = rowNodes[2]
                            .SelectNodes(".//td")
                            .Skip(1)//skip rebate first column
                            .ToList();
            List<decimal> thirdRowData = row3.Select(
                    td => decimal.Parse(Regex.Match(Regex.Replace(td.InnerText, @"\s+", ""), @"\d+\.?\d").Value)
                ).ToList();

            return new List<decimal>[] {firstRowData,secondRowData,thirdRowData };
        }
        
        private void AddTaxBrackets(int tableNo = 0)
        {
            if (tableNo >= _taxTableNodes.Count-7) return;

            //skip table if it data already exist in the tax brackets dictionary

            if(_taxBracketsDic.ContainsKey(currentTaxYear-tableNo))
            {
                AddTaxBrackets(tableNo + 1);

                return;
            }
            
            //find all cells in the tax year table, and treat each cell as a list item
            var cellsData = _taxTableNodes[tableNo]
                            .SelectNodes(".//td")
                            .ToList();

            List<TaxBracket> taxableIncomes= new();

            //getting data in all cells
            for (int i = 0; i < cellsData.Count; i += 2)
            {
                //The first cell contains salary ranges in the form 1 - 100 000 
                //With this code the range will be stored in an array like [1,100000]
                string[] incomeRange = Regex.Replace(cellsData[i].InnerText, @"\s", "").Split('a', '–');
                //Separate other tax information from the tax rates
                string[] taxInfo = Regex.Replace(cellsData[i + 1].InnerText, @"\s", "").Split("%");
                //Separate the pre-added amount of the tax from the tax rates
                string[] taxSum = taxInfo[0].Split("+");

                decimal minIncome = decimal.Parse(incomeRange[0]);
                decimal maxIncome = decimal
                    .TryParse(incomeRange[1], out maxIncome) ? maxIncome :1000000000;

                TaxBracket taxBracket = new()
                {
                    Year = currentTaxYear - tableNo,
                    From = minIncome,
                    To = maxIncome,
                    Base = taxSum.Length > 1 ? decimal.Parse(taxSum[0]) : 0,
                    Margin = taxSum.Length > 1 ? decimal.Parse(taxSum[1]) / 100 : decimal.Parse(taxSum[0]) / 100
                };

                taxableIncomes.Add(taxBracket);
            }

            int taxYear = DateTime.Now.Year+1;

            int key = taxYear - tableNo;

            _taxBracketsDic.Add(key,taxableIncomes);

            AddTaxBrackets(tableNo+1);

        }
    }
}
