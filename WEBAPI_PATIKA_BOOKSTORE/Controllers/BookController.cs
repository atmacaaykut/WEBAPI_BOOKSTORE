using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBAPI_PATIKA_BOOKSTORE.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private static List<Book> BookList = new List<Book>()
        {
            new Book
            {
                Id=1,
                Title="Lean Startup",
                GenreId=1, //Personel Growth
                PageCount=200,
                PublishDate=new DateTime(2001,06,12)
            },
            new Book
            {
                Id=2,
                Title="Herland",
                GenreId=2, //Personel Growth
                PageCount=250,
                PublishDate=new DateTime(2010,05,23)
            },
            new Book
            {
                Id=3,
                Title="Dune",
                GenreId=2, //Personel Growth
                PageCount=540,
                PublishDate=new DateTime(2001,12,21)
            },
        };  //Liste sonu(satır sonu) olduğu için noktalı virgülle kapatılmalı

        [HttpGet]
        public List<Book> GetBooks()
        {
            var bookList = BookList.OrderBy(x => x.Id).ToList<Book>();
            return bookList;
        }

        [HttpGet("{id}")]   //id root'tan alınır. Sık kullanılan yöntemdir
        public Book GetById(int id)
        {
            var book = BookList.Where(book => book.Id == id).SingleOrDefault();
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
            var book = BookList.SingleOrDefault(x => x.Title == newBook.Title);   //title(Kitap adı) ile kontrol eder 

            if (book is not null)
            {
                return BadRequest();     //Kayıt mevcut ise BadRequest döndürür
            }
            else
            {
                BookList.Add(newBook);  //Yeni kaydı ekler
                return Ok();            //Ok döner
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id,[FromBody] Book updatedBook)    //id kayıt kontrolü için kullanılır
        {
            var book = BookList.SingleOrDefault(x => x.Id == id);       //Id değeri=id olan değeri book değerine atar

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

                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = BookList.SingleOrDefault(x => x.Id == id);
            if (book is null)
            {
                return BadRequest();
            }
            else
            {
                BookList.Remove(book);
                return Ok();
            }
        }
    }
}
