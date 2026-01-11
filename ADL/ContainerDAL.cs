using Microsoft.Data.SqlClient;
using QuanLyContainer_API.Model;
using System.Data;
using System.Text;

namespace QuanLyContainer_API.ADL
{
    public class ContainerDAL
    {
        private readonly string _connectionString;

        public ContainerDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Container> GetAll()
        {
            var list = new List<Container>();
            const string sql = "SELECT * FROM Container";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                list.Add(Map(rd));

            return list;
        }

        public Container? GetById(int id)
        {
            const string sql = "SELECT * FROM Container WHERE ContainerID=@id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Map(rd) : null;
        }

        public List<Container> Search(string keyword)
        {
            var list = new List<Container>();

            const string sql = @"
                SELECT * FROM Container
                WHERE TrangThai LIKE @kw
                   OR CAST(ContainerID AS NVARCHAR) LIKE @kw
                   OR CAST(HopDongID AS NVARCHAR) LIKE @kw
                   OR CAST(LoaiHangID AS NVARCHAR) LIKE @kw
                   OR CAST(KhoID AS NVARCHAR) LIKE @kw";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@kw", SqlDbType.NVarChar, 100)
                .Value = $"%{keyword}%";

            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                list.Add(Map(rd));

            return list;
        }

        public bool Create(Container model)
        {
            const string sql = @"
                INSERT INTO Container
                (HopDongID, LoaiHangID, TrongLuong, TrangThai, KhoID, PhuongTienID, ChuyenDiID)
                VALUES
                (@HopDongID, @LoaiHangID, @TrongLuong, @TrangThai, @KhoID, @PhuongTienID, @ChuyenDiID)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@HopDongID", SqlDbType.Int).Value = model.HopDongID;
            cmd.Parameters.Add("@LoaiHangID", SqlDbType.Int).Value = model.LoaiHangID;
            cmd.Parameters.Add("@TrongLuong", SqlDbType.Decimal)
                .Value = (object?)model.TrongLuong ?? DBNull.Value;
            cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50)
                .Value = (object?)model.TrangThai ?? DBNull.Value;
            cmd.Parameters.Add("@KhoID", SqlDbType.Int)
                .Value = (object?)model.KhoID ?? DBNull.Value;
            cmd.Parameters.Add("@PhuongTienID", SqlDbType.Int)
                .Value = (object?)model.PhuongTienID ?? DBNull.Value;
            cmd.Parameters.Add("@ChuyenDiID", SqlDbType.Int)
                .Value = (object?)model.ChuyenDiID ?? DBNull.Value;

            conn.Open();
                return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdatePartial(Container model)
        {
            var sql = new StringBuilder("UPDATE Container SET ");
            var pr = new List<SqlParameter>();

            if (model.HopDongID > 0)
            {
                sql.Append("HopDongID=@HopDongID,");
                pr.Add(new SqlParameter("@HopDongID", SqlDbType.Int)
                { Value = model.HopDongID });
            }

            if (model.LoaiHangID > 0)
            {
                sql.Append("LoaiHangID=@LoaiHangID,");
                pr.Add(new SqlParameter("@LoaiHangID", SqlDbType.Int)
                { Value = model.LoaiHangID });
            }

            if (model.TrongLuong.HasValue)
            {
                sql.Append("TrongLuong=@TrongLuong,");
                pr.Add(new SqlParameter("@TrongLuong", SqlDbType.Decimal)
                { Value = model.TrongLuong.Value });
            }

            if (!string.IsNullOrWhiteSpace(model.TrangThai))
            {
                sql.Append("TrangThai=@TrangThai,");
                pr.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50)
                { Value = model.TrangThai });
            }

            if (model.KhoID.HasValue)
            {
                sql.Append("KhoID=@KhoID,");
                pr.Add(new SqlParameter("@KhoID", SqlDbType.Int)
                { Value = model.KhoID.Value });
            }

            if (model.PhuongTienID.HasValue)
            {
                sql.Append("PhuongTienID=@PhuongTienID,");
                pr.Add(new SqlParameter("@PhuongTienID", SqlDbType.Int)
                { Value = model.PhuongTienID.Value });
            }

            if (model.ChuyenDiID.HasValue)
            {
                sql.Append("ChuyenDiID=@ChuyenDiID,");
                pr.Add(new SqlParameter("@ChuyenDiID", SqlDbType.Int)
                { Value = model.ChuyenDiID.Value });
            }

            if (pr.Count == 0) return false;

            sql.Length--; // bỏ dấu ,
            sql.Append(" WHERE ContainerID=@ContainerID");
            pr.Add(new SqlParameter("@ContainerID", SqlDbType.Int)
            { Value = model.ContainerID });

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.Parameters.AddRange(pr.ToArray());

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Container WHERE ContainerID=@id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // ================= MAP =================
        private static Container Map(SqlDataReader rd)
        {
            return new Container
            {
                ContainerID = rd.GetInt32("ContainerID"),
                HopDongID = rd.GetInt32("HopDongID"),
                LoaiHangID = rd.GetInt32("LoaiHangID"),
                TrongLuong = rd.IsDBNull("TrongLuong") ? null : rd.GetDecimal("TrongLuong"),
                TrangThai = rd.IsDBNull("TrangThai") ? null : rd.GetString("TrangThai"),
                KhoID = rd.IsDBNull("KhoID") ? null : rd.GetInt32("KhoID"),
                PhuongTienID = rd.IsDBNull("PhuongTienID") ? null : rd.GetInt32("PhuongTienID"),
                ChuyenDiID = rd.IsDBNull("ChuyenDiID") ? null : rd.GetInt32("ChuyenDiID")
            };
        }
        public bool UpdateTrangThai(int containerId, string trangThai)
        {
            const string sql = @"
        UPDATE Container
        SET TrangThai = @TrangThai
        WHERE ContainerID = @ContainerID";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = trangThai;
            cmd.Parameters.Add("@ContainerID", SqlDbType.Int).Value = containerId;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
