using QuanLyContainer_API.Model;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace QuanLyContainer_API.ADL
{
    public class LoaiHangDAL
    {
        private readonly string _connectionString;

        public LoaiHangDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ================= GET ALL =================
        public List<LoaiHang> GetAll()
        {
            var list = new List<LoaiHang>();
            const string sql = "SELECT * FROM LoaiHang";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(Map(rd));
            }
            return list;
        }

        // ================= GET BY ID =================
        public LoaiHang? GetById(int id)
        {
            const string sql = "SELECT * FROM LoaiHang WHERE LoaiHangID=@id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Map(rd) : null;
        }

        // ================= SEARCH ALL =================
        public List<LoaiHang> Search(string keyword)
        {
            var list = new List<LoaiHang>();

            const string sql = @"
                            SELECT * FROM LoaiHang
                            WHERE TenLoai LIKE @kw
                                OR MoTa LIKE @kw
                                OR DanhMuc LIKE @kw";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@kw", SqlDbType.NVarChar).Value = $"%{keyword}%";

            conn.Open();
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
                list.Add(Map(rd));

            return list;
        }


        public bool Create(LoaiHang model)
        {
            const string sql = @"
                INSERT INTO LoaiHang (TenLoai, MoTa, DanhMuc)
                VALUES (@TenLoai, @MoTa, @DanhMuc)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@TenLoai", SqlDbType.NVarChar, 100).Value = model.TenLoai;
            cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar, 500)
                .Value = (object?)model.MoTa ?? DBNull.Value;
            cmd.Parameters.Add("@DanhMuc", SqlDbType.NVarChar, 50)
                .Value = (object?)model.DanhMuc ?? DBNull.Value;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdatePartial(LoaiHang model)
        {
            var sql = new StringBuilder("UPDATE LoaiHang SET ");
            var pr = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(model.TenLoai))
            {
                sql.Append("TenLoai=@TenLoai,");
                pr.Add(new SqlParameter("@TenLoai", SqlDbType.NVarChar, 100)
                { Value = model.TenLoai });
            }

            if (model.MoTa != null)
            {
                sql.Append("MoTa=@MoTa,");
                pr.Add(new SqlParameter("@MoTa", SqlDbType.NVarChar, 500)
                { Value = (object?)model.MoTa ?? DBNull.Value });
            }

            if (model.DanhMuc != null)
            {
                sql.Append("DanhMuc=@DanhMuc,");
                pr.Add(new SqlParameter("@DanhMuc", SqlDbType.NVarChar, 50)
                { Value = (object?)model.DanhMuc ?? DBNull.Value });
            }

            if (pr.Count == 0) return false;

            sql.Length--; // xóa dấu ,
            sql.Append(" WHERE LoaiHangID=@ID");
            pr.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = model.LoaiHangID });

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.Parameters.AddRange(pr.ToArray());

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM LoaiHang WHERE LoaiHangID=@id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        private static LoaiHang Map(SqlDataReader rd)
        {
            return new LoaiHang
            {
                LoaiHangID = rd.GetInt32("LoaiHangID"),
                TenLoai = rd.GetString("TenLoai"),
                MoTa = rd.IsDBNull("MoTa") ? null : rd.GetString("MoTa"),
                DanhMuc = rd.IsDBNull("DanhMuc") ? null : rd.GetString("DanhMuc")
            };
        }
    }
}
