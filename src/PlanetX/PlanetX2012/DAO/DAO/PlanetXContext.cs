using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DAO.Models.Mapping;

namespace DAO.Models
{
    public partial class PlanetXContext : DbContext
    {
        static PlanetXContext()
        {
            Database.SetInitializer<PlanetXContext>(null);
        }

        public PlanetXContext()
            : base("Name=PlanetXContext")
        {
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<BookmarkCategory> BookmarkCategories { get; set; }
        public DbSet<BookmarkInfo> BookmarkInfoes { get; set; }
        public DbSet<BookmarkSubCategory> BookmarkSubCategories { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessCode> BusinessCodes { get; set; }
        public DbSet<BusinessLocation> BusinessLocations { get; set; }
        public DbSet<BusinessSubCode> BusinessSubCodes { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<CityCode> CityCodes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CountryCode> CountryCodes { get; set; }
        public DbSet<DegreeCode> DegreeCodes { get; set; }
        public DbSet<Education> Educations { get; set; }
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
        public DbSet<Goverment> Goverments { get; set; }
        public DbSet<GovermentProvince> GovermentProvinces { get; set; }
        public DbSet<GroupMembership> GroupMemberships { get; set; }
        public DbSet<ItemCode> ItemCodes { get; set; }
        public DbSet<Lang> Langs { get; set; }
        public DbSet<LeaderCode> LeaderCodes { get; set; }
        public DbSet<LoanCode> LoanCodes { get; set; }
        public DbSet<LoanFromBusiness> LoanFromBusinesses { get; set; }
        public DbSet<LoanFromPerson> LoanFromPersons { get; set; }
        public DbSet<MajorCode> MajorCodes { get; set; }
        public DbSet<Merchandise> Merchandises { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MilitaryForce> MilitaryForces { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Privacy> Privacies { get; set; }
        public DbSet<PrivacyType> PrivacyTypes { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProvinceCode> ProvinceCodes { get; set; }
        public DbSet<RsvpStatusCode> RsvpStatusCodes { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ThumbUpDown> ThumbUpDowns { get; set; }
        public DbSet<UniversityCode> UniversityCodes { get; set; }
        public DbSet<UniversityLocation> UniversityLocations { get; set; }
        public DbSet<UserChatroom> UserChatrooms { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<WebUser> WebUsers { get; set; }
        public DbSet<WebUserLocation> WebUserLocations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AssetMap());
            modelBuilder.Configurations.Add(new AvatarMap());
            modelBuilder.Configurations.Add(new BlogMap());
            modelBuilder.Configurations.Add(new BookmarkMap());
            modelBuilder.Configurations.Add(new BookmarkCategoryMap());
            modelBuilder.Configurations.Add(new BookmarkInfoMap());
            modelBuilder.Configurations.Add(new BookmarkSubCategoryMap());
            modelBuilder.Configurations.Add(new BusinessMap());
            modelBuilder.Configurations.Add(new BusinessCodeMap());
            modelBuilder.Configurations.Add(new BusinessLocationMap());
            modelBuilder.Configurations.Add(new BusinessSubCodeMap());
            modelBuilder.Configurations.Add(new CardMap());
            modelBuilder.Configurations.Add(new ChatMap());
            modelBuilder.Configurations.Add(new CityCodeMap());
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new CountryCodeMap());
            modelBuilder.Configurations.Add(new DegreeCodeMap());
            modelBuilder.Configurations.Add(new EducationMap());
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
            modelBuilder.Configurations.Add(new GovermentMap());
            modelBuilder.Configurations.Add(new GovermentProvinceMap());
            modelBuilder.Configurations.Add(new GroupMembershipMap());
            modelBuilder.Configurations.Add(new ItemCodeMap());
            modelBuilder.Configurations.Add(new LangMap());
            modelBuilder.Configurations.Add(new LeaderCodeMap());
            modelBuilder.Configurations.Add(new LoanCodeMap());
            modelBuilder.Configurations.Add(new LoanFromBusinessMap());
            modelBuilder.Configurations.Add(new LoanFromPersonMap());
            modelBuilder.Configurations.Add(new MajorCodeMap());
            modelBuilder.Configurations.Add(new MerchandiseMap());
            modelBuilder.Configurations.Add(new MessageMap());
            modelBuilder.Configurations.Add(new MilitaryForceMap());
            modelBuilder.Configurations.Add(new NotificationMap());
            modelBuilder.Configurations.Add(new PrivacyMap());
            modelBuilder.Configurations.Add(new PrivacyTypeMap());
            modelBuilder.Configurations.Add(new ProfileMap());
            modelBuilder.Configurations.Add(new ProvinceCodeMap());
            modelBuilder.Configurations.Add(new RsvpStatusCodeMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new ThumbUpDownMap());
            modelBuilder.Configurations.Add(new UniversityCodeMap());
            modelBuilder.Configurations.Add(new UniversityLocationMap());
            modelBuilder.Configurations.Add(new UserChatroomMap());
            modelBuilder.Configurations.Add(new UserConnectionMap());
            modelBuilder.Configurations.Add(new UserGroupMap());
            modelBuilder.Configurations.Add(new WebUserMap());
            modelBuilder.Configurations.Add(new WebUserLocationMap());
        }
    }
}
