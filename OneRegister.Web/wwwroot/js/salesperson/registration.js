"use strict"
if (typeof (registration) === 'undefined') {
    var registration = {
        method: {
            saveForm: function () {
                app.form.submit('registrationForm', 'Register', registration.method.saveFormSuccess)
            },
            saveFormSuccess: function (data) {

                app.utils.redirectTo('Merchant/EditCommission?mid=' + data.id);
            },
            checkBusinessName: function () {
                var businessNo = $('#BusinessNo').val();
                if (businessNo.trim()) {
                    var data = { 'businessNo': businessNo.trim() };
                    app.ajax.post('GetBusinessName', data, registration.method.checkBusinessNameSuccess, registration.method.checkBusinessNameFailure);
                }
            },
            checkBusinessNameSuccess: function (data) {
                if (data.isSuccessful) {
                    if (!$('#BusinessName').val()) {
                        $('#BusinessName').val(data.id);
                    }
                }
            },
            checkBusinessNameFailure: function (err) {
                console.log(err);
            }
        },
        event: {
            register: function () {
                app.event.select2();

                registration.event.saveBtn();
                registration.event.businessNumberCheck();
            },
            saveBtn: function () {
                $('#saveFormBtn').on('click', function () {
                    registration.method.saveForm();
                });
            },
            businessNumberCheck: function () {
                $('#BusinessNo').on('blur', function () {
                    registration.method.checkBusinessName();
                });
            }
        }
    }
}

$(function () {
    registration.event.register();
});