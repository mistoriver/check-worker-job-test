using CheckWorker.Helpers;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CheckWorker
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class DbProxy : IDbProxy
    {
        readonly static string connString = ((NameValueCollection)ConfigurationManager.GetSection("ApplicationSettings"))["ConnectionString"];
        public async void TakeChequeAsync(Cheque cheque)
        {
            using (IDbConnection conn = new SqlConnection(connString))
            {
                var procParams = new DynamicParameters();
                procParams.Add("cheque_id", cheque.ChequeId, DbType.Guid);
                procParams.Add("cheque_number", cheque.ChequeNumber, DbType.String);
                procParams.Add("summ", cheque.Summ, DbType.Decimal);
                procParams.Add("discount", cheque.Discount, DbType.Decimal);
                procParams.Add("articles", cheque.Articles.Aggregate((x, y) => x + ";" + y), DbType.String);
                await conn.ExecuteAsync("[custom].[save_cheques]", procParams, commandType:CommandType.StoredProcedure);
            }
        }

        public async Task<List<Cheque>> GetChequesAsync(int howMany)
        {
            List<Cheque> resultList = null;
            using (IDbConnection conn = new SqlConnection(connString))
            {
                var procParams = new DynamicParameters();
                procParams.Add("pack_size", howMany);
                var dbData = await conn.QueryAsync("[custom].[get_cheques_pack]", procParams, commandType: CommandType.StoredProcedure);
                dbData.ToList().ForEach(item => 
                    resultList.Add(new Cheque 
                    {
                            ChequeId = item.cheque_id,
                            ChequeNumber = item.cheque_number,
                            Summ = item.summ,
                            Discount = item.discount,
                            Articles = ((string)item.articles).Split(';')
                    }
                ));
            }
            return resultList;
        }
    }
}
