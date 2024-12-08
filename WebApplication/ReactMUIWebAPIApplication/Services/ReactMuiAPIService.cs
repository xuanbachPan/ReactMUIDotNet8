using Dapper;
using ReactMUIWebAPIApplication.DBConnection;
using ReactMUIWebAPIApplication.Models;
using System.Text;
using System.Data;
using System.Configuration;
namespace ReactMUIWebAPIApplication.Services
{
    public class ReactMuiAPIService { 
        DBContext dbContext;
        //ConfigurationProvider config;

        public ReactMuiAPIService()
        {
            //config = new ConfigurationProvider();
            dbContext = new DBContext();
        }

        public void FindTheMinValueofFuel(out FindTheUsedFuel findTheMinFuel, List<Island> AllIslandsList, int p, out int resultFlg)
        {
            findTheMinFuel = new FindTheUsedFuel();
            resultFlg = 1;

            try
            {
                if (AllIslandsList == null || AllIslandsList.Count == 0)
                {
                    resultFlg = -1;
                    throw new Exception("AllIslandPosList is empty.");
                }
                if (p < 1)
                {
                    resultFlg = -1;
                    throw new Exception("P value is incorrect.");
                }

                List<FindTheUsedFuel> FindTheUsedFuelList = new List<FindTheUsedFuel>();
                List<FindTheUsedFuel> FindTheTempUsedFuelList = new List<FindTheUsedFuel>();
                FindTheUsedFuel tempfindTheUsedFuel = new FindTheUsedFuel();

                int findKeyNumber = 1;
                double theMinFuel = 0; // ~ Sqrt(2*(500-1)^2) ~ 706
                double theUsedFuel = 0;

                FindTheUsedFuel findTheUsedFuel = new FindTheUsedFuel();
                findTheUsedFuel.FuelVal = 0;
                findTheUsedFuel.IslandsList.Add(AllIslandsList.FirstOrDefault<Island>());
                FindTheTempUsedFuelList.Add(findTheUsedFuel);

                while (findKeyNumber <= p)
                {
                    FindTheUsedFuelList.Clear();
                    foreach (FindTheUsedFuel item in FindTheTempUsedFuelList)
                    {
                        FindTheUsedFuelList.Add(item.CloneObject());
                    }
                    FindTheTempUsedFuelList.Clear();

                    foreach (Island destItem in AllIslandsList)
                    {
                        if (destItem.KeyNumber == findKeyNumber)
                        {
                            theMinFuel = 0;
                            foreach (FindTheUsedFuel item in FindTheUsedFuelList)
                            {
                                Island departItem = item.IslandsList.LastOrDefault<Island>();
                                theUsedFuel = Math.Sqrt(Math.Pow(destItem.X - departItem.X, 2) + Math.Pow(destItem.Y - departItem.Y, 2)) + item.FuelVal;
                                if (theMinFuel == 0 ||theUsedFuel < theMinFuel)
                                {
                                    theMinFuel = theUsedFuel;
                                    tempfindTheUsedFuel = null;
                                    tempfindTheUsedFuel = item.CloneObject();
                                    tempfindTheUsedFuel.FuelVal = theUsedFuel;
                                    tempfindTheUsedFuel.IslandsList.Add(destItem);
                                }
                            }

                            FindTheTempUsedFuelList.Add(tempfindTheUsedFuel);
                        }
                    }
                    findKeyNumber++;
                }

                if (FindTheTempUsedFuelList.Count == 1)
                {
                    findTheMinFuel = FindTheTempUsedFuelList.LastOrDefault<FindTheUsedFuel>();
                }
                else
                {
                    throw new Exception("There's a problems when finding the min fuel.");
                }

                //SaveData(m, n, p, tableData, findTheMinFuel);
                //using (var connection = dbContext.CreateConnection())
                //{
                //    connection.Open();

                //}
            }
            catch (Exception e)
            {
                resultFlg = -1;
                Console.WriteLine(e.Message);
            }
            
        }

