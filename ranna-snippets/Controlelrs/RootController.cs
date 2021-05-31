using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ranna_snippets.Controlelrs
{
    [Route("/")]
    [ApiVersion("1.0")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private readonly string rootRedirect;

        public RootController(IConfiguration config)
        {
            rootRedirect = config.GetValue<string>("Routing:RootRedirect");
        }

        [HttpGet]
        public ActionResult Get()
        {
            if (!string.IsNullOrWhiteSpace(rootRedirect))
                return RedirectPermanent(rootRedirect);
            return NotFound();
        }
    }
}
