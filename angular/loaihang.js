
const API_BASE = "https://localhost:7050/api/LoaiHang";

document.addEventListener("DOMContentLoaded", function () {
    loadLoaiHang();

    // Tìm kiếm realtime
    const searchInput = document.querySelector("#itemTypes .input");
    if (searchInput) {
        searchInput.addEventListener("input", function () {
            const keyword = this.value.toLowerCase();
            document.querySelectorAll("#itemTypes tbody tr").forEach(row => {
                const text = row.textContent.toLowerCaseInsensitive();
                row.style.display = text.includes(keyword) ? "" : "none";
            });
        });
    }
});

// Load toàn bộ loại hàng
function loadLoaiHang() {
    fetch(`${API_BASE}/all`)
        .then(res => {
            if (!res.ok) throw new Error("Không kết nối được server");
            return res.json();
        })
        .then(data => renderLoaiHang(data))
        .catch(err => {
            console.error(err);
            showNotification("Lỗi tải danh sách loại hàng!", "error");
        });
}

// Render bảng
function renderLoaiHang(list) {
    const tbody = document.querySelector("#itemTypes tbody");
    tbody.innerHTML = "";

    if (!list || list.length === 0) {
        tbody.innerHTML = `<tr><td colspan="5" style="text-align:center; color:#888; padding:30px;">Chưa có loại hàng nào</td></tr>`;
        return;
    }

    list.forEach(item => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${item.loaiHangID || '-'}</td>
            <td>${item.tenLoai || ''}</td>
            <td>${item.moTa || '-'}</td>
            <td>${item.tenDanhMuc || 'Không có'}</td>
            <td class="actions">
                <button class="btn-edit" onclick="openEditModal('${item.loaiHangID}', '${escapeHtml(item.tenLoai || '')}', '${escapeHtml(item.moTa || '')}')">
                    Sửa
                </button>
                <button class="btn-delete" onclick="deleteLoaiHang('${item.loaiHangID}')">
                    Xóa
                </button>
            </td>
        `;
        tbody.appendChild(tr);
    });
}

// Mở modal sửa (điền sẵn dữ liệu)
function openEditModal(id, tenLoai, moTa) {
    openModal('edit', 'itemTypes', id);
    document.getElementById("tenLoai").value = tenLoai;
    document.getElementById("moTa").value = moTa;
}

// Xóa loại hàng
function deleteLoaiHang(id) {
    if (!confirm("Xóa loại hàng này?\nCác sản phẩm thuộc loại này có thể bị ảnh hưởng!")) return;

    fetch(`${API_BASE}/${id}`, { method: "DELETE" })
        .then(res => {
            if (!res.ok) throw new Error("Xóa thất bại");
            showNotification("Xóa thành công!");
            loadLoaiHang();
        })
        .catch(err => {
            console.error(err);
            showNotification("Không thể xóa (có thể đang có sản phẩm)", "error");
        });
}

// Xử lý form Thêm + Sửa (modal chung)
document.getElementById("modalForm")?.addEventListener("submit", function (e) {
    e.preventDefault();

    const mode = this.dataset.mode; // "add" hoặc "edit"
    const id = this.dataset.id;

    const tenLoai = document.getElementById("tenLoai").value.trim();
    const moTa = document.getElementById("moTa").value.trim();

    if (!tenLoai) {
        showNotification("Tên loại hàng không được để trống!", "error");
        return;
    }

    const payload = {
        loaiHangID: mode === "edit" ? id : undefined, // chỉ gửi khi sửa
        tenLoai: tenLoai,
        moTa: moTa || null
        // nếu có danhMucID thì thêm vào đây
    };

    const url = mode === "add" ? `${API_BASE}/create` : `${API_BASE}/update`;
    const method = "POST"; // cả create và update đều dùng POST theo API bạn

    fetch(url, {
        method: method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
    })
    .then(res => {
        if (!res.ok) {
            return res.text().then(text => { throw new Error(text || "Lỗi server"); });
        }
        return res.json();
    })
    .then(() => {
        showNotification(mode === "add" ? "Thêm thành công!" : "Cập nhật thành công!");
        closeModal();
        loadLoaiHang();
    })
    .catch(err => {
        console.error(err);
        showNotification(err.message || "Có lỗi xảy ra", "error");
    });
});

// Hàm phụ trợ
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Để tìm kiếm không lỗi font tiếng Việt
String.prototype.toCaseInsensitive = function() {
    return this.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "");
};