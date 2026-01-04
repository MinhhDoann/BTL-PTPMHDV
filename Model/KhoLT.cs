using System.ComponentModel.DataAnnotations;

namespace QuanLyContainer_API.Model
{
    public class KhoLT
    {
        public int KhoID { get; set; }
        public string TenKho { get; set; } = null!;
        public int SucChua { get; set; }     
        public string? DiaChi { get; set; }
        public string? NhanVienQuanLy { get; set; }
    }

}
