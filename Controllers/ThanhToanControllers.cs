using Microsoft.AspNetCore.Mvc;
using QuanLyKH_TC_API.ADL;
using QuanLyKH_TC_API.Model;

namespace QuanLyKH_TC_API.Controllers
{
    [Route("api/thanh-toan")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly ThanhToanDAL _dal;

        public ThanhToanController(IConfiguration configuration)
        {
            _dal = new ThanhToanDAL(configuration.GetConnectionString("MyDB"));
        }

        [HttpGet("get-all")]
        public IActionResult GetAll() => Ok(_dal.GetAll());

        [HttpGet("get-by-id/{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _dal.GetById(id);
            return item == null ? NotFound("Không tìm thấy thanh toán") : Ok(item);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] ThanhToan model)
        {
            if (model.HoaDonID <= 0)
                return BadRequest("Thiếu HoaDonID");

            return _dal.Create(model)
                ? Ok(new { message = "Thêm thành công" })
                : BadRequest("Thêm thất bại");
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] ThanhToan model)
        {
            if (model.ThanhToanID <= 0)
                return BadRequest("Thiếu ThanhToanID");
            if (model.HoaDonID <= 0)
                return BadRequest("Thiếu HoaDonID");

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
