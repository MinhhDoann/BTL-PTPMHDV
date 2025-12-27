// js/loaihang.js
const API_BASE = 'http://localhost:28171/api/loai-hang';

let currentEditId = null;

export function loadLoaiHang() {
    fetch(`${API_BASE}/get-all`)
        .then(res => {
            if (!res.ok) throw new Error(`HTTP ${res.status}`);
            return res.json();
        })
        .then(data => renderTable(data))
        .catch(err => {
            console.error('Lỗi load loại hàng:', err);
            alert('Không tải được dữ liệu. Kiểm tra API đang chạy chưa?');
        });
}

function renderTable(items) {
    const tbody = document.querySelector('#itemTypes tbody');
    tbody.innerHTML = '<tr><td colspan="5">Đang tải...</td></tr>';

    if (!items || items.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5">Chưa có dữ liệu</td></tr>';
        return;
    }

    tbody.innerHTML = '';
    items.forEach(item => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${item.loaiHangID || ''}</td>
            <td>${item.tenLoai || ''}</td>
            <td>${item.moTa || ''}</td>
            <td>${item.danhMuc || ''}</td>
            <td>
                <button class="edit-btn" data-id="${item.loaiHangID}">Sửa</button>
                <button class="delete-btn" data-id="${item.loaiHangID}">Xóa</button>
            </td>
        `;
        tbody.appendChild(tr);
    });

    // Gắn sự kiện sửa/xóa
    document.querySelectorAll('.edit-btn').forEach(btn => {
        btn.addEventListener('click', () => openEditLoaiHang(btn.dataset.id));
    });
    document.querySelectorAll('.delete-btn').forEach(btn => {
        btn.addEventListener('click', () => deleteLoaiHang(btn.dataset.id));
    });
}

export function openAddLoaiHang() {
    currentEditId = null;
    document.getElementById('modalTitle').textContent = 'Thêm Loại Hàng';
    document.getElementById('formFields').innerHTML = getFormFields();
    document.getElementById('dynamicModal').style.display = 'block';
}

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

function getFormFields(data = {}) {
    return `
        <label>Tên loại hàng: <span style="color:red">*</span></label>
        <input type="text" id="tenLoai" value="${data.tenLoai || ''}" required placeholder="Bắt buộc nhập">
        
        <label>Mô tả:</label>
        <textarea id="moTa">${data.moTa || ''}</textarea>
        
        <label>Danh mục:</label>
        <input type="text" id="danhMuc" value="${data.danhMuc || ''}">
    `;
}

// Lưu dữ liệu
document.getElementById('dynamicForm')?.addEventListener('submit', function(e) {
    e.preventDefault();

    const tenLoai = document.getElementById('tenLoai').value.trim();
    if (!tenLoai) {
        alert('Vui lòng nhập Tên loại hàng!');
        return;
    }

    const payload = {
        tenLoai: tenLoai,
        moTa: document.getElementById('moTa').value.trim(),
        danhMuc: document.getElementById('danhMuc').value.trim()
    };
    if (currentEditId) payload.loaiHangID = currentEditId;

    const url = currentEditId ? `${API_BASE}/update` : `${API_BASE}/create`;
    const method = currentEditId ? 'PUT' : 'POST';

    fetch(url, {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    })
    .then(res => {
        if (!res.ok) {
            return res.text().then(text => { throw new Error(text || 'Lỗi server'); });
        }
        return res.json();
    })
    .then(result => {
        alert(result.message || 'Thao tác thành công!');
        document.getElementById('dynamicModal').style.display = 'none';
        loadLoaiHang();
    })
    .catch(err => {
        console.error('Lỗi:', err);
        alert(err.message || 'Lỗi khi lưu dữ liệu. Vui lòng kiểm tra lại.');
    });
});