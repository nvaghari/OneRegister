"use strict"
if (typeof (MerchantUserRegister) === 'undefined') {
    var MerchantUserRegister = {
        method: {
            register: function (formid) {
                app.form.submit(formid, 'UserRegister', MerchantUserRegister.method.registerSuccess)
            },
            registerSuccess: function (data) {
                window.location.replace(app.properties.BaseURL +  'Account/Login');
            }
        },
        event: {
            registerBtn: function () {
                $('#registerBtn').on('click', function () {
                    MerchantUserRegister.method.register('userRegisterForm');
                });
            },
            registerAll: function () {
                app.event.tooltip();

                var e = MerchantUserRegister.event;

                e.registerBtn();
            }
        }
    }
}
document.addEventListener("DOMContentLoaded", function () {
    MerchantUserRegister.event.registerAll();
});