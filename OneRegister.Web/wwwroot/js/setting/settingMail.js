if (typeof (settingMail) === 'undefined') {
    var settingMail = {
        method: {
            save: function () {
                app.form.submit('mailForm', 'SaveEmail', settingMail.private.saveSuccess);
            },
            send: function () {
                app.form.submit('mailForm', 'SendEmail', settingMail.private.sendSuccess);
            }
        },
        private: {
            saveSuccess: function (data) {
                app.alert.showSuccessAutoHide("Email settings was saved successfully");
            },
            sendSuccess: function (data) {
                app.alert.showSuccessAutoHide("Email was sent successfully");
            }
        },
        event: {
            saveBtn: function () {
                $('#saveBtn').on('click', function () {
                    settingMail.method.save();
                });
            },
            testBtn: function () {
                $('#testBtn').on('click', function () {
                    settingMail.method.send();
                });
            },
            registerAll: function () {
                var e = settingMail.event;

                e.saveBtn();
                e.testBtn();
            }
        }
    }
}

$(function () {
    settingMail.event.registerAll();
});

