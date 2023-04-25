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


        private readonly Dictionary<int, List<TaxBracket>> _taxBracketsDic = new();

        private readonly Dictionary<int, Rebate> _rebateDic = new();


        private Dictionary<int, Threshold> _thresholdsDic = new();

        private Dictionary<int, List<TaxBracket>> PSFundTaxBracketDic = new();
        public SARSDataExtractor()
        {

        }
        //first method to be call after instatiating the class
        //fetching data from sars website
        public async Task<bool> RetrieveSARSDataAsync()
        {
            if (_taxTableNodes.Count > 0) return true;

            const string url = "https://www.sars.gov.za/tax-rates/income-tax/rates-of-tax-for-individuals/";

            try
            {
                var response = await CallUrl(url);

                _document.LoadHtml(response);
            }
            catch
            {
                return false;
            }

            //find all tables from the html document
            _taxTableNodes = _document.DocumentNode.SelectNodes("//table").ToList();

            SetTaxBrackets();
            SetThresholdsDictionery();
            SetRebateDictionery();
           
            

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
                SetTaxBrackets();
            }

            if (!_taxBracketsDic.ContainsKey(taxYear)) return new();

            var taxBracket = _taxBracketsDic[taxYear]
                .Find(ti=>annualIncome >=ti.From && annualIncome <= ti.To);
           
            return taxBracket ==null?new():taxBracket;
        }

        public decimal GetEmployeeTaxRebate(int taxYear, int age)
        {
            if(!_rebateDic.ContainsKey(taxYear))
            {
                SetRebateDictionery();
            }

            if (!_rebateDic.ContainsKey(taxYear)) return 0;

            Rebate rebate = _rebateDic[taxYear];

            if (age < 65) return rebate.Primary;


            if(age < 75) return rebate.Primary + rebate.Secondary;

            return rebate.Primary + rebate.Secondary + rebate.Tertiary;
        }

        public decimal GetEmployeeThreshold(int taxYear,int age)
        {
            if(!_thresholdsDic.ContainsKey(taxYear))
            {
                SetThresholdsDictionery();
            }

            if (!_thresholdsDic.ContainsKey(taxYear)) return 0;

            Threshold threshold = _thresholdsDic[taxYear];

            if(age < 65) return threshold.LowAge;

            if (age < 75) return threshold.MidAge;

            return threshold.OldAge;

        }

        public TaxBracket GetPSFundTaxBracket(int taxYear, decimal amount)
        {
            if (!PSFundTaxBracketDic.ContainsKey(taxYear)) return new TaxBracket();

            var taxBracket = PSFundTaxBracketDic[taxYear]
                .Find(ti => amount >= ti.From && amount <= ti.To);

            return taxBracket == null || amount <= 0?new():taxBracket;
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
                    Year = currentTaxYear - i,
                    Primary = rowsData[0][i], //first row column i
                    Secondary = rowsData[1][i],//second row column i
                    Tertiary = rowsData[2][i] //third row columni
                };

                _rebateDic.Add(rebate.Year,rebate);
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
                    Year = currentTaxYear - i,
                    LowAge = rowsData[0][i], //first row column i
                    MidAge = rowsData[1][i],//second row column i
                    OldAge = rowsData[2][i] //third row columni
                };

                _thresholdsDic.Add(threshold.Year, threshold);
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
                    td=>decimal.Parse(Regex.Match(Regex.Replace(td.InnerText, @"\s+", ""), @"\d+\.?\d{0,}").Value)
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
        
        private void SetTaxBrackets(int tableNo = 0)
        {
            if (tableNo >= _taxTableNodes.Count-7) return;

            //skip table if it data already exist in the tax brackets dictionary

            if(_taxBracketsDic.ContainsKey(currentTaxYear-tableNo))
            {
                SetTaxBrackets(tableNo + 1);

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
                    Margin = taxSum.Length > 1 ? decimal.Parse(taxSum[1]): decimal.Parse(taxSum[0])
                };

                taxableIncomes.Add(taxBracket);
            }

            int taxYear = DateTime.Now.Year+1;

            int key = taxYear - tableNo;

            _taxBracketsDic.Add(key,taxableIncomes);

            SetTaxBrackets(tableNo+1);

        }
        // lists
        public async Task<List<TaxBracket>> GetTaxBracketListAsync(int year)
        {
            if(!_taxBracketsDic.ContainsKey(year))
            {
               await RetrieveSARSDataAsync();
            }

            if(!_taxBracketsDic.ContainsKey(year)) return new();
            
            List<TaxBracket> lst= _taxBracketsDic[year];

            return lst;
        }

        public async Task<List<Rebate>> GetTaxRebateListAsync()
        {
            if (_rebateDic.Count == 0)
            {
                await RetrieveSARSDataAsync();
            }

            if (_rebateDic.Count == 0) return new();

            List<Rebate> lst = _rebateDic.Values.ToList();

            return lst;
        }

        public async Task<List<Threshold>> GetTaxThresholdListAsync()
        {
            if (_thresholdsDic.Count == 0)
            {
                await RetrieveSARSDataAsync();
            }

            if (_thresholdsDic.Count == 0) return new();

            List<Threshold> lst = _thresholdsDic.Values.ToList();

            return lst;
        }
    }
}
