using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO.DAO.ComplexType;
using DAO.Models;

namespace DAO.DAO.ContentModel
{

    public class PostACLContent
    {
        public string ACLType { get; set; }
        public int ACLId { get; set; }

    }

    public class PostViewModel
    {
        public List<PostACLContent> PostPositiveACLList { get; set; }
        public List<PostACLContent> PostNegativeACLList { get; set; }
        public string PostContent { get; set; }
        public sbyte CommentEnabled { get; set; }
        public List<ContentProviderResult> PostUrlContent { get; set; }

    }
}
