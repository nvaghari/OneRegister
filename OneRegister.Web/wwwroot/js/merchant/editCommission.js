if (typeof (editCommission) === 'undefined') {
    var editCommission = {
        method: {
            save: function () {
                var isTermsAccepted = document.getElementById('commissionCheck').checked;
                if (isTermsAccepted) {
                    app.form.submit("merchantCommissionForm", "EditCommission", editCommission.method.saveSuccess);
                } else {
                    app.alert.showWarning("you need to confirm and check that you read our agreement at the end of the page");
                }
            },
            saveSuccess: function (data) {
                window.location = app.properties.BaseURL + 'Merchant/List'
            }
        },
        event: {
            registerAll: function () {
                var e = editCommission.event;

                e.saveBtn();
            },
            saveBtn: function () {
                $('#saveBtn').on('click', function () {
                    editCommission.method.save();
                });
            }
        }
    }
}

$(function () {
    editCommission.event.registerAll();
});