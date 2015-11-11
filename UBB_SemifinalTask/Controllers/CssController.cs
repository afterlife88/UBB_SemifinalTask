using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using UBB_SemifinalTask.Data;
using UBB_SemifinalTask.Models;

namespace UBB_SemifinalTask.Controllers
{
    [Authorize]
    [RoutePrefix("api/css")]
    public class CssController : ApiController
    {
        private readonly ILandingRepository _repository;
        public CssController() : this(new LandingRepository()) { }
        public CssController(ILandingRepository repository)
        {
            _repository = repository;
        }
        [Route("getStyle")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCss()
        {
            HttpRequestHeaders headers = Request.Headers;
            string authorId = headers.GetValues("AuthorId").First();

            var items = await _repository.GetStylesAsync(authorId);
            if (items!=null)
                return Ok(items);
            return NotFound();

        }
        [Route("setStyle")]
        [HttpPost]
        public async Task<IHttpActionResult> SetCss([FromBody]LandingResources resources)
        {
            var item = await _repository.AddStylesAsync(resources);
            var returnItem = new { Id = item.Id };
            return Ok(returnItem);
        }
        [Route("setPage")]
        [HttpPost]
        public async Task<IHttpActionResult> SetPage([FromBody]LandingResources resources)
        {
            var item = await _repository.AddContentAsync(resources);

            var returnItem = new {Id = item.Id};
            return Ok(returnItem);
        }
    }
}
