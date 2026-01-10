namespace QuanLyKH_TC_API.Model
{
    public class KhachHang
    {
        public int KhachHangID { get; set; }        
        public string TenKH { get; set; } = null!;  
        public string? DiaChi { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
    }
}
