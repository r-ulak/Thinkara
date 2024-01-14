using ContactsManager;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Category
{
    [TestFixture]
    [Category("ContactExtractor")]
    public class ContactExtractorUnitTest
    {
        [Test]
        [TestCase("planetgeni", "Testing_24!")]
        public void GetGmailContact(string username, string password)
        {
            //string[] emails = 
            //    GmailExtractor.ExtractEmail(username, password);
        }
    }
}
