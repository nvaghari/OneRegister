if (typeof (checkAccount) === 'undefined') {
    var checkAccount = {
        method: {
            checkAccount: function () {
                app.form.submit('mainForm', 'CheckAccount', checkAccount.method.checkAccountAjaxSuccess)
            },
            checkAccountAjaxSuccess: function (data) {
                app.alert.showSuccess('woooo!');
            }
        },
        event: {
            checkBtnClick: function () {
                $('#checkBtn').on('click', function () {
                    checkAccount.method.checkAccount();
                })
            },
            register: function () {
                var e = checkAccount.event;

                e.checkBtnClick();
            }
        }
    }
}

$(function () {
    checkAccount.event.register();
});