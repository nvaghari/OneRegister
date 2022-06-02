if (typeof (UserRegister) === 'undefined') {
    var UserRegister = {
        method: {
            register: function (formid) {
                app.form.submit(formid, 'Register', UserRegister.method.registerSuccess)
            },
            registerSuccess: function (data) {
                window.location.replace('List');
            }
        },
        event: {
            registerBtn: function () {
                $('#registerBtn').on('click', function () {
                    UserRegister.method.register('userRegisterForm');
                });
            },
            registerAll: function () {
                app.event.tooltip();

                var e = UserRegister.event;

                e.registerBtn();
            }
        }
    }
}

$(function () {
    UserRegister.event.registerAll();
});