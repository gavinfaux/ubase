using System.Web.Mvc;
using Application.Core.Services;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Application.Web.Controllers.Hijacks
{
    public class HomePageController : RenderMvcController
    {
        private readonly ICmsService _cmsService;

        public HomePageController(ICmsService cmsService)
        {
            _cmsService = cmsService;
        }

        public override ActionResult Index(ContentModel model)
        {
            // Do some stuff here, then return the base method
            return base.Index(model);
        }
    }
}