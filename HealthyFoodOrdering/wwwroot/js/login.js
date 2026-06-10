document.addEventListener('DOMContentLoaded', function () {
    // ================= KHAI BÁO CÁC SECTION (KHỐI FORM) =================
    const loginSection = document.getElementById('loginSection');
    const registerSection = document.getElementById('registerSection');
    const forgotSection = document.getElementById('forgotSection');

    // ================= KHAI BÁO CÁC NÚT CHUYỂN ĐỔI GIAO DIỆN =================
    const toRegisterLink = document.getElementById('toRegister');
    const toLoginLink = document.getElementById('toLogin');
    const toForgotLink = document.getElementById('toForgot');
    const forgotToLoginLink = document.getElementById('forgotToLogin');

    // ================= BIẾN KIỂM TRA INPUT & ẨN HIỆN MK =================
    const loginForm = document.getElementById('loginForm');
    const emailInput = document.getElementById('email');
    const passwordInput = document.getElementById('password');
    const togglePassword = document.getElementById('togglePassword');

    const registerForm = document.getElementById('registerForm');
    const regNameInput = document.getElementById('regName');
    const regEmailInput = document.getElementById('regEmail');
    const regPasswordInput = document.getElementById('regPassword');
    const toggleRegPassword = document.getElementById('toggleRegPassword');

    const forgotForm = document.getElementById('forgotForm');
    const forgotEmailInput = document.getElementById('forgotEmail');

    // -----------------------------------------------------------------
    // LOGIC 1: Ẩn/Hiện mật khẩu 
    // -----------------------------------------------------------------
    togglePassword.addEventListener('click', function () {
        const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordInput.setAttribute('type', type);
        this.classList.toggle('fa-eye');
        this.classList.toggle('fa-eye-slash');
    });

    toggleRegPassword.addEventListener('click', function () {
        const type = regPasswordInput.getAttribute('type') === 'password' ? 'text' : 'password';
        regPasswordInput.setAttribute('type', type);
        this.classList.toggle('fa-eye');
        this.classList.toggle('fa-eye-slash');
    });

    // -----------------------------------------------------------------
    // LOGIC 2: Điều hướng chuyển đổi giữa 3 Form (MỚI CẬP NHẬT)
    // -----------------------------------------------------------------
    // Đăng nhập -> Đăng ký
    toRegisterLink.addEventListener('click', function (e) {
        e.preventDefault();
        loginSection.classList.add('hidden');
        registerSection.classList.remove('hidden');
    });

    // Đăng ký -> Đăng nhập
    toLoginLink.addEventListener('click', function (e) {
        e.preventDefault();
        registerSection.classList.add('hidden');
        loginSection.classList.remove('hidden');
    });

    // Đăng nhập -> Quên mật khẩu (MỚI)
    toForgotLink.addEventListener('click', function (e) {
        e.preventDefault();
        loginSection.classList.add('hidden');
        forgotSection.classList.remove('hidden');
    });

    // Quên mật khẩu -> Đăng nhập (MỚI)
    forgotToLoginLink.addEventListener('click', function (e) {
        e.preventDefault();
        forgotSection.classList.add('hidden');
        loginSection.classList.remove('hidden');
    });

    // -----------------------------------------------------------------
    // LOGIC 3: Validate Form Đăng Nhập
    // -----------------------------------------------------------------
    loginForm.addEventListener('submit', function (e) {
        e.preventDefault(); 
        const emailError = document.getElementById('emailError');
        const passwordError = document.getElementById('passwordError');
        emailError.textContent = '';
        passwordError.textContent = '';

        let isValid = true;
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!emailRegex.test(emailInput.value.trim())) {
            emailError.textContent = 'Email không đúng định dạng hợp lệ.';
            isValid = false;
        }
        if (passwordInput.value.length < 6) {
            passwordError.textContent = 'Mật khẩu phải có ít nhất 6 ký tự.';
            isValid = false;
        }

        if (isValid) {
            alert('Đăng nhập thành công!');
        }
    });

    // -----------------------------------------------------------------
    // LOGIC 4: Validate Form Đăng Ký
    // -----------------------------------------------------------------
    registerForm.addEventListener('submit', function (e) {
        e.preventDefault();
        const regNameError = document.getElementById('regNameError');
        const regEmailError = document.getElementById('regEmailError');
        const regPasswordError = document.getElementById('regPasswordError');

        regNameError.textContent = '';
        regEmailError.textContent = '';
        regPasswordError.textContent = '';

        let isRegValid = true;

        if (regNameInput.value.trim() === '') {
            regNameError.textContent = 'Vui lòng nhập họ và tên của bạn.';
            isRegValid = false;
        }
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(regEmailInput.value.trim())) {
            regEmailError.textContent = 'Email không đúng định dạng hợp lệ.';
            isRegValid = false;
        }
        if (regPasswordInput.value.length < 6) {
            regPasswordError.textContent = 'Mật khẩu đăng ký phải có ít nhất 6 ký tự.';
            isRegValid = false;
        }

        if (isRegValid) {
            alert('Đăng ký tài khoản thành công! Tiến hành chuyển về trang đăng nhập.');
            registerSection.classList.add('hidden');
            loginSection.classList.remove('hidden');
            registerForm.reset();
        }
    });

    // -----------------------------------------------------------------
    // LOGIC 5: Validate Form Quên Mật Khẩu (MỚI THÊM)
    // -----------------------------------------------------------------
    forgotForm.addEventListener('submit', function (e) {
        e.preventDefault();
        const forgotEmailError = document.getElementById('forgotEmailError');
        forgotEmailError.textContent = '';

        let isForgotValid = true;
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        // Kiểm tra xem định dạng email có đúng không
        if (!emailRegex.test(forgotEmailInput.value.trim())) {
            forgotEmailError.textContent = 'Email không đúng định dạng hợp lệ.';
            isForgotValid = false;
        }

        if (isForgotValid) {
            alert('Hệ thống đã gửi mã xác nhận khôi phục đến email: ' + forgotEmailInput.value);
            console.log({
                action: 'ForgotPassword',
                email: forgotEmailInput.value
            });
            
            // Xử lý xong tự quay lại form đăng nhập
            forgotSection.classList.add('hidden');
            loginSection.classList.remove('hidden');
            forgotForm.reset();
        }
    });
});