var userProfileObj = {
    method: {
        getUserInfo: function () {

        },
        changePassword: function (userId, oldPass, newPass, confirmPass) {
            $.ajax({
                url: 'ChangePassword',
                type: 'POST',
                dataType: 'JSON',
                data: { 'userId': userId, 'oldPassword': oldPass, 'newPassword': newPass, 'confirm': confirmPass},
                success: function (data) {
                    if (data.isSuccessful == true) {
                        app.alert.showSuccess("password is changed");
                    } else {
                        app.alert.showError(data.description);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxError(err);
                }
            });
        }
    },
    event: {
        registerChangePasswordButton: function () {
            $('#changePasswordBtn').on('click', function () {
                var userId = $('#userId').val();
                var oldPass = $('#oldPassword').val();
                var newPass = $('#newPassword').val()
                var confirmPass = $('#confirmPassword').val();

                userProfileObj.method.changePassword(userId, oldPass, newPass, confirmPass);
            });
        }
    }
};

$(function () {
    userProfileObj.event.registerChangePasswordButton();
});