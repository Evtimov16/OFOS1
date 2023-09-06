using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using Moq;
using System.IO;

namespace OFOS
{
    public class AddItemsTests
    {
        
        public void Page_Load_RedirectsToAdminLoginWhenSessionIsNull()
        {
            // Arrange
            var page = new Add_items();
            var context = new HttpContext(
                new HttpRequest("", "http://localhost", ""),
                new HttpResponse(new StringWriter())
            );
            HttpContext.Current = context;
            context.Session["admin"] = null;

            // Act
            page.Page_Load(null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(context.Response.RedirectLocation.Contains("Admin_Login.aspx"));
        }

        
        public void Page_Load_DoesNotRedirectWhenSessionIsNotNull()
        {
            // Arrange
            var page = new Add_items();
            var context = new HttpContext(
                new HttpRequest("", "http://localhost", ""),
                new HttpResponse(new StringWriter())
            );
            HttpContext.Current = context;
            context.Session["admin"] = "some_value";

            // Act
            page.Page_Load(null, EventArgs.Empty);

            // Assert
            Assert.IsNull(context.Response.RedirectLocation);
        }
    }
}
