using Microsoft.Data.SqlClient;
using QuanLyContainer_API.Model;
using System.Data;
using System.Text;

namespace QuanLyContainer_API.ADL
{
    public class KhoLTDAL
    {
        private readonly string _connectionString;

        public KhoLTDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ================= GET ALL =================
        public List<KhoLT> GetAll()
        {
            var list = new List<KhoLT>();
            const string sql = "SELECT * FROM KhoLT";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                list.Add(Map(rd));

            return list;
        }

        // ================= GET BY ID =================
        public KhoLT? GetById(int id)
        {
            const string sql = "SELECT * FROM KhoLT WHERE KhoID = @id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Map(rd) : null;
        }

        // ================= SEARCH =================
        public List<KhoLT> Search(string keyword)
        {
            var list = new List<KhoLT>();

            const string sql = @"
                SELECT * FROM KhoLT
                WHERE TenKho LIKE @kw
                   OR DiaChi LIKE @kw
                   OR NhanVienQuanLy LIKE @kw
                   OR CAST(SucChua AS NVARCHAR) LIKE @kw";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@kw", SqlDbType.NVarChar, 200).Value = $"%{keyword}%";

            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                list.Add(Map(rd));

            return list;
        }

        // ================= CREATE =================
        public bool Create(KhoLT model)
        {
            const string sql = @"
                INSERT INTO KhoLT (TenKho, SucChua, DiaChi, NhanVienQuanLy)
                VALUES (@TenKho, @SucChua, @DiaChi, @NhanVienQuanLy)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@TenKho", SqlDbType.NVarChar, 100).Value = model.TenKho;
            cmd.Parameters.Add("@SucChua", SqlDbType.Int).Value = model.SucChua;
            cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 200)
                .Value = (object?)model.DiaChi ?? DBNull.Value;
            cmd.Parameters.Add("@NhanVienQuanLy", SqlDbType.NVarChar, 100)
                .Value = (object?)model.NhanVienQuanLy ?? DBNull.Value;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // ================= UPDATE PARTIAL =================
        public bool UpdatePartial(KhoLT model)
        {
            var sql = new StringBuilder("UPDATE KhoLT SET ");
            var pr = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(model.TenKho))
            {
                sql.Append("TenKho=@TenKho,");
                pr.Add(new SqlParameter("@TenKho", SqlDbType.NVarChar, 100)
                { Value = model.TenKho });
            }

            if (model.SucChua > 0)
            {
                sql.Append("SucChua=@SucChua,");
                pr.Add(new SqlParameter("@SucChua", SqlDbType.Int)
                { Value = model.SucChua });
            }

            if (!string.IsNullOrWhiteSpace(model.DiaChi))
            {
                sql.Append("DiaChi=@DiaChi,");
                pr.Add(new SqlParameter("@DiaChi", SqlDbType.NVarChar, 200)
                { Value = model.DiaChi });
            }

            if (model.NhanVienQuanLy != null)
            {
                sql.Append("NhanVienQuanLy=@NhanVienQuanLy,");
                pr.Add(new SqlParameter("@NhanVienQuanLy", SqlDbType.NVarChar, 100)
                { Value = (object?)model.NhanVienQuanLy ?? DBNull.Value });
            }

            if (pr.Count == 0) return false;

            sql.Length--; // bỏ dấu ,
            sql.Append(" WHERE KhoID=@KhoID");
            pr.Add(new SqlParameter("@KhoID", SqlDbType.Int)
            { Value = model.KhoID });

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.Parameters.AddRange(pr.ToArray());

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // ================= DELETE =================
        public bool Delete(int id)
        {
            const string sql = "DELETE FROM KhoLT WHERE KhoID=@id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // ================= MAP =================
        private static KhoLT Map(SqlDataReader rd)
        {
            return new KhoLT
            {
                KhoID = rd.GetInt32("KhoID"),
                TenKho = rd.GetString("TenKho"),
                SucChua = rd.GetInt32("SucChua"),
                DiaChi = rd.IsDBNull("DiaChi") ? null : rd.GetString("DiaChi"),
                NhanVienQuanLy = rd.IsDBNull("NhanVienQuanLy")
                    ? null
                    : rd.GetString("NhanVienQuanLy")
            };
        }
    }
}
