using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisAutoCompleteIndexer
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisAutoCompleteIndexerAsync indexCityList = new RedisAutoCompleteIndexerAsync();
            indexCityList.StartTasks();

        }
    }
}
