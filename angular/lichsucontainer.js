const API_LICHSU = 'http://localhost:28171/api/lich-su-container';

let currentContainerId = null;  // ID container đang xem (nếu có)
let currentEditId = null;       // ID lịch sử đang sửa (nếu có)
let searchTimeout = null;

// ================= LOAD =================
export function loadLichSuContainer() {
    fetch(`${API_LICHSU}/get-all`)
        .then(res => res.json())
        .then(data => renderTable(data))
        .catch(() => alert('Không tải được lịch sử container'));
}

export function loadLichSuByContainer(containerId) {
    currentContainerId = containerId;

    fetch(`${API_LICHSU}/get-by-container/${containerId}`)
        .then(res => res.json())
        .then(data => renderTable(data))
        .catch(() => alert('Không tải được lịch sử theo container'));
}

// ================= RENDER =================
function renderTable(items) {
    const tbody = document.querySelector('#containerhistory table tbody');
    if (!tbody) return;

    tbody.innerHTML = '';

    if (!items || items.length === 0) {
        tbody.innerHTML = '<tr><td colspan="6">Chưa có dữ liệu</td></tr>';
        return;
    }

    items.forEach(item => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${item.lichSuID}</td>
            <td>${item.containerID}</td>
            <td>${item.hoatDong}</td>
            <td>${formatDate(item.thoiGian)}</td>
            <td>${item.viTri || ''}</td>
            <td>
                <button class="edit-btn" data-id="${item.lichSuID}" disabled>Sửa</button>
                <button class="delete-btn" data-id="${item.lichSuID}">Xóa</button>
            </td>
        `;
        tbody.appendChild(tr);
    });
}

// ================= MODAL ADD/EDIT =================
export function openAddLichSu(containerId) {
    currentEditId = null;
    currentContainerId = containerId || null;

    document.getElementById('modalTitle').textContent = 'Thêm lịch sử container';
    document.getElementById('formFields').innerHTML = getFormFields();
    document.getElementById('dynamicModal').style.display = 'block';
}

// Đã disable nút Sửa nên không cần hàm openEditLichSu nữa
// Nhưng giữ lại để tránh lỗi nếu có gọi nhầm (có thể xóa sau)
function openEditLichSu(id) {
    alert('Chức năng sửa đã bị vô hiệu hóa.');
}

// Tạo form fields động
function getFormFields(data = {}) {
    const containerValue = data.containerID || currentContainerId || '';
    const isReadonly = (currentContainerId || currentEditId) ? 'readonly' : '';

    return `
        <label>Container ID *</label>
        <input id="containerID" type="number" value="${containerValue}" ${isReadonly} required>

        <label>Thời gian *</label>
        <input id="thoiGian" type="datetime-local" 
               value="${data.thoiGian ? new Date(data.thoiGian).toISOString().slice(0,16) : new Date().toISOString().slice(0,16)}"
               required>

        <label>Hoạt động *</label>
        <input id="hoatDong" value="${data.hoatDong || ''}" required>

        <label>Vị trí</label>
        <input id="viTri" value="${data.viTri || ''}">
    `;
}

// ================= DELETE =================
function deleteLichSu(id) {
    if (!confirm('Bạn chắc chắn muốn xóa lịch sử này?')) return;

    fetch(`${API_LICHSU}/delete/${id}`, { method: 'DELETE' })
        .then(res => {
            if (!res.ok) throw new Error();
            return res.text();
        })
        .then(msg => {
            alert(msg || 'Xóa thành công');
            currentContainerId
                ? loadLichSuByContainer(currentContainerId)
                : loadLichSuContainer();
        })
        .catch(() => alert('Xóa thất bại'));
}

// ================= SUBMIT FORM (CHỈ THÊM MỚI - KHÔNG CÓ SỬA) =================
document.getElementById('dynamicForm')?.addEventListener('submit', e => {
    e.preventDefault();

    const containerIDInput = document.getElementById('containerID');
    if (!containerIDInput || !containerIDInput.value) {
        alert('Container ID không được để trống');
        return;
    }

    const payload = {
        containerID: Number(containerIDInput.value),
        thoiGian: document.getElementById('thoiGian').value 
                   ? new Date(document.getElementById('thoiGian').value + ':00Z').toISOString()
                   : new Date().toISOString(),
        hoatDong: document.getElementById('hoatDong').value.trim(),
        viTri: document.getElementById('viTri').value.trim() || null
    };

    if (!payload.hoatDong) {
        alert('Hoạt động không được để trống');
        return;
    }

    // Chỉ dùng create (POST) - không có update nữa vì không cho sửa
    const url = `${API_LICHSU}/create`;
    const method = 'POST';

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
        alert(msg || 'Thêm thành công');
        document.getElementById('dynamicModal').style.display = 'none';
        currentContainerId
            ? loadLichSuByContainer(currentContainerId)
            : loadLichSuContainer();
    })
    .catch(err => {
        console.error(err);
        alert('Thêm thất bại');
    });
});

// ================= SEARCH =================
function searchLichSu(keyword) {
    if (!keyword.trim()) {
        currentContainerId
            ? loadLichSuByContainer(currentContainerId)
            : loadLichSuContainer();
        return;
    }

    fetch(`${API_LICHSU}/search?keyword=${encodeURIComponent(keyword)}`)
        .then(res => {
            if (!res.ok) throw new Error('Search lỗi');
            return res.json();
        })
        .then(data => renderTable(data))
        .catch(err => {
            console.error('Lỗi search:', err);
            alert('Tìm kiếm thất bại');
        });
}

document.addEventListener('DOMContentLoaded', () => {
    const searchInput = document.querySelector('#containerhistory .search-input');
    if (searchInput) {
        searchInput.addEventListener('input', e => {
            clearTimeout(searchTimeout);
            const keyword = e.target.value;
            searchTimeout = setTimeout(() => searchLichSu(keyword), 300);
        });
    }

    const tbody = document.querySelector('#containerhistory table tbody');
    if (tbody) {
        tbody.addEventListener('click', e => {
            const deleteBtn = e.target.closest('.delete-btn');
            if (deleteBtn) {
                deleteLichSu(deleteBtn.dataset.id);
            }

            const editBtn = e.target.closest('.edit-btn');
            if (editBtn) {
                alert('Chức năng sửa tạm thời bị vô hiệu hóa.');
            }
        });
    }
});

// ================= UTILS =================
function formatDate(date) {
    return new Date(date).toLocaleString('vi-VN');
}

// Đóng modal
document.querySelector('.close-modal')?.addEventListener('click', () => {
    document.getElementById('dynamicModal').style.display = 'none';
});

window.addEventListener('click', e => {
    const modal = document.getElementById('dynamicModal');
    if (e.target === modal) {
        modal.style.display = 'none';
    }
});