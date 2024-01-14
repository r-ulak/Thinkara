using Common;
using DAO;
using DataCache;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Jobs
{
    public class AutoCompleteIndexManager<T> where T : new()
    {
        private string HashKey { get; set; }
        private string SetKey { get; set; }
        private string GetProcedureName { get; set; }
        private string HashKeyFieldName { get; set; }
        private Dictionary<string, object> DictionaryParm { get; set; }
        private StoredProcedure spContext = new StoredProcedure();
        private IRedisCacheProvider cache { get; set; }
         List<T> IndexItems { get; set; }
        private PropertyInfo Idfield { get; set; }
        private PropertyInfo[] HashExceptionProperty { get; set; }
        private string[] SetExceptionProperty { get; set; }

        public AutoCompleteIndexManager(string hashkey, string setkey, string procedureName,
            Dictionary<string, object> dictionaryParm, int databaseId, string hasKeyFieldName, PropertyInfo[] hashExceptionProperty, string[] setExceptionProperty)
        {
            HashKey = hashkey;
            SetKey = setkey;
            GetProcedureName = procedureName;
            DictionaryParm = dictionaryParm;
            HashKeyFieldName = hasKeyFieldName;
            cache = new RedisCacheProvider(databaseId);
            IndexItems = new List<T>();
            Idfield = typeof(T).GetProperty(HashKeyFieldName);
            HashExceptionProperty = hashExceptionProperty;
            SetExceptionProperty = setExceptionProperty;

        }

        public void IndexAll()
        {
            cache.Invalidate(new string[] { HashKey, SetKey });
            LoadData();
            AddIndexList();
        }
        public void LoadData()
        {
            IndexItems = spContext.GetSqlData<T>(GetProcedureName, DictionaryParm).ToList<T>();
        }
        private void AddtoHash()
        {
            Dictionary<string, string> hashValues = new Dictionary<string, string>();

            foreach (var item in IndexItems)
            {

                foreach (var removeitem in HashExceptionProperty)
                {
                    removeitem.SetValue(item, "");
                }
                hashValues.Add(Idfield.GetValue(item).ToString(), JsonConvert.SerializeObject(item,
                                new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore }
                                ));

            }
            cache.SetHashDictionary(HashKey, hashValues);
        }

        public void AddIndexList()
        {
            Dictionary<string, double> sortValues = new Dictionary<string, double>();
            foreach (var item in IndexItems)
            {
                string keyvalue = Idfield.GetValue(item).ToString();

                foreach (var f in typeof(T).GetProperties().Where(f => !(SetExceptionProperty.Contains(f.Name))))
                {
                    try
                    {
                        sortValues.Add(f.GetValue(item).ToString().ToLower() + ":" + keyvalue, 0);
                    }
                    catch (ArgumentException e)
                    {

                    }

                }
            }
            cache.AddSoretedSets(SetKey, sortValues);
            AddtoHash();
            IndexItems.Clear();

        }
        public void AddIndexItem(T indexItem)
        {
            IndexItems.Add(indexItem);
            AddIndexList();
            AddtoHash();
            IndexItems.Clear();

        }


        public void UpdateIndexItem(T oldindexItem, T newindexItem)
        {
            RemoveIndexItem(oldindexItem);
            AddIndexItem(newindexItem);
        }

        public void RemoveIndexItem(T indexItem)
        {
            List<string> sortValues = new List<String>();
            string keyvalue = Idfield.GetValue(indexItem).ToString();
            cache.HashRemove(HashKey, keyvalue);

            foreach (var f in typeof(T).GetProperties().Where(f => !(SetExceptionProperty.Contains(f.Name))))
            {
                sortValues.Add(f.GetValue(indexItem).ToString() + ":" + keyvalue);
            }
            cache.RemoveSortedSetMembers(SetKey, sortValues.ToArray());
        }

    }
}
