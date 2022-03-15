using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBAPI_PATIKA_BOOKSTORE.DBOperations;

namespace WEBAPI_PATIKA_BOOKSTORE.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        
        public BookController (BookStoreDbContext context)
        {
            _context = context;
        }

        //private static List<Book> BookList = new List<Book>()
        //{
        //    new Book
        //    {
        //        Id=1,
        //        Title="Lean Startup",
        //        GenreId=1, //Personel Growth
        //        PageCount=200,
        //        PublishDate=new DateTime(2001,06,12)
        //    },
        //    new Book
        //    {
        //        Id=2,
        //        Title="Herland",
        //        GenreId=2, //Personel Growth
        //        PageCount=250,
        //        PublishDate=new DateTime(2010,05,23)
        //    },
        //    new Book
        //    {
        //        Id=3,
        //        Title="Dune",
        //        GenreId=2, //Personel Growth
        //        PageCount=540,
        //        PublishDate=new DateTime(2001,12,21)
        //    },
        //};  //Liste sonu(satır sonu) olduğu için noktalı virgülle kapatılmalı

        [HttpGet]
        public List<Book> GetBooks()
        {
            var bookList = _context.Books.OrderBy(x => x.Id).ToList<Book>();
            return bookList;
        }

        [HttpGet("{id}")]   //id root'tan alınır. Sık kullanılan yöntemdir
        public Book GetById(int id)
        {
            var book = _context.Books.Where(book => book.Id == id).SingleOrDefault();
            return book;
        }

        //[HttpGet]
        //public Book Get([FromQuery] string id)  //id sorguyla(querystring) alınır. Tercih edilen bir yöntem değildir
        //{
        //    var book = BookList.Where(book => book.Id == Convert.ToInt32(id)).SingleOrDefault();
        //    return book;
        //}

        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook)    //IAction başarılı veya başarısız diye cevap döner. frombody post isteğini body'den alır
        {
            var book = _context.Books.SingleOrDefault(x => x.Title == newBook.Title);   //title(Kitap adı) ile kontrol eder 

            if (book is not null)
            {
                return BadRequest();     //Kayıt mevcut ise BadRequest döndürür
            }
            else
            {
                _context.Books.Add(newBook);  //Yeni kaydı ekler
                _context.SaveChanges();      //Execute eder
                return Ok();            //Ok döner
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id,[FromBody] Book updatedBook)    //id kayıt kontrolü için kullanılır
        {
            var book = _context.Books.SingleOrDefault(x => x.Id == id);       //Id değeri=id olan değeri book değerine atar

            if (book is null)                               //böyle bir değer yoksa badrequest döndürür.
            {
                return BadRequest();
            }
            else
            {
                book.GenreId = updatedBook.GenreId != default ? updatedBook.GenreId : book.GenreId;     //Kullanıcının girdiği değer default'tan farklıysa (swagger'da default 0) değeri girilen değer yapar. Yoksa değeri değiştirmez
                book.PageCount = updatedBook.PageCount != default ? updatedBook.PageCount : book.PageCount;
                book.PublishDate = updatedBook.PublishDate != default ? updatedBook.PublishDate : book.PublishDate;
                book.Title = updatedBook.Title != default ? updatedBook.Title : book.Title;

                _context.SaveChanges();
                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.SingleOrDefault(x => x.Id == id);
            if (book is null)
            {
                return BadRequest();
            }
            else
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                return Ok();
            }
        }
    }
}
