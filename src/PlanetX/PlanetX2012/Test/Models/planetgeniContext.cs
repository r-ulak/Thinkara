using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Test.Models.Mapping;

namespace Test.Models
{
    public partial class planetgeniContext : DbContext
    {
        static planetgeniContext()
        {
            Database.SetInitializer<planetgeniContext>(null);
        }

        public planetgeniContext()
            : base("Name=planetgeniContext")
        {
        }

        public DbSet<AdsFrequencyType> AdsFrequencyTypes { get; set; }
        public DbSet<AdsType> AdsTypes { get; set; }
        public DbSet<AdsTypeList> AdsTypeLists { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<ApplyJob> ApplyJobs { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<BookmarkCategory> BookmarkCategories { get; set; }
        public DbSet<BookmarkInfo> BookmarkInfoes { get; set; }
        public DbSet<BookmarkSubCategory> BookmarkSubCategories { get; set; }
        public DbSet<BudgetCode> BudgetCodes { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessCode> BusinessCodes { get; set; }
        public DbSet<BusinessLocation> BusinessLocations { get; set; }
        public DbSet<BusinessSubCode> BusinessSubCodes { get; set; }
        public DbSet<CandidateAgenda> CandidateAgendas { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<CityCode> CityCodes { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubMember> ClubMembers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CountryBudget> CountryBudgets { get; set; }
        public DbSet<CountryBudgetByType> CountryBudgetByTypes { get; set; }
        public DbSet<CountryCode> CountryCodes { get; set; }
        public DbSet<CountryLeader> CountryLeaders { get; set; }
        public DbSet<CountryWeapon> CountryWeapons { get; set; }
        public DbSet<DegreeCode> DegreeCodes { get; set; }
        public DbSet<DegreeOffered> DegreeOffereds { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<ElectionAgenda> ElectionAgendas { get; set; }
        public DbSet<ElectionCandidate> ElectionCandidates { get; set; }
        public DbSet<ElectionDonation> ElectionDonations { get; set; }
        public DbSet<ElectionPosition> ElectionPositions { get; set; }
        public DbSet<ElectionQueue> ElectionQueues { get; set; }
        public DbSet<Employment> Employments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCode> EventCodes { get; set; }
        public DbSet<EventLocation> EventLocations { get; set; }
        public DbSet<EventMembership> EventMemberships { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<FeedCategory> FeedCategories { get; set; }
        public DbSet<FeedInfo> FeedInfoes { get; set; }
        public DbSet<FeedSubCategory> FeedSubCategories { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<GeneralLog> GeneralLogs { get; set; }
        public DbSet<Goverment> Goverments { get; set; }
        public DbSet<GovermentProvince> GovermentProvinces { get; set; }
        public DbSet<GroupMembership> GroupMemberships { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<ItemCode> ItemCodes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobCode> JobCodes { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Lang> Langs { get; set; }
        public DbSet<LeaderCode> LeaderCodes { get; set; }
        public DbSet<LoanCode> LoanCodes { get; set; }
        public DbSet<LoanFromBusiness> LoanFromBusinesses { get; set; }
        public DbSet<LoanFromPerson> LoanFromPersons { get; set; }
        public DbSet<MajorCode> MajorCodes { get; set; }
        public DbSet<Merchandise> Merchandises { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageInfo> MessageInfoes { get; set; }
        public DbSet<MilitaryForce> MilitaryForces { get; set; }
        public DbSet<Nomination> Nominations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PartyMember> PartyMembers { get; set; }
        public DbSet<PartyMemberType> PartyMemberTypes { get; set; }
        public DbSet<PoliticalParty> PoliticalParties { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostClubACL> PostClubACLs { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<PostUserACL> PostUserACLs { get; set; }
        public DbSet<PostWebContent> PostWebContents { get; set; }
        public DbSet<Privacy> Privacies { get; set; }
        public DbSet<PrivacyType> PrivacyTypes { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProvinceCode> ProvinceCodes { get; set; }
        public DbSet<RequestWarKey> RequestWarKeys { get; set; }
        public DbSet<RsvpStatusCode> RsvpStatusCodes { get; set; }
        public DbSet<SchoolCode> SchoolCodes { get; set; }
        public DbSet<SchoolLocation> SchoolLocations { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<StockCode> StockCodes { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StocksTransaction> StocksTransactions { get; set; }
        public DbSet<TaskReminder> TaskReminders { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<TopicTag> TopicTags { get; set; }
        public DbSet<UserBankAccount> UserBankAccounts { get; set; }
        public DbSet<UserChatroom> UserChatrooms { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserLoan> UserLoans { get; set; }
        public DbSet<UserStock> UserStocks { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<UserVote> UserVotes { get; set; }
        public DbSet<UserVoteChoice> UserVoteChoices { get; set; }
        public DbSet<UserVoteSelectedChoice> UserVoteSelectedChoices { get; set; }
        public DbSet<WeaponType> WeaponTypes { get; set; }
        public DbSet<WebUser> WebUsers { get; set; }
        public DbSet<WebUserLocation> WebUserLocations { get; set; }
        public DbSet<WebUserUpdate> WebUserUpdates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AdsFrequencyTypeMap());
            modelBuilder.Configurations.Add(new AdsTypeMap());
            modelBuilder.Configurations.Add(new AdsTypeListMap());
            modelBuilder.Configurations.Add(new AdvertisementMap());
            modelBuilder.Configurations.Add(new ApplyJobMap());
            modelBuilder.Configurations.Add(new AssetMap());
            modelBuilder.Configurations.Add(new AvatarMap());
            modelBuilder.Configurations.Add(new BlogMap());
            modelBuilder.Configurations.Add(new BookmarkMap());
            modelBuilder.Configurations.Add(new BookmarkCategoryMap());
            modelBuilder.Configurations.Add(new BookmarkInfoMap());
            modelBuilder.Configurations.Add(new BookmarkSubCategoryMap());
            modelBuilder.Configurations.Add(new BudgetCodeMap());
            modelBuilder.Configurations.Add(new BusinessMap());
            modelBuilder.Configurations.Add(new BusinessCodeMap());
            modelBuilder.Configurations.Add(new BusinessLocationMap());
            modelBuilder.Configurations.Add(new BusinessSubCodeMap());
            modelBuilder.Configurations.Add(new CandidateAgendaMap());
            modelBuilder.Configurations.Add(new CardMap());
            modelBuilder.Configurations.Add(new ChatMap());
            modelBuilder.Configurations.Add(new CityCodeMap());
            modelBuilder.Configurations.Add(new ClubMap());
            modelBuilder.Configurations.Add(new ClubMemberMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new CountryBudgetMap());
            modelBuilder.Configurations.Add(new CountryBudgetByTypeMap());
            modelBuilder.Configurations.Add(new CountryCodeMap());
            modelBuilder.Configurations.Add(new CountryLeaderMap());
            modelBuilder.Configurations.Add(new CountryWeaponMap());
            modelBuilder.Configurations.Add(new DegreeCodeMap());
            modelBuilder.Configurations.Add(new DegreeOfferedMap());
            modelBuilder.Configurations.Add(new EducationMap());
            modelBuilder.Configurations.Add(new ElectionMap());
            modelBuilder.Configurations.Add(new ElectionAgendaMap());
            modelBuilder.Configurations.Add(new ElectionCandidateMap());
            modelBuilder.Configurations.Add(new ElectionDonationMap());
            modelBuilder.Configurations.Add(new ElectionPositionMap());
            modelBuilder.Configurations.Add(new ElectionQueueMap());
            modelBuilder.Configurations.Add(new EmploymentMap());
            modelBuilder.Configurations.Add(new EventMap());
            modelBuilder.Configurations.Add(new EventCodeMap());
            modelBuilder.Configurations.Add(new EventLocationMap());
            modelBuilder.Configurations.Add(new EventMembershipMap());
            modelBuilder.Configurations.Add(new FeedMap());
            modelBuilder.Configurations.Add(new FeedCategoryMap());
            modelBuilder.Configurations.Add(new FeedInfoMap());
            modelBuilder.Configurations.Add(new FeedSubCategoryMap());
            modelBuilder.Configurations.Add(new FriendMap());
            modelBuilder.Configurations.Add(new GeneralLogMap());
            modelBuilder.Configurations.Add(new GovermentMap());
            modelBuilder.Configurations.Add(new GovermentProvinceMap());
            modelBuilder.Configurations.Add(new GroupMembershipMap());
            modelBuilder.Configurations.Add(new IndustryMap());
            modelBuilder.Configurations.Add(new ItemCodeMap());
            modelBuilder.Configurations.Add(new JobMap());
            modelBuilder.Configurations.Add(new JobCodeMap());
            modelBuilder.Configurations.Add(new JobTypeMap());
            modelBuilder.Configurations.Add(new LangMap());
            modelBuilder.Configurations.Add(new LeaderCodeMap());
            modelBuilder.Configurations.Add(new LoanCodeMap());
            modelBuilder.Configurations.Add(new LoanFromBusinessMap());
            modelBuilder.Configurations.Add(new LoanFromPersonMap());
            modelBuilder.Configurations.Add(new MajorCodeMap());
            modelBuilder.Configurations.Add(new MerchandiseMap());
            modelBuilder.Configurations.Add(new MessageMap());
            modelBuilder.Configurations.Add(new MessageInfoMap());
            modelBuilder.Configurations.Add(new MilitaryForceMap());
            modelBuilder.Configurations.Add(new NominationMap());
            modelBuilder.Configurations.Add(new NotificationMap());
            modelBuilder.Configurations.Add(new PartyMemberMap());
            modelBuilder.Configurations.Add(new PartyMemberTypeMap());
            modelBuilder.Configurations.Add(new PoliticalPartyMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new PostClubACLMap());
            modelBuilder.Configurations.Add(new PostCommentMap());
            modelBuilder.Configurations.Add(new PostTagMap());
            modelBuilder.Configurations.Add(new PostUserACLMap());
            modelBuilder.Configurations.Add(new PostWebContentMap());
            modelBuilder.Configurations.Add(new PrivacyMap());
            modelBuilder.Configurations.Add(new PrivacyTypeMap());
            modelBuilder.Configurations.Add(new ProfileMap());
            modelBuilder.Configurations.Add(new ProvinceCodeMap());
            modelBuilder.Configurations.Add(new RequestWarKeyMap());
            modelBuilder.Configurations.Add(new RsvpStatusCodeMap());
            modelBuilder.Configurations.Add(new SchoolCodeMap());
            modelBuilder.Configurations.Add(new SchoolLocationMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new StockCodeMap());
            modelBuilder.Configurations.Add(new StockMap());
            modelBuilder.Configurations.Add(new StocksTransactionMap());
            modelBuilder.Configurations.Add(new TaskReminderMap());
            modelBuilder.Configurations.Add(new TaskTypeMap());
            modelBuilder.Configurations.Add(new TopicTagMap());
            modelBuilder.Configurations.Add(new UserBankAccountMap());
            modelBuilder.Configurations.Add(new UserChatroomMap());
            modelBuilder.Configurations.Add(new UserConnectionMap());
            modelBuilder.Configurations.Add(new UserGroupMap());
            modelBuilder.Configurations.Add(new UserLoanMap());
            modelBuilder.Configurations.Add(new UserStockMap());
            modelBuilder.Configurations.Add(new UserTaskMap());
            modelBuilder.Configurations.Add(new UserVoteMap());
            modelBuilder.Configurations.Add(new UserVoteChoiceMap());
            modelBuilder.Configurations.Add(new UserVoteSelectedChoiceMap());
            modelBuilder.Configurations.Add(new WeaponTypeMap());
            modelBuilder.Configurations.Add(new WebUserMap());
            modelBuilder.Configurations.Add(new WebUserLocationMap());
            modelBuilder.Configurations.Add(new WebUserUpdateMap());
        }
    }
}
