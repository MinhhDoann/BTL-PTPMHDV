using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuanLyContainer_API.ADL;
using QuanLyContainer_API.Model;

namespace QuanLyContainer_API.Controllers
{
    [ApiController]
    [Route("api/kho-lt")]
    public class KhoLTController : ControllerBase
    {
        private readonly KhoLTDAL _dal;

        public KhoLTController(IConfiguration config)
        {
            _dal = new KhoLTDAL(config.GetConnectionString("MyDB"));
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
            return data == null
                ? NotFound(new { message = "Không tìm thấy kho" })
                : Ok(data);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Ok(_dal.GetAll());

            return Ok(_dal.Search(keyword.Trim()));
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] KhoLT model)
        {
            ModelState.Remove("KhoID");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return _dal.Create(model)
                ? Ok(new { message = "Thêm kho thành công" })
                : BadRequest(new { message = "Thêm kho thất bại" });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] KhoLT model)
        {
            if (model.KhoID <= 0)
                return BadRequest(new { message = "Thiếu KhoID" });

            return _dal.UpdatePartial(model)
                ? Ok(new { message = "Cập nhật kho thành công" })
                : BadRequest(new { message = "Không có dữ liệu cần cập nhật" });
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var ok = _dal.Delete(id);
                if (!ok)
                    return NotFound(new { message = "Không tìm thấy kho" });

                return Ok(new { message = "Xóa kho thành công" });
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return BadRequest(new
                {
                    message = "Không thể xóa vì kho đang được sử dụng trong Container"
                });
            }
        }
    }
}