        public void SaveData(int m, int n, int p, string tableData, FindTheUsedFuel findTheMinFuel, out int resultFlg)
        {
            resultFlg = 1;
            int index = 0;
            int maxInputId = 0;
            int maxrouteId = 0;
            string stringIslandsList = string.Empty;

            if (findTheMinFuel == null)
                {
                resultFlg = -1;
                throw new Exception("findTheMinFuel is empty.");
            }

            try
            {
                using (var connection = dbContext.CreateConnection())
                {
                    //Check duplicated Data 
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" Select count(*) from dbo.InputData  ");
                    sql.Append("  Where RowNums        =  @rownums   ");
                    sql.Append("    And ColNums        =  @colnums   ");
                    sql.Append("    And TreasureKeyNum =  @keynum    ");
                    sql.Append("    And TableData      =  @tbldata   ");

                    var parameters = new DynamicParameters();
                    parameters.Add("rownums", n, DbType.Int32);
                    parameters.Add("colnums", m, DbType.Int32);
                    parameters.Add("keynum", p, DbType.Int32);
                    parameters.Add("tbldata", tableData, DbType.String);

                    int count = connection.QuerySingle<int>(sql.ToString(), parameters);

                    if (count > 0)
                    {
                        //Dont save the dupplicated data
                        resultFlg = 2;
                        return;
                    }

                    //Select Max InputID from InputData table
                    sql.Length = 0;
                    sql.Append("Select ISNULL(MAX(InputID),0) from dbo.InputData");
                    maxInputId = connection.QuerySingle<int>(sql.ToString());
                    maxInputId++;

                    sql.Length = 0;
                    //Insert data to InputData table
                    sql.Append(" Insert into dbo.InputData                            ");
                    sql.Append("       (RowNums, ColNums, TreasureKeyNum, TableData)  ");
                    sql.Append(" Values                                               ");
                    sql.Append("      (@rownums,                                      ");
                    sql.Append("       @colnums,                                      ");
                    sql.Append("       @keynum,                                       ");
                    sql.Append("       @tbldata)                                      ");
                    connection.Execute(sql.ToString(), parameters);

                    //Select Max RouteId in RouteData table
                    sql.Length = 0;
                    sql.Append("Select ISNULL(MAX(RouteID),0) from dbo.RouteData");
                    maxrouteId = connection.QuerySingle<int>(sql.ToString());
                    maxrouteId++;

                    sql.Length = 0;
                    //Insert data to OutputData table
                    sql.Append(" Insert into dbo.OutputData                                    ");
                    sql.Append("       (InputID, RowNums, ColNums, PVal, MinFuelVal, RouteID) ");
                    sql.Append(" Values                                                       ");
                    sql.Append("      (@inputId,                                               ");
                    sql.Append("       @rownums,                                              ");
                    sql.Append("       @colnums,                                              ");
                    sql.Append("       @keynum,                                               ");
                    sql.Append("       @minFueVal,                                            ");
                    sql.Append("       @routeId)                                               ");
                    parameters = new DynamicParameters();
                    parameters.Add("inputId", maxInputId, DbType.Int32);
                    parameters.Add("rownums", n, DbType.Int32);
                    parameters.Add("colnums", m, DbType.Int32);
                    parameters.Add("keynum", p, DbType.Int32);
                    parameters.Add("minFueVal", findTheMinFuel.FuelVal, DbType.Double);
                    parameters.Add("routeId", maxrouteId, DbType.Int32);
                    connection.Execute(sql.ToString(), parameters);

                    //Insert data into RouteData table
                    foreach (Island item in findTheMinFuel.IslandsList)
                    {
                        index++;
                        sql.Length = 0;
                        sql.Append(" Insert into dbo.RouteData                                    ");
                        sql.Append("       (InputID, RouteID, PositionNo, XVal, YVal, KeyNumber)  ");
                        sql.Append(" Values                                                       ");
                        sql.Append("      (@inputId,                                               ");
                        sql.Append("       @routeId,                                              ");
                        sql.Append("       @positionNo,                                           ");
                        sql.Append("       @xval,                                                 ");
                        sql.Append("       @yval,                                                 ");
                        sql.Append("       @keyNum)                                               ");
                        parameters = new DynamicParameters();
                        parameters.Add("inputId", maxInputId, DbType.Int32);
                        parameters.Add("routeId", maxrouteId, DbType.Int32);
                        parameters.Add("positionNo", index, DbType.Int32);
                        parameters.Add("xval", item.X, DbType.Int32);
                        parameters.Add("yval", item.Y, DbType.Int32);
                        parameters.Add("keyNum", item.KeyNumber, DbType.Int32);
                        connection.Execute(sql.ToString(), parameters);
                    }
                }
            }
            catch (Exception e)
            {
                resultFlg = -1;
                Console.WriteLine(e.Message);
            }
        }

    }
}
