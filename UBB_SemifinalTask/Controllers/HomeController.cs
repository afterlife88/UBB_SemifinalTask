using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using HtmlAgilityPack;
using UBB_SemifinalTask.Data;

namespace UBB_SemifinalTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILandingRepository _repository;
        public HomeController() : this(new LandingRepository()) { }
        public HomeController(ILandingRepository repository)
        {
            _repository = repository;
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Theme()
        {
            return View();
        }
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Route("GetPage/{id}")]
        // Caching requests
        [OutputCache(Duration = 60, VaryByParam = "id")]
        public async Task<ActionResult> GetPage(int id)
        {
            var dataPage = await _repository.GetPageAsync(id);
            var moneyData = await _repository.GetMoney(id);
            if (dataPage == null) return HttpNotFound();
            ViewBag.ProjectName = dataPage.ProjectName;
            if (moneyData != null)
            {
                ViewBag.Sum = moneyData.Sum;
                ViewBag.StillNeeded = moneyData.StillNeeded;
            }
            ViewBag.Css = dataPage.Styles;
            ViewBag.Html = dataPage.Content;
            ViewBag.Link = dataPage.LinkOnUbb;
            return View();
        }
    }
}
