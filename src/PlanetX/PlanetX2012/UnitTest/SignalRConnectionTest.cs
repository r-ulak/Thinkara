namespace UnitTest
{
    using System;
    using System.Configuration;
    using BookSleeve;
    using DAO.DAO.Repository;
    using NUnit.Framework;
    using PlanetX2012.UserStatusManager;
    [TestFixture]
    public class SignalRConnectionTest
    {
        [Test]
        [Repeat(10)]
        public void AddConnectionRepeat()
        {
            ResetDb();
            SignalRConnectionRepository connectionSignalR = new SignalRConnectionRepository();
            string GuidConnection1 = Guid.NewGuid().ToString();
            string GuidConnection2 = Guid.NewGuid().ToString();

            connectionSignalR.AddSignalRConnection("1", GuidConnection1);
            connectionSignalR.AddSignalRConnection("1", GuidConnection2);

            string[] userConnection = connectionSignalR.GetSignalRConnection("1");

            CollectionAssert.Contains(userConnection, GuidConnection1);
            CollectionAssert.Contains(userConnection, GuidConnection2);

        }

        [Test]
        [Repeat(10)]
        public void GetFriendsConnection()
        {
            ResetDb();

            SignalRConnectionRepository connectionSignalR = new SignalRConnectionRepository();
            string GuidConnection1_1 = Guid.NewGuid().ToString();
            string GuidConnection1_2 = Guid.NewGuid().ToString();
            string GuidConnection2_1 = Guid.NewGuid().ToString();
            string GuidConnection2_2 = Guid.NewGuid().ToString();
            string GuidConnection3 = Guid.NewGuid().ToString();
            string GuidConnection4 = Guid.NewGuid().ToString();
            string GuidConnection5 = Guid.NewGuid().ToString();


            connectionSignalR.AddSignalRConnection("1", GuidConnection1_1);
            connectionSignalR.AddSignalRConnection("1", GuidConnection1_2);

            connectionSignalR.AddSignalRConnection("2", GuidConnection2_1);
            connectionSignalR.AddSignalRConnection("2", GuidConnection2_2);

            connectionSignalR.AddSignalRConnection("3", GuidConnection3);
            connectionSignalR.AddSignalRConnection("4", GuidConnection4);
            connectionSignalR.AddSignalRConnection("5", GuidConnection5);


            //Friends of userid 1
            string[] actualuser1FirendConnection = connectionSignalR.GetAllFriendsSignalRConnection("1");

            string[] expecteduser1FirendConnection =
                new string[] { GuidConnection2_1, GuidConnection2_2,
                    GuidConnection3, GuidConnection4 ,GuidConnection5};

            //Friends of userid 2
            string[] actualuser2FirendConnection = connectionSignalR.GetAllFriendsSignalRConnection("2");

            string[] expecteduser2FirendConnection =
                new string[] { GuidConnection1_1, GuidConnection1_2,
                    GuidConnection3, GuidConnection4 ,GuidConnection5};

            //Friends of userid 3
            string[] actualuser3FirendConnection = connectionSignalR.GetAllFriendsSignalRConnection("3");

            string[] expecteduser3FirendConnection =
                new string[] { GuidConnection1_1, GuidConnection1_2,
                    GuidConnection2_1, GuidConnection2_2};

            //Friends of userid 4
            string[] actualuser4FirendConnection = connectionSignalR.GetAllFriendsSignalRConnection("4");

            string[] expecteduser4FirendConnection =
                new string[] { GuidConnection1_1, GuidConnection1_2,
                    };


            //Friends of userid 5
            string[] actualuser5FirendConnection = connectionSignalR.GetAllFriendsSignalRConnection("5");
            string[] expecteduser5FirendConnection =
                new string[] { GuidConnection1_1, GuidConnection1_2,
                    };


            CollectionAssert.AreEquivalent(expecteduser1FirendConnection, actualuser1FirendConnection);
            CollectionAssert.AreEquivalent(expecteduser2FirendConnection, actualuser2FirendConnection);
            CollectionAssert.AreEquivalent(expecteduser3FirendConnection, actualuser3FirendConnection);
            CollectionAssert.AreEquivalent(expecteduser4FirendConnection, actualuser4FirendConnection);
            CollectionAssert.AreEquivalent(expecteduser5FirendConnection, actualuser5FirendConnection);


        }

        [Test]
        [Repeat(1)]
        public void DeleteConnection()
        {
            ResetDb();

            SignalRConnectionRepository connectionSignalR = new SignalRConnectionRepository();
            string GuidConnection1_1 = Guid.NewGuid().ToString();
            string GuidConnection1_2 = Guid.NewGuid().ToString();
            string GuidConnection2_1 = Guid.NewGuid().ToString();
            string GuidConnection2_2 = Guid.NewGuid().ToString();
            string GuidConnection3 = Guid.NewGuid().ToString();
            string GuidConnection4 = Guid.NewGuid().ToString();
            string GuidConnection5 = Guid.NewGuid().ToString();


            connectionSignalR.AddSignalRConnection("1", GuidConnection1_1);
            connectionSignalR.AddSignalRConnection("1", GuidConnection1_2);

            connectionSignalR.AddSignalRConnection("2", GuidConnection2_1);
            connectionSignalR.AddSignalRConnection("2", GuidConnection2_2);

            connectionSignalR.AddSignalRConnection("3", GuidConnection3);
            connectionSignalR.AddSignalRConnection("4", GuidConnection4);
            connectionSignalR.AddSignalRConnection("5", GuidConnection5);

            connectionSignalR.DeleteConnection("1", GuidConnection1_2);
            connectionSignalR.DeleteConnection("5", GuidConnection5);
            connectionSignalR.DeleteConnection("2", GuidConnection5);
            connectionSignalR.DeleteConnection("2", GuidConnection2_2);
            connectionSignalR.DeleteConnection("5", GuidConnection5);

            //Connection of userid 1
            string[] actualuser1Connection = connectionSignalR.GetSignalRConnection("1");

            string[] expecteduser1Connection =
                new string[] { GuidConnection1_1 };

            //Connection of userid 2
            string[] actualuser2Connection = connectionSignalR.GetSignalRConnection("2");

            string[] expecteduser2Connection =
                new string[] { GuidConnection2_1 };

            //Connection of userid 3
            string[] actualuser3Connection = connectionSignalR.GetSignalRConnection("3");

            string[] expecteduser3Connection =
                new string[] { GuidConnection3 };

            //Connection of userid 4
            string[] actualuser4Connection = connectionSignalR.GetSignalRConnection("4");

            string[] expecteduser4Connection =
                new string[] { GuidConnection4
                    };


            //Connection of userid 5
            string[] actualuser5Connection = connectionSignalR.GetSignalRConnection("5");
            string[] expecteduser5Connection =
                new string[] { };


            CollectionAssert.AreEquivalent(expecteduser1Connection, actualuser1Connection);
            CollectionAssert.AreEquivalent(expecteduser2Connection, actualuser2Connection);
            CollectionAssert.AreEquivalent(expecteduser3Connection, actualuser3Connection);
            CollectionAssert.AreEquivalent(expecteduser4Connection, actualuser4Connection);
            CollectionAssert.AreEquivalent(expecteduser5Connection, actualuser5Connection);


        }

        private void ResetDb()
        {
            string server = ConfigurationManager.AppSettings["redis.server"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["redis.port"]);
            string password = ConfigurationManager.AppSettings["redis.password"];
            using (var conn = new RedisConnection(server, port, -1, password, allowAdmin: true))
            {
                conn.Open();
                var result = conn.Server.FlushDb(0);
                result.Wait();
            }
        }

    }
}
