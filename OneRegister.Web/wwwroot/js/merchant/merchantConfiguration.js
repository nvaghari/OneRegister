if (typeof (merchantConfigObj) === 'undefined') {
    var merchantConfigObj = {
        method: {
            save: function (formid) {
                app.validation.alert.reset();
                var isvalid = app.validation.validateForm(formid);
                if (isvalid) {
                    var fd = new FormData(document.getElementById(formid));
                    $.ajax({
                        url: 'SaveConfig',
                        type: 'Post',
                        data: fd,
                        processData: false,
                        contentType: false,
                        datatype: 'JSON',
                        success: function (data) {
                            if (data.isSuccessful) {
                                app.alert.showSuccess('Configuration was saved successfully');
                            } else {
                                app.alert.showError(data.description);
                            }
                        },
                        error: function (err) {
                            app.alert.showAjaxErrorAutoHide(err);
                        }
                    });
                }
            }
        },
        event: {
            saveBtnClick: function () {
                $('#saveBtn').on('click', function () {
                    merchantConfigObj.method.save('configForm');
                });
            }
        }
    }
}

$(function () {
    merchantConfigObj.event.saveBtnClick();
});