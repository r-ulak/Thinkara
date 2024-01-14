using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanetX2012.Models.ContentModel;
using PlanetX2012.Models.DAO;
using DAO.Models;
using PlanetX2012.UserStatusManager;
using DAO.DAO.ContentModel;
using System.Data.Entity.Validation;
using DAO.DAO.ComplexType;
using System.Net.Http;
using System.Configuration;
using System.Text;



namespace PlanetX2012.Controllers.Home
{
    public class PostController : Controller
    {
        //
        // GET: /Post/
        private Uri trendingTopicsAddUrl = new Uri(ConfigurationManager.AppSettings["trendingTopic.AddTopicsUrl"]);

        private HttpClient client = new HttpClient();


        public ActionResult Index()
        {
            return View();
        }

        //private ChatWithTracking chatController = new ChatWithTracking();
        //[OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public JsonResult GetPostsForUser(int postid = 0)
        {
            PlanetXContext db = new PlanetXContext();
            int userId = (int)Session["WebUserId"];
            try
            {
                IEnumerable<Post> userPosts;
                IEnumerable<PostComment> userPostsComment;
                IEnumerable<PostWebContent> userPostsWebContent;
                StoredProcedure sp = new StoredProcedure();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("webUserId", userId);
                dictionary.Add("userPostId", postid);
                dictionary.Add("postLimit", 15);

                userPosts = sp.GetSqlData<Post>("GetPostForUserWithLimit", dictionary);
                dictionary.Clear();
                string postIdList = String.Join(",", userPosts.Select(x => x.PostId.ToString()).ToArray());
                dictionary.Add("postIdList", postIdList);
                userPostsComment = sp.GetSqlData<PostComment>("GetCommentsByPost", dictionary);

                dictionary.Clear();
                dictionary.Add("postIdList", postIdList);
                userPostsWebContent = sp.GetSqlData<PostWebContent>("GetWebContentByPost", dictionary);
                return Json(new
                {
                    Posts = userPosts,
                    Comments = userPostsComment,
                    WebContent = userPostsWebContent
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = 0, messgae = ex.Message + ex.StackTrace.ToString() });

            }
            finally
            {
                //db.Dispose();
            }


        }

        [HttpPost]
        public JsonResult AddPostsForUser(PostViewModel postViewModel
            )
        {
            try
            {
                PlanetXContext db = new PlanetXContext();
                Post postModel = new Post();
                int userId = (int)Session["WebUserId"];
                postModel.PostContent = postViewModel.PostContent;
                postModel.UpdatedAt = DateTime.Now;
                postModel.CreatedAt = DateTime.Now;
                postModel.UserId = userId;
                postModel.Slug = " ";
                postModel.CommentEnabled = postModel.CommentEnabled;
                postModel.PostComments = postModel.PostComments;

                if ((postViewModel.PostPositiveACLList != null ?
                    postViewModel.PostPositiveACLList.Select(p => p.ACLType == "Club").Count() > 0 : false) ||
                    (postViewModel.PostNegativeACLList != null ?
                    postViewModel.PostNegativeACLList.Select(p => p.ACLType == "Club").Count() > 0 : false))
                    postModel.ClubACL = 0;
                else
                    postModel.ClubACL = 1;


                if ((postViewModel.PostPositiveACLList != null ?
                               postViewModel.PostPositiveACLList.Select(p => p.ACLType == "Friend").Count() > 0 : false) ||
                               (postViewModel.PostNegativeACLList != null ?
                               postViewModel.PostNegativeACLList.Select(p => p.ACLType == "Friend").Count() > 0 : false))
                    postModel.UserACL = 0;
                else
                    postModel.UserACL = 1;

                db.Posts.Add(postModel);
                db.SaveChanges();


                client.PostAsync(trendingTopicsAddUrl, new StringContent(PutIntoQuotes(postViewModel.PostContent), Encoding.UTF8, "application/json"));



                if (postViewModel.PostUrlContent != null)
                    AddWebContent(postViewModel.PostUrlContent, postModel.PostId);

                if (postViewModel.PostPositiveACLList != null)
                {
                    foreach (PostACLContent item in postViewModel.PostPositiveACLList)
                    {

                        if (item.ACLType == "Club")
                        {
                            AddPostClubACL(new PostClubACL { ClubId = item.ACLId, PostId = postModel.PostId, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, AccessType = 1 });

                        }
                        else if (item.ACLType == "Friend")
                        {
                            AddPostUserACL(new PostUserACL { UserId = item.ACLId, PostId = postModel.PostId, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, AccessType = 1 });

                        }

                    }
                }

                if (postViewModel.PostNegativeACLList != null)
                {
                    foreach (PostACLContent item in postViewModel.PostNegativeACLList)
                    {

                        if (item.ACLType == "Club")
                        {
                            AddPostClubACL(new PostClubACL { ClubId = item.ACLId, PostId = postModel.PostId, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, AccessType = 0 });

                        }
                        else if (item.ACLType == "Friend")
                        {
                            AddPostUserACL(new PostUserACL { UserId = item.ACLId, PostId = postModel.PostId, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, AccessType = 0 });

                        }

                    }
                }
                string postWebContent = String.Empty;

                if (postViewModel.PostUrlContent != null)
                {
                    postWebContent = String.Join("",
                   postViewModel.PostUrlContent.Where(p => p.Processed == true
                       && p.Title != String.Empty && p.Uri != String.Empty
                       && p.Content != String.Empty).Select(c => c.Content));
                }
                return Json(new
                {
                    success = 1,
                    PostId = postModel.PostId,
                    PostContent = postModel.PostContent,
                    WebContent = postWebContent
                });

            }

            catch (DbEntityValidationException dbEx)
            {
                return Json(new { success = 0, messgae = dbEx.Message + dbEx.StackTrace.ToString() });
            }
            catch (Exception ex)
            {

                return Json(new { success = 0, messgae = ex.Message + ex.StackTrace.ToString() });
            }
            finally
            {
                //db.Dispose();
            }


        }


