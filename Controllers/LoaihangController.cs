using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyContainer_API.Model;
using QuanLyContainer_API.ADL;

namespace QuanLyContainer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiHangController : ControllerBase
    {
        private readonly LoaiHangDAL _dal;

        public LoaiHangController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MyDB");
            _dal = new LoaiHangDAL(connectionString);
        }

        [HttpGet]
        public ActionResult<IEnumerable<LoaiHang>> GetAll()
        {
            return Ok(_dal.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<LoaiHang> GetById(string id)
        {
            var item = _dal.GetById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] LoaiHang model)
        {
            if (_dal.Create(model))
                return Ok(new { message = "Thêm thành công" });
            return BadRequest("Thêm thất bại");
        }

        [HttpPut]
        public IActionResult Update([FromBody] LoaiHang model)
        {
            if (string.IsNullOrWhiteSpace(model.LoaiHangID))
                return BadRequest("Thiếu LoaiHangID");

            var old = _dal.GetById(model.LoaiHangID);
            if (old == null)
                return NotFound("Không tìm thấy loại hàng");

            old.TenLoai =
                string.IsNullOrWhiteSpace(model.TenLoai) || model.TenLoai == "string"
                ? old.TenLoai
                : model.TenLoai;

            old.MoTa =
                string.IsNullOrWhiteSpace(model.MoTa) || model.MoTa == "string"
                ? old.MoTa
                : model.MoTa;

            if (_dal.Update(old))
                return Ok(new { message = "Sửa thành công" });

            return BadRequest("Sửa thất bại");
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_dal.Delete(id))
                return Ok(new { message = "Xóa thành công" });
            return BadRequest("Xóa thất bại");
        }
    }
}
