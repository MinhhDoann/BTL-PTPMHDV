const API_BASE = 'http://localhost:28171/api/loai-hang';

let currentEditId = null;

// ================= LOAD =================
export function loadLoaiHang() {
    fetch(`${API_BASE}/get-all`)
        .then(res => res.json())
        .then(data => renderTable(data))
        .catch(() => alert('Không tải được dữ liệu'));
}

// ================= RENDER =================
function renderTable(items) {
    const tbody = document.querySelector('#itemTypes tbody');
    tbody.innerHTML = '';

    if (!items || items.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5">Chưa có dữ liệu</td></tr>';
        return;
    }

    items.forEach(item => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${item.loaiHangID}</td>
            <td>${item.tenLoai}</td>
            <td>${item.moTa || ''}</td>
            <td>${item.danhMuc || ''}</td>
            <td>
                <button class="edit-btn" data-id="${item.loaiHangID}">Sửa</button>
                <button class="delete-btn" data-id="${item.loaiHangID}">Xóa</button>
            </td>
        `;
        tbody.appendChild(tr);
    });

    document.querySelectorAll('.edit-btn')
        .forEach(b => b.onclick = () => openEditLoaiHang(b.dataset.id));

    document.querySelectorAll('.delete-btn')
        .forEach(b => b.onclick = () => deleteLoaiHang(b.dataset.id));
}

// ================= ADD =================
export function openAddLoaiHang() {
    currentEditId = null;
    document.getElementById('modalTitle').textContent = 'Thêm Loại Hàng';
    document.getElementById('formFields').innerHTML = getFormFields();
    document.getElementById('dynamicModal').style.display = 'block';
}

// ================= EDIT =================
function openEditLoaiHang(id) {
    currentEditId = id;

    fetch(`${API_BASE}/get-by-id/${id}`)
        .then(res => res.json())
        .then(item => {
            document.getElementById('modalTitle').textContent = 'Sửa Loại Hàng';
            document.getElementById('formFields').innerHTML = getFormFields(item);
            document.getElementById('dynamicModal').style.display = 'block';
        });
}

// ================= FORM =================
function getFormFields(data = {}) {
    return `
        <label>Tên loại hàng *</label>
        <input id="tenLoai" value="${data.tenLoai || ''}" required>

        <label>Mô tả</label>
        <textarea id="moTa">${data.moTa || ''}</textarea>

        <label>Danh mục</label>
        <input id="danhMuc" value="${data.danhMuc || ''}">
    `;
}

// ================= DELETE (CHUẨN REST) =================
function deleteLoaiHang(id) {
    if (!confirm('Bạn chắc chắn muốn xóa?')) return;

    fetch(`${API_BASE}/delete/${id}`, {
        method: 'DELETE'
    })
    .then(res => {
        if (!res.ok) throw new Error();
        return res.text();
    })
    .then(msg => {
        alert(msg);
        loadLoaiHang();
    })
    .catch(() => alert('Xóa thất bại'));
}

// ================= SAVE =================
document.getElementById('dynamicForm')?.addEventListener('submit', e => {
    e.preventDefault();

    const payload = {
        tenLoai: document.getElementById('tenLoai').value.trim(),
        moTa: document.getElementById('moTa').value.trim(),
        danhMuc: document.getElementById('danhMuc').value.trim()
    };

    if (!payload.tenLoai) {
        alert('Tên loại không được rỗng');
        return;
    }

    let url = `${API_BASE}/create`;
    let method = 'POST';

    if (currentEditId) {
        payload.loaiHangID = Number(currentEditId);
        url = `${API_BASE}/update`;
        method = 'PUT';
    }

    fetch(url, {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    })
    .then(res => {
        if (!res.ok) throw new Error();
        return res.text();
    })
    .then(msg => {
        alert(msg);
        document.getElementById('dynamicModal').style.display = 'none';
        loadLoaiHang();
    })
    .catch(() => alert('Lưu thất bại'));
});

let searchTimeout = null;

function searchLoaiHang(keyword) {
    if (!keyword.trim()) {
        loadLoaiHang(); // rỗng → load lại tất cả
        return;
    }

    fetch(`${API_BASE}/search?keyword=${encodeURIComponent(keyword)}`)
        .then(res => {
            if (!res.ok) throw new Error('Search lỗi');
            return res.json();
        })
        .then(data => renderTable(data))
        .catch(err => {
            console.error('Lỗi search:', err);
        });
}

document.addEventListener('DOMContentLoaded', () => {
    const searchInput = document.querySelector('#itemTypes .search-input');
    if (!searchInput) return;

    searchInput.addEventListener('input', e => {
        clearTimeout(searchTimeout);

        const keyword = e.target.value;

        searchTimeout = setTimeout(() => {
            searchLoaiHang(keyword);
        }, 300); // debounce 300ms
    });
});
