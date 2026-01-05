using Microsoft.Data.SqlClient;
using QuanLyContainer_API.Model;
using System.Data;

namespace QuanLyContainer_API.DAL
{
    public class LichSuContainerDAL
    {
        private readonly string _cs;

        public LichSuContainerDAL(string connectionString)
        {
            _cs = connectionString;
        }

        private bool ContainerExists(int containerId)
        {
            const string sql = "SELECT COUNT(*) FROM Container WHERE ContainerID=@id";

            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = containerId;

            conn.Open();
            return (int)cmd.ExecuteScalar() > 0;
        }
        public List<LichSuContainer> GetAll()
        {
            var list = new List<LichSuContainer>();

            const string sql = @"
                SELECT *
                FROM LichSuContainer
                ORDER BY ThoiGian DESC";

            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(Map(rd));
            }

            return list;
        }
        public List<LichSuContainer> GetByContainer(int containerId)
        {
            var list = new List<LichSuContainer>();
            const string sql = @"
                SELECT * FROM LichSuContainer
                WHERE ContainerID=@id
                ORDER BY ThoiGian DESC";

            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = containerId;

            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(Map(rd));
            }
            return list;
        }
        public List<LichSuContainer> Search(string keyword)
        {
            var list = new List<LichSuContainer>();

            const string sql = @"
        SELECT * FROM LichSuContainer
        WHERE HoatDong LIKE @kw
           OR ViTri LIKE @kw
           OR CAST(ContainerID AS NVARCHAR) LIKE @kw
        ORDER BY ThoiGian DESC";

            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@kw", SqlDbType.NVarChar, 200).Value = $"%{keyword}%";

            conn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(Map(rd));
            }

            return list;
        }

        public bool Insert(LichSuContainer model, out string error)
        {
            error = "";

            if (!ContainerExists(model.ContainerID))
            {
                error = "Container không tồn tại";
                return false;
            }

            const string sql = @"
                INSERT INTO LichSuContainer(ContainerID, ThoiGian, HoatDong, ViTri)
                VALUES(@ContainerID,@ThoiGian,@HoatDong,@ViTri)";

            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@ContainerID", SqlDbType.Int).Value = model.ContainerID;
            cmd.Parameters.Add("@ThoiGian", SqlDbType.DateTime).Value = model.ThoiGian;
            cmd.Parameters.Add("@HoatDong", SqlDbType.NVarChar, 50).Value = model.HoatDong;
            cmd.Parameters.Add("@ViTri", SqlDbType.NVarChar, 100).Value =
                (object?)model.ViTri ?? DBNull.Value;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM LichSuContainer WHERE LichSuID=@id";

            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        private LichSuContainer Map(SqlDataReader r)
        {
            return new LichSuContainer
            {
                LichSuID = r.GetInt32(r.GetOrdinal("LichSuID")),
                ContainerID = r.GetInt32(r.GetOrdinal("ContainerID")),
                ThoiGian = r.GetDateTime(r.GetOrdinal("ThoiGian")),
                HoatDong = r.GetString(r.GetOrdinal("HoatDong")),
                ViTri = r["ViTri"] == DBNull.Value ? null : r.GetString(r.GetOrdinal("ViTri"))
            };
        }
    }
}
