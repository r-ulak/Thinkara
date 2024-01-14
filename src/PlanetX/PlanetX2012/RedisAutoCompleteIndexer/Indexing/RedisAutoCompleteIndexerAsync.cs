using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanetX2012.Models.DAO;

namespace RedisAutoCompleteIndexer
{
    public class RedisAutoCompleteIndexerAsync
    {
        private int redisautoCompletedatabase =
            Convert.ToInt32(ConfigurationManager.AppSettings["redis.autoComplete.database"]);
        private string redisautoCompleteIndex =
            ConfigurationManager.AppSettings["redis.autoComplete.Index"];

        private string redisautoCompleteIndexType;
        private string redisautoInsertProcedureName;
        private string redisautoUpdateProcedureName;
        private string redisautoDeleteProcedureName;
        private string redisautoCompleteIndexKey;
        private string redisautoCompletePrefixKey;

        private int taskCountInsert = Convert.ToInt32(ConfigurationManager.AppSettings["redis.autoComplete.TaskCount.Insert"]);
        private int taskCountUpdate = Convert.ToInt32(ConfigurationManager.AppSettings["redis.autoComplete.TaskCount.Update"]);
        private int taskCountDelete = Convert.ToInt32(ConfigurationManager.AppSettings["redis.autoComplete.TaskCount.Delete"]);

        public RedisAutoCompleteIndexerAsync()
        {


            redisautoCompleteIndexType = ConfigurationManager.
                                            AppSettings["redis.autoComplete." + redisautoCompleteIndex + ".IndexType"];
            redisautoCompleteIndexKey = ConfigurationManager.
                                            AppSettings["redis.autoComplete." + redisautoCompleteIndex + ".Key"];
            redisautoInsertProcedureName = ConfigurationManager.
                                            AppSettings["redis.autoComplete." + redisautoCompleteIndex + ".ProcedureName.Insert." + redisautoCompleteIndexType];
            redisautoUpdateProcedureName = ConfigurationManager.
                                            AppSettings["redis.autoComplete." + redisautoCompleteIndex + ".ProcedureName.Update." + redisautoCompleteIndexType];
            redisautoDeleteProcedureName = ConfigurationManager.
                                            AppSettings["redis.autoComplete." + redisautoCompleteIndex + ".ProcedureName.Delete." + redisautoCompleteIndexType];

            redisautoCompletePrefixKey =
                ConfigurationManager.AppSettings["redis.autoComplete." + redisautoCompleteIndex + ".PrefixKey"];


        }

