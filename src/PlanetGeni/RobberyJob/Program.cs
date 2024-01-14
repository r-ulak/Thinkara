using Common;
using DAO.Models;
using DTO.Custom;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobberyJob
{
    class Program
    {
        private static ICountryCodeRepository countryRepo;
        private static ICountryBudgetDetailsDTORepository countrybudgetrepo;
        private static ICountryCodeRepository countryPopulation;
        private static IWebUserDTORepository webUsers;
        private static IUserBankAccountDTORepository userBankAccount;
        private static IPostCommentDTORepository postRepo;

        static void Main(string[] args)
        {
            countryRepo = new CountryCodeRepository();
            countrybudgetrepo = new CountryBudgetDetailsDTORepository();
            webUsers = new WebUserDTORepository();
            userBankAccount = new UserBankAccountDTORepository();
            postRepo = new PostCommentDTORepository();
            StartRobbery();
        }
        static List<CountryCode> GetCountryList()
        {
            string countries = countryRepo.GetCountryCodes();
            return JsonConvert.DeserializeObject<List<CountryCode>>(countries);
        }

        static void StartRobbery()
        {
            List<CountryCode> countries = GetCountryList();
            countryPopulation = new CountryCodeRepository();
            decimal totalLoss = 0;
            int victims = 0;
            foreach (var item in countries)
            {
                decimal budgetAmount = countrybudgetrepo.GetCountryBudgetByType(item.CountryId, 2).Amount;
                int countryBudget = Convert.ToInt32(budgetAmount);
                int currentPopulation = countryPopulation.GetCountryPopulation(item.CountryId.ToString());
                totalLoss = 0;
                victims = 0;
                if (currentPopulation != 0)
                {
                    if (countryBudget / currentPopulation < 50)
                    {
                        //Get 10 random webusers from the same country.
                        IEnumerable<WebUser> randomUsers = webUsers.GetRandomWebUsers(10, item.CountryId.ToString());
                        CountryCode targetCountry = JsonConvert.DeserializeObject<CountryCode>(countryRepo.GetCountryCodeJson(item.CountryId));
                        StringBuilder postParms = new StringBuilder();
                        StringBuilder postUsers = new StringBuilder();
                        Console.WriteLine("\n\n Currently Processing {1} {0}", item.Code, item.CountryId);
                        foreach (var user in randomUsers)
                        {

                            decimal balanceAmount = 0;
                            decimal deductAmount = 0;
                            decimal randomPercent = 0;
                            Random rand;


                            UserBankAccount bankAccount = userBankAccount.GetUserBankDetails(user.UserId);
                            balanceAmount = bankAccount.Cash;
                            if (balanceAmount > 1000)
                            {
                                rand = new Random(user.UserId);
                                randomPercent = rand.Next(1, 50 - (countryBudget / currentPopulation)) / 10;
                                deductAmount = (balanceAmount * randomPercent) / 100;
                                if (userBankAccount.UpdateBankAc(-deductAmount, user.UserId) == true)
                                {
                                    victims++;
                                    totalLoss += deductAmount;
                                    Console.WriteLine("     Dedudcted {0} From {1} Total Balance Before: {2} Total Balance After {3} Deduction % {4}"
                                , deductAmount, user.UserId, balanceAmount, balanceAmount - deductAmount, randomPercent);
                                }


                                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                                String parmText = "";
                                sbyte priority = 10;
                                short notificationTypeId = AppSettings.SecurityNotification;
                                postUsers.AppendFormat(
                                    "<div class='col-xs-6 padding3centt'><a class='btn-link fontsize90'  onclick='viewUserProfile({0})'><img width='28px' height='28px' src='{1}'> <span class='text-ellipsis '> {2} {3} </span></a> </div>",
                                    user.UserId,
                                   AppSettings.AzureProfilePicUrl + user.Picture,
                                    user.NameFirst, user.NameLast
                                    );

                                parmText = string.Format("{0}|{1}|{2}",
                                targetCountry.Code, targetCountry.CountryId,
                                deductAmount);

                                userNotif.AddNotification(false, string.Empty,
                                notificationTypeId, parmText.ToString(), priority, user.UserId);
                            }
                        }
                        postParms.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                               targetCountry.Code.Trim(), targetCountry.CountryId, victims, Math.Round(totalLoss, 2), postUsers.ToString());
                        AddPost(postParms.ToString(), targetCountry.CountryId);
                    }
                    else
                    {
                        Console.WriteLine("\n\n ********* Skipped Processing {1} {0} *********", item.Code, item.CountryId);

                    }
                }
            }
        }

        static void AddPost(string messgae, string countryId)
        {

            Post post = new Post
            {
                Parms = messgae,
                PostContentTypeId = AppSettings.RobberyPostContentTypeId,
                CountryId = countryId


            };
            postRepo.SavePost(post);
        }
    }
}
