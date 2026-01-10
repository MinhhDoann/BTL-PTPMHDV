const API_BASE = 'http://localhost:28171/api/container';
const API_LOAIHANG = 'http://localhost:28171/api/loai-hang/get-all';
const API_KHO = 'http://localhost:28171/api/kho-lt/get-all'; 

let currentEditId = null;
let loaiHangMap = {};
let khoMap = {};

export async function loadContainer() {
    try {
        await loadLoaiHangMap();
        await loadKhoMap();

        const res = await fetch(`${API_BASE}/get-all`);
        const data = await res.json();
        renderTable(data);
    } catch {
        alert('Không tải được dữ liệu');
    }
}

async function loadLoaiHangMap() {
    const res = await fetch(API_LOAIHANG);
    const data = await res.json();
    loaiHangMap = {};
    data.forEach(x => loaiHangMap[x.loaiHangID] = x.tenLoai);
}

async function loadKhoMap() {
    try {
        const res = await fetch(API_KHO);
        const data = await res.json();
        khoMap = {};
        data.forEach(x => khoMap[x.khoID] = x.tenKho);
    } catch {
        khoMap = {};
    }
}

function renderTable(items) {
    const tbody = document.querySelector('#containers tbody');
    tbody.innerHTML = '';

    if (!items || items.length === 0) {
        tbody.innerHTML = '<tr><td colspan="9">Chưa có dữ liệu</td></tr>';
        return;
    }

    items.forEach(item => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${item.containerID}</td>
            <td>${item.hopDongID}</td>
            <td>${loaiHangMap[item.loaiHangID] || item.loaiHangID}</td>
            <td>${item.trongLuong ?? ''}</td>
            <td>${item.trangThai ?? ''}</td>
            <td>${khoMap[item.khoID] || item.khoID || ''}</td>
            <td>${item.phuongTienID ?? ''}</td>
            <td>${item.chuyenDiID ?? ''}</td>
            <td>
                <button class="edit-btn" data-id="${item.containerID}">Sửa</button>
                <button class="delete-btn" data-id="${item.containerID}">Xóa</button>
            </td>
        `;
        tbody.appendChild(tr);
    });

    document.querySelectorAll('.edit-btn')
        .forEach(b => b.onclick = () => openEditContainer(b.dataset.id));

    document.querySelectorAll('.delete-btn')
        .forEach(b => b.onclick = () => deleteContainer(b.dataset.id));
}

export function openAddContainer() {
    currentEditId = null;
    document.getElementById('modalTitle').textContent = 'Thêm Container';
    document.getElementById('formFields').innerHTML = getFormFields();
    document.getElementById('dynamicModal').style.display = 'block';

    loadLoaiHangOptions();
    loadKhoOptions();
}

function openEditContainer(id) {
    currentEditId = id;

    fetch(`${API_BASE}/get-by-id/${id}`)
        .then(res => res.json())
        .then(item => {
            document.getElementById('modalTitle').textContent = 'Sửa Container';
            document.getElementById('formFields').innerHTML = getFormFields(item);
            document.getElementById('dynamicModal').style.display = 'block';

            loadLoaiHangOptions(item.loaiHangID);
            loadKhoOptions(item.khoID);
        });
}

// ================= FORM =================
function getFormFields(data = {}) {
    return `
        <label>Hợp đồng ID *</label>
        <input type="number" id="hopDongID" value="${data.hopDongID ?? ''}" required>

        <label>Loại hàng *</label>
        <select id="loaiHangID"></select>

        <label>Trọng lượng</label>
        <input type="number" step="0.01" id="trongLuong" value="${data.trongLuong ?? ''}">

        <label>Trạng thái</label>
        <select id="trangThai">
            ${[
                'Rỗng',
                'Đã đóng hàng',
                'Đang vận chuyển',
                'Cần bảo trì',
                'Đã Giao'
            ].map(status => `
                <option value="${status}"
                    ${(data.trangThai ?? 'Rỗng') === status ? 'selected' : ''}>
                    ${status}
                </option>
            `).join('')}
        </select>        

        <label>Kho</label>
        <select id="khoID"></select>

        <label>Phương tiện ID</label>
        <input type="number" id="phuongTienID" value="${data.phuongTienID ?? ''}">

        <label>Chuyến đi ID</label>
        <input type="number" id="chuyenDiID" value="${data.chuyenDiID ?? ''}">
    `;
}

function loadLoaiHangOptions(selectedId = null) {
    fetch(API_LOAIHANG)
        .then(res => res.json())
        .then(data => {
            const select = document.getElementById('loaiHangID');
            select.innerHTML = '<option value="">-- Chọn loại hàng --</option>';
            data.forEach(item => {
                select.innerHTML += `
                    <option value="${item.loaiHangID}"
                        ${item.loaiHangID === selectedId ? 'selected' : ''}>
                        ${item.tenLoai}
                    </option>
                `;
            });
        });
}

function loadKhoOptions(selectedId = null) {
    fetch(API_KHO)
        .then(res => res.json())
        .then(data => {
            const select = document.getElementById('khoID');
            select.innerHTML = '<option value="">-- Chọn kho --</option>';
            data.forEach(item => {
                select.innerHTML += `
                    <option value="${item.khoID}"
                        ${item.khoID === selectedId ? 'selected' : ''}>
                        ${item.tenKho}
                    </option>
                `;
            });
        });
}

function deleteContainer(id) {
    if (!confirm('Bạn chắc chắn muốn xóa?')) return;

    fetch(`${API_BASE}/delete/${id}`, { method: 'DELETE' })
        .then(res => res.ok ? res.text() : Promise.reject())
        .then(msg => {
            alert(msg);
            loadContainer();
        })
        .catch(() => alert('Xóa thất bại'));
}

document.getElementById('dynamicForm')?.addEventListener('submit', e => {
    e.preventDefault();

    const payload = {
        hopDongID: Number(hopDongID.value),
        loaiHangID: Number(loaiHangID.value),
        trongLuong: trongLuong.value ? Number(trongLuong.value) : null,
        trangThai: trangThai.value || null,
        khoID: khoID.value ? Number(khoID.value) : null,
        phuongTienID: phuongTienID.value ? Number(phuongTienID.value) : null,
        chuyenDiID: chuyenDiID.value ? Number(chuyenDiID.value) : null
    };

    if (!payload.hopDongID || !payload.loaiHangID) {
        alert('Thiếu dữ liệu bắt buộc');
        return;
    }

    let url = `${API_BASE}/create`;
    let method = 'POST';

    if (currentEditId) {
        payload.containerID = Number(currentEditId);
        url = `${API_BASE}/update`;
        method = 'PUT';
    }

    fetch(url, {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    })
    .then(res => res.ok ? res.text() : Promise.reject())
    .then(msg => {
        alert(msg);
        document.getElementById('dynamicModal').style.display = 'none';
        loadContainer();
    })
    .catch(() => alert('Lưu thất bại'));
});

let searchTimeout = null;

function searchContainer(keyword) {
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
    const searchInput = document.querySelector('#containers .search-input');
    if (!searchInput) return;

    searchInput.addEventListener('input', e => {
        clearTimeout(searchTimeout);

        const keyword = e.target.value;

        searchTimeout = setTimeout(() => {
            searchContainer(keyword);
        }, 300); // debounce 300ms
    });
});