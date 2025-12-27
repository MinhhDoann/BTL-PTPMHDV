using Microsoft.AspNetCore.Mvc;
using QuanLyContainer_API.Model;
using QuanLyContainer_API.ADL;

namespace QuanLyContainer_API.Controllers
{
    [Route("api/loai-hang")]
    [ApiController]
    public class LoaiHangController : ControllerBase
    {
        private readonly LoaiHangDAL _dal;

        public LoaiHangController(IConfiguration configuration)
        {
            _dal = new LoaiHangDAL(configuration.GetConnectionString("MyDB"));
        }


        [HttpGet("get-all")]
        public IActionResult GetAllLoaiHang()
        {
            return Ok(_dal.GetAll());
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetLoaiHangById(string id)
        {
            var item = _dal.GetById(id);
            return item == null ? NotFound("Không tìm thấy loại hàng") : Ok(item);
        }


        [HttpPost("create")]
        public IActionResult CreateLoaiHang([FromBody] LoaiHang model)
        {
            if (string.IsNullOrWhiteSpace(model.TenLoai))
                return BadRequest("Tên loại không được rỗng");

            return _dal.Create(model)
                ? Ok(new { message = "Thêm thành công" })
                : BadRequest("Thêm thất bại");
        }


        [HttpPut("update")]
        public IActionResult UpdateLoaiHang([FromBody] LoaiHang model)
        {
            if (string.IsNullOrWhiteSpace(model.LoaiHangID))
            {
                return BadRequest("Thiếu LoaiHangID");
            }

            bool success = _dal.UpdatePartial(model);

            if (success)
            {
                return Ok(new { message = "Sửa thành công" });
            }

            return BadRequest("Không có dữ liệu cần sửa hoặc sửa thất bại");
        }



        [HttpDelete("delete/{id}")]
        public IActionResult DeleteLoaiHang(string id)
        {
            return _dal.Delete(id)
                ? Ok(new { message = "Xóa thành công" })
                : BadRequest("Xóa thất bại");
        }
    }
}
