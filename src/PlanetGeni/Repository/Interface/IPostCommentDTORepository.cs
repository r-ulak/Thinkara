using DAO.Models;
using DTO.Custom;
using DTO.Db;
using System;
using System.Linq;


namespace Repository
{
    public interface IPostCommentDTORepository
    {
        IQueryable<CommentDTO> GetMoreCommentList(string postId, Nullable<DateTime> lastDateTime, int userId);
        IQueryable<CommentDTO> GetMoreChildCommentList(int userId, string parentCommentId, Nullable<DateTime> lastDateTime = null);
        bool SavePost(Post postDetail);
        bool SavePostComment(CommentDTO postCommentDetail);
        PostCommentDTO GetPostCommentList(GetPostDTO postDto);
        void SendBudgetImpNotify();
        void SaveUserDig(UserDigDTO userDig);
        void CalculateSpotTotal(ref BuySpotDTO spotDetails);
        bool SaveSpot(BuySpotDTO spotDetails);
    }
}
