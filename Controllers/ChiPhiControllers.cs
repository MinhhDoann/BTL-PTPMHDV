using Microsoft.AspNetCore.Mvc;
using QuanLyKH_TC_API.ADL;
using QuanLyKH_TC_API.Model;

namespace QuanLyKH_TC_API.Controllers
{
    [Route("api/chi-phi")]
    [ApiController]
    public class ChiPhiController : ControllerBase
    {
        private readonly ChiPhiDAL _dal;

        public ChiPhiController(IConfiguration configuration)
        {
            _dal = new ChiPhiDAL(configuration.GetConnectionString("MyDB"));
        }

        [HttpGet("get-all")]
        public IActionResult GetAll() => Ok(_dal.GetAll());

        [HttpGet("get-by-id/{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _dal.GetById(id);
            return item == null ? NotFound("Không tìm thấy chi phí") : Ok(item);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] ChiPhi model)
        {
            if (string.IsNullOrWhiteSpace(model.LoaiChiPhi))
                return BadRequest("Loại chi phí không được rỗng");

            return _dal.Create(model)
                ? Ok(new { message = "Thêm thành công" })
                : BadRequest("Thêm thất bại");
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] ChiPhi model)
        {
            if (model.ChiPhiID <= 0)
                return BadRequest("Thiếu ChiPhiID");
            if (string.IsNullOrWhiteSpace(model.LoaiChiPhi))
                return BadRequest("Loại chi phí không được rỗng");

            return _dal.Update(model)
                ? Ok(new { message = "Sửa thành công" })
                : BadRequest("Sửa thất bại");
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            return _dal.Delete(id)
                ? Ok(new { message = "Xóa thành công" })
                : BadRequest("Xóa thất bại");
        }
    }
}
