using Microsoft.AspNetCore.Mvc;
using QuanLyContainer_API.ADL;
using QuanLyContainer_API.DAL;
using QuanLyContainer_API.Model;

namespace QuanLyContainer_API.Controllers
{
    [ApiController]
    [Route("api/lich-su-container")]
    public class LichSuContainerController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly LichSuContainerDAL _dal;

        public LichSuContainerController(IConfiguration config)
        {
            _config = config;
            _dal = new LichSuContainerDAL(
                config.GetConnectionString("MyDB"));
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_dal.GetAll());
        }

        [HttpGet("container/{containerId:int}")]
        public IActionResult GetByContainer(int containerId)
        {
            return Ok(_dal.GetByContainer(containerId));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Ok(_dal.GetAll());

            return Ok(_dal.Search(keyword.Trim()));
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] LichSuContainer model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_dal.Insert(model, out string error))
            {
                string trangThai = MapTrangThai(model.HoatDong);

                var containerDAL = new ContainerDAL(
                    _config.GetConnectionString("MyDB"));

                containerDAL.UpdateTrangThai(model.ContainerID, trangThai);

                return Ok(new { message = "Thêm lịch sử container thành công" });
            }

            return BadRequest(new { message = error });
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            return _dal.Delete(id)
                ? Ok(new { message = "Xóa lịch sử thành công" })
                : NotFound(new { message = "Không tìm thấy lịch sử" });
        }

        private string MapTrangThai(string hoatDong)
        {
            return hoatDong switch
            {
                "Nhập container" => "Rỗng",
                "Đóng hàng" => "Đã đóng hàng",
                "Xuất kho" => "Đang vận chuyển",
                "Giao hàng" => "Đã giao",
                "Kiểm tra container" => "Cần bảo trì",
                _ => "Rỗng"
            };
        }
    }
}
