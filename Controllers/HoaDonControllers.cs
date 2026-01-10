using Microsoft.AspNetCore.Mvc;
using QuanLyKH_TC_API.ADL;
using QuanLyKH_TC_API.Model;

namespace QuanLyKH_TC_API.Controllers
{
    [Route("api/hoa-don")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly HoaDonDAL _dal;

        public HoaDonController(IConfiguration configuration)
        {
            _dal = new HoaDonDAL(configuration.GetConnectionString("MyDB"));
        }

        [HttpGet("get-all")]
        public IActionResult GetAll() => Ok(_dal.GetAll());

        [HttpGet("get-by-id/{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _dal.GetById(id);
            return item == null ? NotFound("Không tìm thấy hóa đơn") : Ok(item);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HoaDon model)
        {
            if (model.HopDongID <= 0)
                return BadRequest("Thiếu HopDongID");
            if (model.PhanTramDaThanhToan < 0 || model.PhanTramDaThanhToan > 100)
                return BadRequest("Phần trăm đã thanh toán phải từ 0-100");

            return _dal.Create(model)
                ? Ok(new { message = "Thêm thành công" })
                : BadRequest("Thêm thất bại");
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] HoaDon model)
        {
            if (model.HoaDonID <= 0)
                return BadRequest("Thiếu HoaDonID");
            if (model.HopDongID <= 0)
                return BadRequest("Thiếu HopDongID");
            if (model.PhanTramDaThanhToan < 0 || model.PhanTramDaThanhToan > 100)
                return BadRequest("Phần trăm đã thanh toán phải từ 0-100");

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
