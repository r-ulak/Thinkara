using Common;
using DAO;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PostCommentDTORepository : IPostCommentDTORepository
    {
        private StoredProcedure spContext = new StoredProcedure();
        public PostCommentDTORepository()
        {
        }

        public PostCommentDTO GetPostCommentList(GetPostDTO postDto)
        {
            PostCommentDTO postComment = new PostCommentDTO();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", postDto.UserId);
            dictionary.Add("lastPostId", postDto.LastPostId);
            dictionary.Add("lastCreatedAt", postDto.LastCreatedAt);
            dictionary.Add("postLimit", AppSettings.PostLimit);
            dictionary.Add("parmCountryId", postDto.CountryId);
            dictionary.Add("parmPartyId", postDto.PartyId);

            IEnumerable<PostDTO> userPosts;
            if (postDto.NewPost)
            {
                userPosts = spContext.GetSqlData<PostDTO>(AppSettings.SPGetNewUserPostWithLimit, dictionary);
            }
            else
            {
                userPosts = spContext.GetSqlData<PostDTO>(AppSettings.SPGetPostForUserWithLimit, dictionary);
            }
            dictionary.Clear();


            if (userPosts.Count() > 0)
            {

                string postIdList = String.Join(",", userPosts.Select(x => x.PostId.ToString()).ToArray());
                dictionary.Add("postIdList", postIdList);
                dictionary.Add("parmUserId", postDto.UserId);
                dictionary.Add("commentLimit", AppSettings.InitialCommentLimit);
                IEnumerable<CommentDTO> userPostsComment = spContext.GetSqlData<CommentDTO>(AppSettings.SPGetCommentsForPosts, dictionary);
                postComment.Posts = userPosts.ToArray();
                postComment.PostComments = userPostsComment.ToArray();
            }
            return postComment;
        }

        public IQueryable<CommentDTO> GetMoreCommentList(string postId, Nullable<DateTime> lastDateTime, int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPostId", postId);
            dictionary.Add("parmUserId", userId);
            dictionary.Add("lastDateTime", lastDateTime);
            dictionary.Add("commentLimit", AppSettings.CommentLimit);
            IEnumerable<CommentDTO> postComment = spContext.GetSqlData<CommentDTO>(AppSettings.SPGetCommentList, dictionary);
            return postComment.AsQueryable();
        }

        public IQueryable<CommentDTO> GetMoreChildCommentList(int userId, string parentCommentId, Nullable<DateTime> lastDateTime = null)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parentCommentId", parentCommentId);
            dictionary.Add("parmUserId", userId);
            dictionary.Add("lastDateTime", lastDateTime);
            dictionary.Add("commentLimit", AppSettings.CommentLimit);
            IEnumerable<CommentDTO> postComment = spContext.GetSqlData<CommentDTO>(AppSettings.SPGetChildCommentsForParent, dictionary);
            return postComment.AsQueryable();
        }
        public bool SavePost(Post postDetail)
        {
            bool result = false;
            try
            {
                DateTime dateTime = DateTime.UtcNow;
                postDetail.IsSpam = false;
                postDetail.IsDeleted = false;
                postDetail.IsApproved = true;
                postDetail.CoolIt = 0;
                postDetail.DigIt = 0;
                postDetail.ChildCommentCount = 0;
                postDetail.CreatedAt = dateTime;
                postDetail.UpdatedAt = dateTime;
                postDetail.ImageName = "";
                if (postDetail.PostId == null || postDetail.PostId == Guid.Empty)
                {
                    postDetail.PostId = Guid.NewGuid();
                }


                spContext.Add(postDetail);

                return result;

            }
            catch (Exception ex)
            {
                result = false;
                ExceptionLogging.LogError(ex, "Error Saving Post to repository");
                return result;
            }


        }
        public bool SavePostComment(CommentDTO postCommentDetail)
        {
            bool result = false;
            try
            {
                PostComment postComment = new PostComment();
                postComment.UserId = postCommentDetail.UserId;
                postComment.IsSpam = false;
                postComment.IsDeleted = false;
                postComment.IsApproved = true;
                postComment.CoolIt = 0;
                postComment.DigIt = 0;
                postComment.ChildCommentCount = 0;
                postComment.ParentCommentId = postCommentDetail.ParentCommentId;
                postComment.PostCommentId = postCommentDetail.PostCommentId;
                postComment.PostId = postCommentDetail.PostId;
                postComment.CreatedAt = postCommentDetail.CreatedAt;
                postComment.CommentText = postCommentDetail.CommentText;
                spContext.Add(postComment);

                string spSql = AppSettings.SPUpdateCommentCount;


                if (postCommentDetail.ParentCommentId != null)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("parmPostId", postCommentDetail.PostId);
                    dictionary.Add("parmParentCommentId", postCommentDetail.ParentCommentId);
                    int count = spContext.ExecuteStoredProcedure(spSql, dictionary);
                    if (count == 0)
                    {
                        result = false;
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                result = false;
                ExceptionLogging.LogError(ex, "Error Saving Post to repository");
                return result;
            }


        }
        public void SendBudgetImpNotify()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPostContentTypeId", AppSettings.BudgetImplentationContentTypeId);
            spContext.ExecuteStoredProcedure(AppSettings.SPSendBudgetImpNotify, dictionary);
        }

        public void SaveUserDig(UserDigDTO userDigdto)
        {
            try
            {
                if (userDigdto.DigType > 0)
                {
                    UserDig userDig = new UserDig
                    {
                        DigType = userDigdto.DigType,
                        PostCommentId = userDigdto.PostCommentId,
                        UserId = userDigdto.UserId
                    };
                    spContext.AddUpdate(userDig);
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("parmPostCommentId", userDigdto.PostCommentId);
                    dictionary.Add("parmDigType", userDigdto.DigType);
                    dictionary.Add("parmOldDigType", userDigdto.OldDigType);
                    dictionary.Add("parmPostCommentType", userDigdto.PostCommentType);

                    spContext.ExecuteStoredProcedure(AppSettings.SPUpdateUserDigAdd, dictionary);
                }
                else
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("parmPostCommentId", userDigdto.PostCommentId);
                    dictionary.Add("parmOldDigType", userDigdto.OldDigType);
                    dictionary.Add("parmPostCommentType", userDigdto.PostCommentType);
                    dictionary.Add("parmUserId", userDigdto.UserId);

                    spContext.ExecuteStoredProcedure(AppSettings.SPUpdateUserDigMinus, dictionary);


                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error Saving SavePostVote");
            }
        }

        public void CalculateSpotTotal(ref BuySpotDTO spotDetails)
        {
            ICountryTaxDetailsDTORepository taxRepo = new CountryTaxDetailsDTORepository();
            decimal taxRate = taxRepo.GetCountryTaxByCode(spotDetails.CountryId, AppSettings.TaxAdsCode);

            decimal costTotal = spotDetails.Message.Length;
            decimal taxTotal = 0;
            decimal total = 0;
            costTotal += spotDetails.ImageBufferLength * RulesSettings.SpotImageCostFactor;
            costTotal = costTotal * RulesSettings.SpotCostFactor;
            taxTotal = (taxRate / 100) * costTotal;
            total = costTotal + taxTotal;
            spotDetails.CalculatedTotalCost = total;
            spotDetails.CalculatedTax = taxTotal;
        }
        public bool SaveSpot(BuySpotDTO spotDetails)
        {
            try
            {
                IUserBankAccountDTORepository bankrepo = new UserBankAccountDTORepository();

                PayNationDTO payNation = new PayNationDTO()
                {
                    Amount = spotDetails.CalculatedTotalCost - spotDetails.CalculatedTax,
                    CountryId = spotDetails.CountryId,
                    CountryUserId = spotDetails.CountryUserId,
                    FundType = AppSettings.AdsFundType,
                    TaskId = Guid.NewGuid(),
                    Tax = spotDetails.CalculatedTax,
                    TaxCode = (sbyte)AppSettings.TaxAdsCode,
                    UserId = spotDetails.UserId
                };

                if (bankrepo.PayNation(payNation) == 1)
                {
                    Post newPost = new Post()
                    {
                        ChildCommentCount = 0,
                        CommentEnabled = true,
                        CoolIt = 0,
                        CountryId = null,
                        CreatedAt = DateTime.UtcNow,
                        DigIt = 0,
                        ImageName = spotDetails.ImageName,
                        IsApproved = true,
                        IsDeleted = false,
                        IsSpam = false,
                        Parms = null,
                        PartyId = null,
                        PostContent = spotDetails.PreviewMsg,
                        PostContentTypeId = 0,
                        PostId = Guid.NewGuid(),
                        PostTitle = string.Empty,
                        UpdatedAt = DateTime.UtcNow,
                        UserId = spotDetails.UserId
                    };

                    spContext.Add(newPost);
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error Saving Spot to repository");
                return false;

            }
        }
    }
}
