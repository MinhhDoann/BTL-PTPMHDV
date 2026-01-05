
import { loadLoaiHang, openAddLoaiHang } from './loaihang.js';
import { loadKhoLT, openAddKhoLT } from './kholt.js';
import { loadContainer, openAddContainer } from './container.js';
import { loadLichSuContainer, openAddLichSu } from './lichsucontainer.js';


function showModule(moduleId) {
    document.querySelectorAll('.module-content').forEach(el => el.classList.remove('active'));
    const target = document.getElementById(moduleId);
    if (!target) return;

    target.classList.add('active');

    switch (moduleId) {
        case 'itemTypes':
            loadLoaiHang();
            break;

        case 'warehouses':
            loadKhoLT();
            break;

        case 'containers':
            loadContainer();
            break;

        case 'containerhistory':
            loadLichSuContainer();
            break;
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

    document.querySelector('.btn-add-loaihang')?.addEventListener('click', openAddLoaiHang);
    document.querySelector('.btn-add-kho')?.addEventListener('click', openAddKhoLT);
    document.querySelector('.btn-add-containers')?.addEventListener('click', openAddContainer);
    document.querySelector('.btn-add-containerhistory')
        ?.addEventListener('click', () => openAddLichSu());

    document.querySelector('.close')?.addEventListener('click', closeModal);
    document.getElementById('dynamicModal')?.addEventListener('click', e => {
        if (e.target === e.currentTarget) closeModal();
    });

    showModule('containers');
});