        public void StartTasks()
        {

            int startIndex = Convert.ToInt32(ConfigurationManager.
                AppSettings["redis.autoComplete." + redisautoCompleteIndex + ".StartIndex"]);
            var updateTasks = new Task[taskCountInsert];
            var insertTasks = new Task[taskCountInsert];
            var deleteTasks = new Task[taskCountDelete];

            int insertTotal = GetInsertTotal();
            if (insertTotal > 0)
            {
                int endRange = 0;
                int startRange = 0;

                for (int i = 0; i < taskCountInsert; i++)
                {
                    GetRanges(ref startRange, ref endRange, insertTotal, i, startIndex, taskCountInsert);
                    int tempStartRange = startRange;
                    int tempEndRange = endRange;
                    Console.WriteLine("Starting Insert Indexing task startRange {0} EndRange {1}", tempStartRange, tempEndRange);
                    insertTasks[i] = Task.Factory.StartNew(() => StartInsertIndexing(tempStartRange, tempEndRange));

                }

            }


            int updateTotal = GeUpdateTotal();

            if (updateTotal > 0)
            {
                int endRange = 0;
                int startRange = 0;

                for (int i = 0; i < taskCountUpdate; i++)
                {
                    GetRanges(ref startRange, ref endRange, updateTotal, i, startIndex, taskCountUpdate);
                    int tempStartRange = startRange;
                    int tempEndRange = endRange;
                    Console.WriteLine("Starting Update Indexing task startRange {0} EndRange {1}", tempStartRange, tempEndRange);
                    updateTasks[i] = Task.Factory.StartNew(() => StartUpdateIndexing(tempStartRange, tempEndRange));

                }

            }


            int deleteTotal = GetDeleteTotal();

            if (deleteTotal > 0)
            {
                int endRange = 0;
                int startRange = 0;

                for (int i = 0; i < taskCountDelete; i++)
                {
                    GetRanges(ref startRange, ref endRange, deleteTotal, i, startIndex, taskCountDelete);
                    int tempStartRange = startRange;
                    int tempEndRange = endRange;
                    Console.WriteLine("Starting Delete Indexing task startRange {0} EndRange {1}", tempStartRange, tempEndRange);
                    deleteTasks[i] = Task.Factory.StartNew(() => StartDeleteIndexing(tempStartRange, tempEndRange));

                }

            }
            if (insertTotal > 0)
                try
                {
                    Task.WaitAll(insertTasks);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            if (updateTotal > 0)
                try
                {
                    Task.WaitAll(updateTasks);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            if (deleteTotal > 0)
                try
                {
                    Task.WaitAll(deleteTasks);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            Console.WriteLine("Completed Indexing");
        }

        private void StartInsertIndexing(int startRange, int endRange)
        {

            AutoCompleteIndexer redisListIndexer;
            if (redisautoCompleteIndex == "CityList")
                redisListIndexer = new IndexCityListController(
                    redisautoCompletedatabase, redisautoCompleteIndexKey, redisautoCompletePrefixKey
                    ) { startRange = startRange, endRange = endRange };
            else if (redisautoCompleteIndex == "WebUserList")
                redisListIndexer = new IndexWebUserController(
                  redisautoCompletedatabase, redisautoCompleteIndexKey, redisautoCompletePrefixKey
                  ) { startRange = startRange, endRange = endRange };
            else
                redisListIndexer = new IndexCityListController(
                    redisautoCompletedatabase, redisautoCompleteIndexKey, redisautoCompletePrefixKey
                    ) { startRange = startRange, endRange = endRange };


            redisListIndexer.LoadDataListInsert(redisautoInsertProcedureName);
            var taskHashIndex = Task.Factory.StartNew(redisListIndexer.IndexHashKey);
            var taskIndexPrefixKeyInsert = Task.Factory.StartNew(redisListIndexer.IndexPrefixKeyInsert);

            taskHashIndex.Wait();
            taskIndexPrefixKeyInsert.Wait();



        }

        private void StartUpdateIndexing(int startRange, int endRange)
        {

            AutoCompleteIndexer redisListIndexer;
            if (redisautoCompleteIndex == "CityList")
                redisListIndexer = new IndexCityListController(
                    redisautoCompletedatabase, redisautoCompleteIndexKey, redisautoCompletePrefixKey
                    ) { startRange = startRange, endRange = endRange };
            else if (redisautoCompleteIndex == "WebUserList")
                redisListIndexer = new IndexWebUserController(
                  redisautoCompletedatabase, redisautoCompleteIndexKey, redisautoCompletePrefixKey
                  ) { startRange = startRange, endRange = endRange };
            else
                redisListIndexer = new IndexCityListController(
                    redisautoCompletedatabase, redisautoCompleteIndexKey, redisautoCompletePrefixKey
                    ) { startRange = startRange, endRange = endRange };


            redisListIndexer.LoadDataListUpdate(redisautoUpdateProcedureName);

            var taskRemoveIndex = Task.Factory.StartNew(redisListIndexer.RemoveIndex);
            var taskIndexPrefixKeyUpdate = Task.Factory.StartNew(redisListIndexer.IndexPrefixKeyUpdate);

            taskRemoveIndex.Wait();
            taskIndexPrefixKeyUpdate.Wait();



        }

        private void StartDeleteIndexing(int startRange, int endRange)
        {

            AutoCompleteIndexer redisListIndexer;
            if (redisautoCompleteIndex == "CityList")
                redisListIndexer = new IndexCityListController(
                    redisautoCompletedatabase, redisautoCompleteIndexKey, redisautoCompletePrefixKey
                    ) { startRange = startRange, endRange = endRange };
            else if (redisautoCompleteIndex == "WebUserList")
                redisListIndexer = new IndexWebUserController(
                  redisautoCompletedatabase, redisautoCompleteIndexKey, redisautoCompletePrefixKey
                  ) { startRange = startRange, endRange = endRange };
            else
                return;


            redisListIndexer.LoadDataListDelete(redisautoDeleteProcedureName);
            var taskRemoveIndex = Task.Factory.StartNew(redisListIndexer.RemoveIndex);
            var taskIndexRemovePrefixIndex = Task.Factory.StartNew(redisListIndexer.RemovePrefixIndex);

            taskRemoveIndex.Wait();
            taskIndexRemovePrefixIndex.Wait();



        }

        private int GetInsertTotal()
        {

            StoredProcedure sp = new StoredProcedure();
            object count;
            string redisautoInsertTotalProcedureName = ConfigurationManager.
                                        AppSettings["redis.autoComplete." + redisautoCompleteIndex + ".ProcedureName.Total.Insert." + redisautoCompleteIndexType];
            if (redisautoInsertTotalProcedureName.Length == 0)
                return 0;
            else
                count = sp.GetSqlDataSignleValue(redisautoInsertTotalProcedureName, new Dictionary<string, object>(), "count(*)");
            return Convert.ToInt32(count);
        }

        private int GeUpdateTotal()
        {
            StoredProcedure sp = new StoredProcedure();
            string redisautoUpdateTotalProcedureName = ConfigurationManager.
                                        AppSettings["redis.autoComplete." + redisautoCompleteIndex + ".ProcedureName.Total.Update." + redisautoCompleteIndexType];
            if (redisautoUpdateTotalProcedureName.Length == 0)
                return 0;
            else
                return (int)sp.GetSqlDataSignleValue(redisautoUpdateTotalProcedureName, new Dictionary<string, object>(), "count(*)");
        }

        private int GetDeleteTotal()
        {
            StoredProcedure sp = new StoredProcedure();
            string redisautoDeleteTotalProcedureName = ConfigurationManager.
                                        AppSettings["redis.autoComplete." + redisautoCompleteIndex + ".ProcedureName.Total.Delete." + redisautoCompleteIndexType];
            if (redisautoDeleteTotalProcedureName.Length == 0)
                return 0;
            else
                return (int)sp.GetSqlDataSignleValue(redisautoDeleteTotalProcedureName, new Dictionary<string, object>(), "count(*)");
        }

        private void GetRanges(ref int startRange, ref int endRange, int total, int threadIndex, int startIndex, int taskCount)
        {
            int averagetask = (total - startIndex) / taskCount;
            startRange = (averagetask * threadIndex) + startIndex;

            if (threadIndex == taskCount - 1)
                endRange = total;
            else
                endRange = averagetask * (threadIndex + 1) + startIndex;

        }
    }
}
