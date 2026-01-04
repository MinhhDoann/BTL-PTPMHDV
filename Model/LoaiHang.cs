using System.ComponentModel.DataAnnotations;

namespace QuanLyContainer_API.Model
{
    public class LoaiHang
    {
        public int LoaiHangID { get; set; }

        [Required(ErrorMessage = "Tên loại không được rỗng")]
        [StringLength(100)]
        public string TenLoai { get; set; } = string.Empty;

        [StringLength(500)]
        public string? MoTa { get; set; }

        [StringLength(50)]
        public string? DanhMuc { get; set; }
    }
}
