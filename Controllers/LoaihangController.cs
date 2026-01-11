using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuanLyContainer_API.ADL;
using QuanLyContainer_API.Model;

namespace QuanLyContainer_API.Controllers
{
    [ApiController]
    [Route("api/loai-hang")]
    public class LoaiHangController : ControllerBase
    {
        private readonly LoaiHangDAL _dal;

        public LoaiHangController(IConfiguration config)
        {
            _dal = new LoaiHangDAL(config.GetConnectionString("MyDB"));
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_dal.GetAll());
        }

        [HttpGet("get-by-id/{id:int}")]
        public IActionResult GetById(int id)
        {
            var data = _dal.GetById(id);
            return data == null ? NotFound("Không tìm thấy") : Ok(data);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Ok(_dal.GetAll());

            return Ok(_dal.Search(keyword.Trim()));
        }

        [HttpPost("create")]
        public IActionResult Create(LoaiHang model)
        {
            ModelState.Remove("LoaiHangID");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return _dal.Create(model)
                ? Ok("Thêm thành công")
                : BadRequest("Thêm thất bại");
        }

        [HttpPut("update")]
        public IActionResult Update(LoaiHang model)
        {
            if (model.LoaiHangID <= 0)
                return BadRequest("Thiếu ID");

            return _dal.UpdatePartial(model)
                ? Ok("Cập nhật thành công")
                : BadRequest("Không có dữ liệu cần cập nhật");
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var ok = _dal.Delete(id);
                if (!ok)
                    return BadRequest(new { message = "Không tìm thấy loại hàng" });

                return Ok(new { message = "Xóa thành công" });
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return BadRequest(new
                {
                    message = "Không thể xóa vì loại hàng đang được sử dụng trong Container"
                });
            }
        }

    }
}
