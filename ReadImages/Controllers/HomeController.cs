using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace ReadImages.Controllers
{
     public class HomeController : Controller
     {
          //
          // GET: /Home/

          public ActionResult Index()
          {
               return View( new List<Color>() );
          }

          [HttpPost]
          public ViewResult Index( HttpPostedFileBase file )
          {
               IPicture picture = new Imaging( file );
               return View( picture.GetTopColorsFromImage() );
          }

     }
}
