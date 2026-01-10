using Microsoft.AspNetCore.Mvc;
using QuanLyKH_TC_API.ADL;
using QuanLyKH_TC_API.Model;

namespace QuanLyKH_TC_API.Controllers
{
    [Route("api/khach-hang")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly KhachHangDAL _dal;

        public KhachHangController(IConfiguration configuration)
        {
            _dal = new KhachHangDAL(configuration.GetConnectionString("MyDB"));
        }

        [HttpGet("get-all")]
        public IActionResult GetAll() => Ok(_dal.GetAll());

        [HttpGet("get-by-id/{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _dal.GetById(id);
            return item == null ? NotFound("Không tìm thấy khách hàng") : Ok(item);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] KhachHang model)
        {
            if (string.IsNullOrWhiteSpace(model.TenKH))
                return BadRequest("Tên khách hàng không được rỗng");

            return _dal.Create(model)
                ? Ok(new { message = "Thêm thành công" })
                : BadRequest("Thêm thất bại");
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] KhachHang model)
        {
            if (model.KhachHangID <= 0)
                return BadRequest("Thiếu KhachHangID");
            if (string.IsNullOrWhiteSpace(model.TenKH))
                return BadRequest("Tên khách hàng không được rỗng");

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
