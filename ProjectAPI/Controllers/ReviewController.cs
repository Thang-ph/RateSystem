using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Models;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ReviewController( IMapper mapper)
        {
            _mapper = mapper;
        }
        public ProjectPRN231Context context=new ProjectPRN231Context();
        [HttpGet("Rating/productId")]
        public IActionResult getAllStar(int productId)
        {
            List<Review>  reviews = context.Reviews.Where(x=>x.ProductId == productId).ToList();
           float aveRate = 0;
            foreach (Review review in reviews) {
                aveRate += review.Star.Value;
            }
            if (reviews.Count > 0)
            {
                aveRate = aveRate / reviews.Count;
            }
            return Ok(aveRate);
        }
        [HttpGet("Rating/productId/date")]
        public IActionResult GetAverageRatingBeforeDate(int productId, DateTime date)
        {
            var reviews = context.Reviews
                                 .Where(x => x.ProductId == productId && x.CreateAt < date)
                                 .ToList();
            if (reviews.Count == 0)
            {
                return Ok(0);
            }
            float aveRate = 0;
            foreach (Review review in reviews)
            {
                aveRate += review.Star.Value;
            }
            if (reviews.Count > 0)
            {
                aveRate = aveRate / reviews.Count;
            }
            return Ok(aveRate);
        }

        [HttpGet("GetAllRating/productId")]
        public IActionResult getAllRating(int productId)
        {
            List<Review> reviews = context.Reviews.Where(x => x.ProductId == productId).ToList();
           
            return Ok(reviews);
        }
        [HttpGet("GetAllComment/productId")]
        public IActionResult getAllComment(int productId)
        {
            var comments = context.Comments
            .Include(x => x.User)
            .Where(x => x.ProductId == productId)
            .ToList();

            var commentDtos = _mapper.Map<List<CommentDTO>>(comments);

            return Ok(commentDtos);
        }
        [HttpGet("GetAllComment/productId/pageNumber/pageSize")]
        public IActionResult getAllCommentByPage(int productId,int pageNumber, int pageSize)
        {
            var comments =  context.Comments
        .Include(x => x.User)
        .Where(x => x.ProductId == productId)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList();

            var commentDtos = _mapper.Map<List<CommentDTO>>(comments);

            return Ok(commentDtos);
        }
        [HttpPost("Rating/productId/star")]
        public IActionResult Rate(int productId, int star)
        {
            Review r = context.Reviews.FirstOrDefault(x => x.UserId == Account.CurrentUser.UserId && x.ProductId == productId);
            if (r == null) {
                Review review = new Review();
                review.ProductId = productId;
                review.Star = star;
                review.UserId = Account.CurrentUser.UserId;
                context.Reviews.Add(review);
                context.SaveChanges();
                return Ok(review);
            }
            else
            {
                r.Star = star;
                context.Reviews.Update(r);
                context.SaveChanges();
                return Ok(r);
            }
        }
        [HttpGet("GetRate/productId")]
        public IActionResult GetRate(int productId)
        {
            Review r = context.Reviews.FirstOrDefault(x => x.UserId == Account.CurrentUser.UserId && x.ProductId == productId);
            if (r != null)
            {
                return Ok(r);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        /// 
        [HttpGet("GetRate/categoryId")]
        public IActionResult GetRateByCategory(int categoryId)
        {
            List<Review> reviews = context.Reviews.Where(x=>x.Product.CategoryId == categoryId).ToList();
            float aveRate = 0;
            foreach (Review review in reviews)
            {
                aveRate += review.Star.Value;
            }
            if (reviews.Count > 0)
            {
                aveRate = aveRate / reviews.Count;
            }
            return Ok(aveRate);
        }

        [HttpPost("Comment/productId")]
        public IActionResult Comment(int productId, string comment)
        {
           Comment c=new Comment();
            c.ProductId = productId;
            c.UserId=Account.CurrentUser.UserId;    
            c.ContentComment = comment;
            c.CreateAt = DateTime.Now;
            c.UserId = Account.CurrentUser.UserId;
            context.Comments.Add(c);
            context.SaveChanges();
            return Ok(c);
        }

        [HttpPost("Comment/Reply/productId/fatherID")]
        public IActionResult Reply(int productId, string comment,int fatherID)
        {
            Comment c = new Comment();
            c.ProductId = productId;
            c.UserId = Account.CurrentUser.UserId;
            c.ContentComment = comment;
            c.CreateAt = DateTime.Now;
            c.FatherId=fatherID;
            c.UserId = Account.CurrentUser.UserId;
            context.Comments.Add(c);
            context.SaveChanges();
            return Ok(c);
        }
        [HttpGet("RateComment/commentId")]
        public IActionResult GetRateComment(int commentId)
        {
            List<CommentRate> reviews=context.CommentRates.Where(x=>x.CommentId==commentId).ToList();
            float aveRate = 0;
            foreach (CommentRate review in reviews)
            {
                aveRate += review.Rate.Value;
            }
            if (reviews.Count > 0)
            {
                aveRate = aveRate / reviews.Count;
            }
            return Ok(aveRate);

        }
        
        [HttpPost("Comment/Delete/commentId")]
        public IActionResult DeleteComment(int commentId)
        {
            Comment c = context.Comments.FirstOrDefault(x=>x.CommentId==commentId);
            context.Comments.Remove(c);
            context.SaveChanges();
            return Ok(c);
        }
        [HttpPost("Comment/Edit/commentId")]
        public IActionResult EditComment(int commentId,Comment comment)
        {
            Comment c = context.Comments.FirstOrDefault(x => x.CommentId == commentId);
            c.ContentComment = comment.ContentComment;
            c.CreateAt= DateTime.Now;
            context.Comments.Update(c);
            context.SaveChanges();
            return Ok(c);
        }


    }
}
