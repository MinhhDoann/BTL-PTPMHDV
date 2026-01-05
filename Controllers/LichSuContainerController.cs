using Microsoft.AspNetCore.Mvc;
using QuanLyContainer_API.DAL;
using QuanLyContainer_API.Model;

namespace QuanLyContainer_API.Controllers
{
    [ApiController]
    [Route("api/lich-su-container")]
    public class LichSuContainerController : ControllerBase
    {
        private readonly LichSuContainerDAL _dal;

        public LichSuContainerController(IConfiguration config)
        {
            _dal = new LichSuContainerDAL(
                config.GetConnectionString("MyDB"));
        }

        [HttpGet("container/{containerId:int}")]
        public IActionResult GetByContainer(int containerId)
        {
            return Ok(_dal.GetByContainer(containerId));
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_dal.GetAll());
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Ok(_dal.GetAll());

            return Ok(_dal.Search(keyword.Trim()));
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] LichSuContainer model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_dal.Insert(model, out string error))
                return Ok(new { message = "Thêm lịch sử container thành công" });

            return BadRequest(new { message = error });
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            return _dal.Delete(id)
                ? Ok(new { message = "Xóa lịch sử thành công" })
                : NotFound(new { message = "Không tìm thấy lịch sử" });
        }
    }
}
