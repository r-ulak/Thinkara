using System;
using System.Configuration;
using BookSleeve;
using DAO.DAO.Repository;
using DAO.DAO.Repository.Redis;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class RedisAutoCompleteIndexerTest
    {
        [Test]
        public void GetAutoCompleteList()
        {
            RediAutoCompleteRepository repo = new RediAutoCompleteRepository(1);

            foreach (string item in repo.GetAutoCompleteList("Oklahoma","PC", "C"))
            {
                Console.WriteLine(item);
            }

            Assert.Pass();
        }
    }
}
