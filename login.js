document.addEventListener('DOMContentLoaded', function () {
    // DOM elements
    const loginForm = document.getElementById('loginForm');
    const forgotLink = document.getElementById('forgotPasswordLink');
    const registerLink = document.getElementById('registerLink');
    const forgotModal = document.getElementById('forgotModal');
    const registerModal = document.getElementById('registerModal');
    const closeButtons = document.querySelectorAll('.close');
    const sendResetBtn = document.getElementById('sendResetBtn');
    const registerForm = document.getElementById('registerFormInModal');

    // Hàm đóng modal
    function closeModal(modal) {
        modal.style.display = 'none';
    }

    // Mở modal quên mật khẩu
    forgotLink.addEventListener('click', function (e) {
        e.preventDefault();
        forgotModal.style.display = 'block';
    });

    // Mở modal đăng ký
    registerLink.addEventListener('click', function (e) {
        e.preventDefault();
        registerModal.style.display = 'block';
    });

    // Đóng modal khi click nút ×
    closeButtons.forEach(btn => {
        btn.addEventListener('click', function () {
            closeModal(btn.closest('.modal'));
        });
    });

    // Đóng modal khi click ngoài nội dung
    window.addEventListener('click', function (e) {
        if (e.target.classList.contains('modal')) {
            closeModal(e.target);
        }
    });

    // Xử lý GỬI HƯỚNG DẪN (Quên mật khẩu)
    sendResetBtn.addEventListener('click', function () {
        const email = document.getElementById('forgotEmail').value.trim();
        if (!email || !email.includes('@')) {
            alert('Vui lòng nhập email hợp lệ!');
            return;
        }

        // Giả lập kiểm tra email trong "DB"
        const users = JSON.parse(localStorage.getItem('users') || '[]');
        const user = users.find(u => u.email === email);

        if (!user) {
            alert('📧 Email này chưa được đăng ký trong hệ thống.');
            return;
        }

        alert(`✅ Đã gửi hướng dẫn đặt lại mật khẩu đến:\n${email}\n\n💡 Mật khẩu mới tạm thời: ${user.username}123`);
        closeModal(forgotModal);
    });

    //  ĐĂNG KÝ
    registerForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const fullname = document.getElementById('regFullname').value.trim();
        const username = document.getElementById('regUsername').value.trim();
        const email = document.getElementById('regEmail').value.trim();
        const password = document.getElementById('regPassword').value;
        const confirmPassword = document.getElementById('regConfirmPassword').value;

        // Validate
        if (fullname.length < 2) return alert('Họ và tên phải có ít nhất 2 ký tự.');
        if (username.length < 3) return alert('Tên đăng nhập phải có ít nhất 3 ký tự.');
        if (!email.includes('@') || !email.includes('.')) return alert('Email không hợp lệ.');
        if (password.length < 6) return alert('Mật khẩu phải có ít nhất 6 ký tự.');
        if (password !== confirmPassword) return alert('Mật khẩu xác nhận không khớp.');

        // Kiểm tra trùng lặp
        let users = JSON.parse(localStorage.getItem('users') || '[]');
        if (users.some(u => u.username === username)) return alert('Tên đăng nhập đã tồn tại!');
        if (users.some(u => u.email === email)) return alert('Email đã được sử dụng!');

        // Lưu người dùng mới
        users.push({
            username,
            fullname,
            email,
            password, 
            created_at: new Date().toISOString()
        });
        localStorage.setItem('users', JSON.stringify(users));

        alert(`✅ Đăng ký thành công!\nTài khoản: ${username}\nMật khẩu: ${password}`);
        closeModal(registerModal);

        // Tự điền vào form đăng nhập
        document.getElementById('username').value = username;
    });

    // Xử lý ĐĂNG NHẬP
    loginForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const username = document.getElementById('username').value.trim();
        const password = document.getElementById('password').value;
        const remember = document.getElementById('remember').checked;

        if (!username || password.length < 4) {
            alert('Vui lòng nhập tài khoản và mật khẩu hợp lệ!');
            return;
        }

        // Kiểm tra trong "DB" giả lập
        const users = JSON.parse(localStorage.getItem('users') || '[]');
        const user = users.find(u => 
            (u.username === username || u.email === username) && u.password === password
        );

        // Cho phép đăng nhập mặc định: admin / admin123
        const isDefault = (username === 'admin' && password === 'admin123');

        if (!user && !isDefault) {
            alert('❌ Sai tài khoản hoặc mật khẩu!');
            return;
        }

        // Lưu trạng thái
        if (remember) {
            localStorage.setItem('remembered_username', username);
        } else {
            localStorage.removeItem('remembered_username');
        }
        
        sessionStorage.setItem('isLoggedIn', 'true');
        sessionStorage.setItem('username', isDefault ? 'admin' : user.username);

        alert(`✅ Đăng nhập thành công!\nChào mừng, ${isDefault ? 'admin' : user?.fullname || username}!`);
        window.location.href = 'index.html';
    });

    // Tải username đã ghi nhớ
    const savedUsername = localStorage.getItem('remembered_username');
    if (savedUsername) {
        document.getElementById('username').value = savedUsername;
        document.getElementById('remember').checked = true;
    }
});