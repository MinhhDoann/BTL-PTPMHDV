const API_BASE = 'http://localhost:28171/api/kho-lt';

let currentEditId = null;

// ================= LOAD =================
export function loadKhoLT() {
    fetch(`${API_BASE}/get-all`)
        .then(res => res.json())
        .then(data => renderTable(data))
        .catch(() => alert('Không tải được dữ liệu'));
}

// ================= RENDER =================
function renderTable(items) {
    const tbody = document.querySelector('#warehouses tbody');
    tbody.innerHTML = '';

    if (!items || items.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5">Chưa có dữ liệu</td></tr>';
        return;
    }

    items.forEach(item => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${item.khoID}</td>
            <td>${item.tenKho}</td>
            <td>${item.sucChua}</td>
            <td>${item.diaChi || ''}</td>
            <td>${item.nhanVienQuanLy || ''}</td>
            <td>
                <button class="edit-btn" data-id="${item.khoID}">Sửa</button>
                <button class="delete-btn" data-id="${item.khoID}">Xóa</button>
            </td>
        `;
        tbody.appendChild(tr);
    });    

    document.querySelectorAll('.edit-btn')
        .forEach(b => b.onclick = () => openEditKhoLT(b.dataset.id));

    document.querySelectorAll('.delete-btn')
        .forEach(b => b.onclick = () => deleteKhoLT(b.dataset.id));
}

// ================= ADD =================
export function openAddKhoLT() {
    currentEditId = null;
    document.getElementById('modalTitle').textContent = 'Thêm Kho';
    document.getElementById('formFields').innerHTML = getFormFields();
    document.getElementById('dynamicModal').style.display = 'block';
}

// ================= EDIT =================
function openEditKhoLT(id) {
    currentEditId = id;

    fetch(`${API_BASE}/get-by-id/${id}`)
        .then(res => res.json())
        .then(item => {
            document.getElementById('modalTitle').textContent = 'Sửa Kho';
            document.getElementById('formFields').innerHTML = getFormFields(item);
            document.getElementById('dynamicModal').style.display = 'block';
        });
}

// ================= FORM =================
function getFormFields(data = {}) {
    return `
        <label>Tên kho *</label>
        <input id="TenKho" value="${data.tenKho || ''}" required>

        <label>Sức chứa *</label>
        <input type="number" id="SucChua" min="1"
               value="${data.sucChua || ''}" required>

        <label>Địa chỉ</label>
        <input id="DiaChi" value="${data.diaChi || ''}">

        <label>Nhân viên quản lý</label>
        <input id="NhanVienQuanLy" value="${data.nhanVienQuanLy || ''}">
    `;
}

// ================= DELETE (CHUẨN REST) =================
function deleteKhoLT(id) {
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
        loadKhoLT();
    })
    .catch(() => alert('Xóa thất bại'));
}

// ================= SAVE =================
document.getElementById('dynamicForm')?.addEventListener('submit', e => {
    e.preventDefault();

    const payload = {
        TenKho: document.getElementById('TenKho').value.trim(),
        SucChua: Number(document.getElementById('SucChua').value),
        DiaChi: document.getElementById('DiaChi').value.trim() || null,
        NhanVienQuanLy: document.getElementById('NhanVienQuanLy').value.trim() || null
    };

    if (!payload.TenKho) {
        alert('Tên kho không được rỗng');
        return;
    }
    
    if (!Number.isInteger(payload.SucChua) || payload.SucChua <= 0) {
        alert('Sức chứa phải là số > 0');
        return;
    }

    let url = `${API_BASE}/create`;
    let method = 'POST';

    if (currentEditId) {
        payload.KhoID = Number(currentEditId);
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
        loadKhoLT();
    })
    .catch(() => alert('Lưu thất bại'));
});

let searchTimeout = null;

function searchKhoLT(keyword) {
    if (!keyword.trim()) {
        loadKhoLT(); // rỗng → load lại tất cả
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
    const searchInput = document.querySelector('#warehouses .search-input');
    if (!searchInput) return;

    searchInput.addEventListener('input', e => {
        clearTimeout(searchTimeout);

        const keyword = e.target.value;

        searchTimeout = setTimeout(() => {
            searchKhoLT(keyword);
        }, 300); // debounce 300ms
    });
});
