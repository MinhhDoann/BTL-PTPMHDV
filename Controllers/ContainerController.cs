using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuanLyContainer_API.ADL;
using QuanLyContainer_API.Model;

namespace QuanLyContainer_API.Controllers
{
    [ApiController]
    [Route("api/container")]
    public class ContainerController : ControllerBase
    {
        private readonly ContainerDAL _dal;

        public ContainerController(IConfiguration config)
        {
            var connStr = config.GetConnectionString("MyDB");

            if (string.IsNullOrEmpty(connStr))
                throw new Exception("ConnectionString MyDB bị NULL");

            _dal = new ContainerDAL(connStr);
        }

        // ================= GET ALL =================
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_dal.GetAll());
        }

        // ================= GET BY ID =================
        [HttpGet("get-by-id/{id:int}")]
        public IActionResult GetById(int id)
        {
            var data = _dal.GetById(id);
            return data == null
                ? NotFound(new { message = "Không tìm thấy container" })
                : Ok(data);
        }

        // ================= SEARCH =================
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Ok(_dal.GetAll());

            return Ok(_dal.Search(keyword.Trim()));
        }

        // ================= CREATE =================
        [HttpPost("create")]
        public IActionResult Create([FromBody] Container model)
        {
            ModelState.Remove("ContainerID");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return _dal.Create(model)
                ? Ok(new { message = "Thêm container thành công" })
                : BadRequest(new { message = "Thêm container thất bại" });
        }

        // ================= UPDATE =================
        [HttpPut("update")]
        public IActionResult Update([FromBody] Container model)
        {
            if (model.ContainerID <= 0)
                return BadRequest(new { message = "Thiếu ContainerID" });

            return _dal.UpdatePartial(model)
                ? Ok(new { message = "Cập nhật container thành công" })
                : BadRequest(new { message = "Không có dữ liệu cần cập nhật" });
        }

        // ================= DELETE =================
        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var ok = _dal.Delete(id);
                if (!ok)
                    return NotFound(new { message = "Không tìm thấy container" });

                return Ok(new { message = "Xóa container thành công" });
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return BadRequest(new
                {
                    message = "Không thể xóa vì container đang được sử dụng"
                });
            }
        }
    }
}
