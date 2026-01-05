using System.ComponentModel.DataAnnotations;

namespace QuanLyContainer_API.Model
{
    public class LichSuContainer
    {
        public int LichSuID { get; set; }

        [Required]
        public int ContainerID { get; set; }

        [Required]
        public DateTime ThoiGian { get; set; }

        [Required]
        public string HoatDong { get; set; }

        public string ViTri { get; set; }
    }
}
