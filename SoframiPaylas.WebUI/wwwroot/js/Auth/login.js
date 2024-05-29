document.getElementById('loginForm').addEventListener('submit', function (event) {
    event.preventDefault();
    let isValid = true;

    isValid = validateField('Email', 'Email alanı gereklidir.', isValid);
    isValid = validateField('Password', 'Password alanı gereklidir.', isValid);

    if (isValid) {
        NProgress.start();
        var formData = new FormData(this);
        fetch('@Url.Action("Login", "Auth")', {
            method: 'POST',
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    NProgress.done();
                    Swal.fire({
                        title: 'Giriş Başarılı!',
                        text: data.message,
                        icon: 'success',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = data.redirectUrl;
                        }
                    });
                } else {
                    NProgress.done();
                    Swal.fire({
                        title: 'Error!',
                        text: data.message || "An error occurred.",
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            })
            .catch(error => {
                NProgress.done();
                console.error('Error:', error);
                alert('Bir hata oluştu, lütfen daha sonra tekrar deneyiniz.');
            });
    }
});


// Alanlara girilen bilgiyi sürekli kontrol etmek
document.querySelectorAll('#loginForm input').forEach(input => {
    input.addEventListener('input', function () {
        var errorSpan = this.nextElementSibling;
        if (this.classList.contains('invalid')) {
            this.classList.remove('invalid');
            errorSpan.textContent = '';
        }
    });
});
function validateField(id, errorMessage, isValid) {
    var input = document.getElementById(id);
    var errorSpan = document.getElementById(id + 'Error');
    if (!input.value.trim()) {
        input.classList.add('invalid');
        errorSpan.textContent = errorMessage;
        return false; // Eğer hata varsa, isValid false olarak döner
    } else {
        input.classList.remove('invalid');
        errorSpan.textContent = '';
        return isValid; // Mevcut isValid değerini döndürür
    }
}