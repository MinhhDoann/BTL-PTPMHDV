// 🧩 Module: UI & Navigation
function showModule(id) {
    document.querySelectorAll('.module-content').forEach(m => m.classList.remove('active'));
    const target = document.getElementById(id);
    if (target) {
        target.classList.add('active');
    } else {
        console.warn(`Module "${id}" không tồn tại.`);
    }
}

// Mở modal thêm 
function openModal(mode, module) {
    alert(`Mở modal: ${mode} cho module "${module}"`);
}

// Module: Authentication Check
function initializeAuthUI() {
    const loginLi = document.getElementById('login');
    const logoutLi = document.getElementById('outlogin');
    const logoutLink = document.getElementById('logout-link');

    if (!loginLi || !logoutLi || !logoutLink) {
        console.warn('Thiếu phần tử auth trong DOM — có thể không ở trang index.html');
        return;
    }

    const isLoggedIn = sessionStorage.getItem('isLoggedIn') === 'true';
    const username = sessionStorage.getItem('username');

    if (isLoggedIn && username) {
        loginLi.style.display = 'none';
        logoutLi.style.display = 'block';

        if (!logoutLink.dataset.listenerAttached) {
            logoutLink.addEventListener('click', function (e) {
                e.preventDefault();
                if (confirm(`Bạn có chắc muốn đăng xuất khỏi tài khoản "${username}"?`)) {
                    sessionStorage.removeItem('isLoggedIn');
                    sessionStorage.removeItem('username');
                    window.location.href = 'login.html';
                }
            });
            logoutLink.dataset.listenerAttached = 'true'; 
        }
    } else {
        loginLi.style.display = 'block';
        logoutLi.style.display = 'none';
    }
}

// Module: Search
function searchEmployees() {
    const query = document.getElementById('employeeSearch')?.value?.trim().toLowerCase();
    if (!query) {
        return;
    }
    console.log('Tìm nhân viên:', query);
}

// Khởi chạy khi DOM sẵn sàng
document.addEventListener('DOMContentLoaded', function () {
    initializeAuthUI();

    if (!document.querySelector('.module-content.active')) {
        showModule('containers');
    }

    document.querySelectorAll('.menu-toggle').forEach(toggle => {
        toggle.addEventListener('click', function () {
            this.nextElementSibling.classList.toggle('show');
        });
    });
});