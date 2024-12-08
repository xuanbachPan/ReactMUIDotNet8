using Microsoft.AspNetCore.Mvc;
using ReactMUIWebAPIApplication.Models;
using ReactMUIWebAPIApplication.Services;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ReactMUIWebAPIApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactMuiAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ReactMuiAPIController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("[action]")]
        public JsonResult FindTheMinFuel(string rows, string cols, string pVal, string data)
        {
            int stsCode = 0;
            string stsMessage = "OK";
            int resultFlg = 0;
            FindTheUsedFuel findTheMinFuel = new FindTheUsedFuel();
            ReactMuiAPIService reactMuiAPIService = new ReactMuiAPIService();
            BizResult result = new BizResult(stsCode, stsMessage, String.Empty);
            JsonResult jsonResult = new JsonResult(result);

            try
            {
                //Console.WriteLine(data);
                if (String.IsNullOrEmpty(rows) || String.IsNullOrEmpty(cols) || String.IsNullOrEmpty(pVal)
                    || String.IsNullOrEmpty(data))
                {
                    stsCode = -1;
                    stsMessage = "rows or cols or p or dataCells is null.";
                    BizResult resultError = new BizResult(stsCode, stsMessage, String.Empty);
                    return new JsonResult(resultError);
                }

                int m = Int32.Parse(rows);
                int n = Int32.Parse(cols);
                int p = Int32.Parse(pVal);
                int q = p - 1;
                string[] dataArray = data.Split(",");
                int count = 0;
                List<Island> AllIslandPosList = new List<Island>();

                if (m * n != dataArray.Length)
                {
                    stsCode = -2;
                    stsMessage = "Numbers of all dataCells does not match value of rows multiply by columns.";
                    BizResult resultError = new BizResult(stsCode, stsMessage, String.Empty);
                    return new JsonResult(resultError);
                }

                if (m * n < p)
                {
                    stsCode = -3;
                    stsMessage = "pVal is bigger than value of rows multiply by columns.";
                    BizResult resultError = new BizResult(stsCode, stsMessage, String.Empty);
                    return new JsonResult(resultError);
                }
                var numQueryP = from numP in dataArray where numP == pVal select numP;
                if (numQueryP.Count() <= 0)
                {
                    stsCode = -4;
                    stsMessage = "There's no dataCell has the key value equals to pVal(" + pVal + ").";
                    BizResult resultError = new BizResult(stsCode, stsMessage, String.Empty);
                    return new JsonResult(resultError);
                }
                else if (numQueryP.Count() > 1)
                {
                    stsCode = -4;
                    stsMessage = "There's only one dataCell has the key value equals to pVal(" + pVal + ").The dataCell List has too many dataCells have the key value equals to pVal(" + pVal + ")";
                    BizResult resultError = new BizResult(stsCode, stsMessage, String.Empty);
                    return new JsonResult(resultError);
                }

                var numQueryBiggerP = from num in dataArray where Int32.Parse(num) > p select num;
                if (numQueryBiggerP.Count() > 0)
                {
                    stsCode = -4;
                    stsMessage = "Values of all dataCells must not be bigger than pVal(" + pVal + ").";
                    BizResult resultError = new BizResult(stsCode, stsMessage, String.Empty);
                    return new JsonResult(resultError);
                }

                while (q > 0)
                {
                    var numQuerySmallerP = from num in dataArray where num == q.ToString() select num;
                    if (numQuerySmallerP.Count() <= 0)
                    {
                        stsCode = -4;
                        stsMessage = "There's not any dataCell has the key value equal to " + q + ".";
                        BizResult resultError = new BizResult(stsCode, stsMessage, String.Empty);
                        return new JsonResult(resultError);
                    }
                    q--;
                }

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Island islandPos = new Island(i + 1, j + 1, Int32.Parse(dataArray[count]));
                        AllIslandPosList.Add(islandPos);
                        count++;
                    }
                }

                reactMuiAPIService.FindTheMinValueofFuel(out findTheMinFuel, AllIslandPosList, p, out resultFlg);

                if (resultFlg != 1)
                {
                    stsCode = -5;
                    stsMessage = "resultFlg != 1";
                    BizResult resultError = new BizResult(stsCode, stsMessage, String.Empty);
                    return new JsonResult(resultError);
                }

                string strFindTheMinFuel = JsonConvert.SerializeObject(findTheMinFuel);
                reactMuiAPIService.SaveData(m, n, p, data, findTheMinFuel, out resultFlg);

                if (resultFlg == 2)
                {
                    stsMessage = "Duplicate"; //Input data existed in database. Dont save.
                }

                if (resultFlg == -1)
                {
                    stsCode = -7;
                    stsMessage = "There is a problem. Save input data unsuccesful.";
                    BizResult resultError = new BizResult(stsCode, stsMessage, String.Empty);
                    return new JsonResult(resultError);
                }

                result = new BizResult(stsCode, stsMessage, strFindTheMinFuel);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                stsCode = -6;
                stsMessage = ex.Message.ToString();
                BizResult resultError = new BizResult(stsCode, stsMessage, String.Empty);
                Console.WriteLine(ex.ToString());
                return new JsonResult(resultError);
            }
            finally
            {
            }
        }
    }
}
