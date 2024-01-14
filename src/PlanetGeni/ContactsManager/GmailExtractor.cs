using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.GData.Contacts;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.Contacts;
using System.Configuration;
using DAO.Models;

namespace ContactsManager
{
    public static class GmailExtractor
    {

        public static List<WebUserContact> ExtractEmail(string authorizationToken)
        {
            try
            {

                RequestSettings rs = new RequestSettings(ConfigurationManager.AppSettings["AppName"], authorizationToken);
                // AutoPaging results in automatic paging in order to retrieve all contacts
                rs.AutoPaging = true;
                rs.PageSize = 5000;
                ContactsRequest cr = new ContactsRequest(rs);
                Feed<Contact> f = cr.GetContacts();
                List<WebUserContact> userContacts = new List<WebUserContact>();

                foreach (Contact entry in f.Entries)
                {
                    WebUserContact contact = new WebUserContact();
                    if (entry.PrimaryEmail != null)
                    {
                        if (!string.IsNullOrEmpty(entry.PrimaryEmail.Address))
                        {
                            contact.FriendEmailId = entry.PrimaryEmail.Address.ToLower();

                        }
                        if (entry.Name != null)
                        {
                            Name name = entry.Name;
                            if (!string.IsNullOrEmpty(name.GivenName))
                            {
                                string givenNameToDisplay = name.GivenName;
                                if (!string.IsNullOrEmpty(name.GivenNamePhonetics))
                                    givenNameToDisplay += " (" + name.GivenNamePhonetics + ")";
                                contact.NameFirst = givenNameToDisplay;
                            }
                            if (!string.IsNullOrEmpty(name.FamilyName))
                            {
                                string familyNameToDisplay = name.FamilyName;
                                if (!string.IsNullOrEmpty(name.FamilyNamePhonetics))
                                    familyNameToDisplay += " (" + name.FamilyNamePhonetics + ")";
                                contact.NameLast = familyNameToDisplay;

                            }

                        }
                        contact.Unsubscribe = false;
                        userContacts.Add(contact);
                    }
                }
                return userContacts;

                //return f.Entries.SelectMany(x => x.Emails.Select(y => y.Address)).ToArray();

            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to ImportGmail Contacts");
                return null;
            }
        }
    }
}
