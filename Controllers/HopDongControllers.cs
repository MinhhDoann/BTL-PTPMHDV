using Microsoft.AspNetCore.Mvc;
using QuanLyKH_TC_API.ADL;
using QuanLyKH_TC_API.Model;

namespace QuanLyKH_TC_API.Controllers
{
    [Route("api/hop-dong")]
    [ApiController]
    public class HopDongController : ControllerBase
    {
        private readonly HopDongDAL _dal;

        public HopDongController(IConfiguration configuration)
        {
            _dal = new HopDongDAL(configuration.GetConnectionString("MyDB"));
        }

        [HttpGet("get-all")]
        public IActionResult GetAll() => Ok(_dal.GetAll());

        [HttpGet("get-by-id/{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _dal.GetById(id);
            return item == null ? NotFound("Không tìm thấy hợp đồng") : Ok(item);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HopDong model)
        {
            if (model.KhachHangID <= 0)
                return BadRequest("Thiếu KhachHangID");
            if (model.NgayKy == default)
                return BadRequest("Ngày ký không hợp lệ");

            return _dal.Create(model)
                ? Ok(new { message = "Thêm thành công" })
                : BadRequest("Thêm thất bại");
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] HopDong model)
        {
            if (model.HopDongID <= 0)
                return BadRequest("Thiếu HopDongID");
            if (model.KhachHangID <= 0)
                return BadRequest("Thiếu KhachHangID");
            if (model.NgayKy == default)
                return BadRequest("Ngày ký không hợp lệ");

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
