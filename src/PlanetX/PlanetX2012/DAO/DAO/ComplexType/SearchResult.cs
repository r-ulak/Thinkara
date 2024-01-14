using System.Runtime.Serialization;
namespace DAO.DAO
{
    [DataContract]
    public class SearchResult
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Picture { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
}