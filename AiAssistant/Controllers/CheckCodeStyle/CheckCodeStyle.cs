using AiAssistant.Models.CheckCodeStyle;
using Microsoft.AspNetCore.Mvc;

namespace AiAssistant.Controllers.CheckCodeStyle
{
    public class CheckCodeStyle : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public CheckCodeStyleResponse generateCodeStyle (CheckCodeStyleRequest request)
        {


            return new CheckCodeStyleResponse();
        }
    }
}
