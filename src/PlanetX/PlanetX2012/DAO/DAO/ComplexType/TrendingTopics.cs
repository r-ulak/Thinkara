
using System.Collections;
using System.Runtime.Serialization;
namespace DAO.DAO
{
    [DataContract]
    public class TrendingTopics
    {
        [DataMember]
        public string Tag { get; set; }
        [DataMember]
        public int TagCount { get; set; }

        public override bool Equals(object other)
        {
            var toCompareWith = other as TrendingTopics;
            if (toCompareWith == null)
                return false;

            return this.TagCount == toCompareWith.TagCount &&
              this.Tag == toCompareWith.Tag;
        }

        public override int GetHashCode()
        {
            return (TagCount.GetHashCode() * Tag.GetHashCode());

        }
    }

}
