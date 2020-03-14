using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeLibrary.Objects;
using RecipeLibrary;
using RecipeLibrary.Objects.Boxes.Elements;

namespace WebServer.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class PageController : ControllerBase
    {
        private readonly ILogger<PageController> _logger;

        public PageController(ILogger<PageController> logger)
        {
            _logger = logger;
        }


        [HttpGet("getPage")]
        public List<RecipeShort> Get(string section, int page = 1, string recipeName = null)
        {
            if (recipeName == null)
                recipeName = string.Empty;
            
            try
            {
                return GetData.GetPage(section, page, recipeName.ToLower()).Result;
            }
            catch (Exception)
            {
                return new List<RecipeShort> {new RecipeShort("error", new Picture("error"), "error")};
            }
        }
    }
}