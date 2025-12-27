
import { loadLoaiHang, openAddLoaiHang } from './loaihang.js';

function showModule(moduleId) {
    document.querySelectorAll('.module-content').forEach(el => el.classList.remove('active'));
    const target = document.getElementById(moduleId);
    if (target) {
        target.classList.add('active');
        if (moduleId === 'itemTypes') {
            loadLoaiHang();
        }
    }
}

function closeModal() {
    document.getElementById('dynamicModal').style.display = 'none';
}

document.addEventListener('DOMContentLoaded', () => {
    // Menu
    document.querySelectorAll('.menu-link').forEach(link => {
        link.addEventListener('click', e => {
            e.preventDefault();
            const moduleId = link.dataset.module;
            if (moduleId) showModule(moduleId);
        });
    });

    // Nút thêm
    document.querySelector('.btn-add-loaihang')?.addEventListener('click', openAddLoaiHang);

    // Đóng modal
    document.querySelector('.close')?.addEventListener('click', closeModal);
    document.getElementById('dynamicModal')?.addEventListener('click', e => {
        if (e.target === e.currentTarget) closeModal();
    });

    // Mở Loại hàng ngay khi load để test
    showModule('itemTypes');
});