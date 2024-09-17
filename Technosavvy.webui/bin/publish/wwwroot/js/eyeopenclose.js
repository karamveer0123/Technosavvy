
$(".eye112.convertPwd").click(function () {
    $(this).children().toggleClass("fa-eye fa-eye-slash");
    input = $(this).parent().find(".changePwd");
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});

let togglePassword2 = document.getElementById('#togglePassword');
let password = document.getElementById('#id_password');
if (togglePassword2 != null) {
    togglePassword2.addEventListener('click', function (e) {
        // toggle the type attribute
        let type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);
        // toggle the eye slash icon
        this.classList.toggle('fa-eye-slash');
    });
}