        private void AddPostClubACL(PostClubACL postClubACLViewModel)
        {
            using (var db = new PlanetXContext())
            {

                if (db.PostClubACLs.Any
                        (e => e.PostId == postClubACLViewModel.PostId
                               && e.ClubId == postClubACLViewModel.ClubId))
                {
                    db.PostClubACLs.Attach(postClubACLViewModel);

                }
                else
                {
                    db.PostClubACLs.Add(postClubACLViewModel);
                }
                db.SaveChanges();

            }
        }
        private void AddPostUserACL(PostUserACL postUserACLViewModel)
        {

            using (var db = new PlanetXContext())
            {

                if (db.PostUserACLs.Any
                        (e => e.PostId == postUserACLViewModel.PostId
                               && e.UserId == postUserACLViewModel.UserId))
                {
                    db.PostUserACLs.Attach(postUserACLViewModel);

                }
                else
                {
                    db.PostUserACLs.Add(postUserACLViewModel);
                }
                db.SaveChanges();

            }
        }
        private void AddWebContent(List<ContentProviderResult> postWebContentModel, int postId)
        {

            using (var db = new PlanetXContext())
            {
                int userId = (int)Session["WebUserId"];



                foreach (ContentProviderResult item in postWebContentModel)
                {
                    if (item.Content != null && item.Content != string.Empty)
                    {
                        PostWebContent postWebContent = new PostWebContent();
                        postWebContent.Uri = item.Uri;
                        postWebContent.UserId = userId;
                        postWebContent.PostId = postId;
                        postWebContent.Title = item.Title;
                        postWebContent.Content = item.Content;
                        postWebContent.CreatedAt = DateTime.Now;
                        postWebContent.UpdatedAt = DateTime.Now;
                        db.PostWebContents.Add(postWebContent);
                    }
                }
                db.SaveChanges();

            }
        }
        public JsonResult AddPostCommentForUser(string comment, int parentCommentId, int postId)
        {
            try
            {
                PostComment postCommentViewModel = new PostComment();
                PlanetXContext db = new PlanetXContext();
                int userId = (int)Session["WebUserId"];
                postCommentViewModel.UpdatedAt = DateTime.Now;
                postCommentViewModel.CreatedAt = DateTime.Now;
                postCommentViewModel.PostId = postId;
                postCommentViewModel.UserId = userId;
                postCommentViewModel.Comment = comment;
                postCommentViewModel.ParentCommentId = parentCommentId;
                db.PostComments.Add(postCommentViewModel);
                db.SaveChanges();

                client.PostAsync(trendingTopicsAddUrl, new StringContent(PutIntoQuotes(comment), Encoding.UTF8, "application/json"));

                return Json(new
                {
                    success = 1,
                    postCommentId = postCommentViewModel.PostCommentId,
                    parentCommentId = postCommentViewModel.ParentCommentId,
                    postComment = postCommentViewModel.Comment,
                    postId = postCommentViewModel.PostId

                });

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return Json(new { success = 0 });
            }
            catch (Exception)
            {

                return Json(new { success = 0 });
            }
            finally
            {
                //db.Dispose();
            }


        }

        private string PutIntoQuotes(string value)
        {
            return "\"" + value + "\"";
        }

    }
}
