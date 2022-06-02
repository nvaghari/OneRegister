if (typeof (agpRegisterObj) === "undefined") {
    var agpRegisterObj = {
        config: {
            imageFileId: 'Photo'
        },
        method: {
            registerForm: function (formId) {
                var isValid = app.validation.validateForm(formId);
                if (isValid) {
                    var formData = new FormData(document.getElementById(formId));
                    if (Imager.properties.photoC) {
                        formData.append("PhotoC", Imager.properties.photoC, Imager.properties.photoName);
                    }
                    if (Imager.properties.photoT) {
                        formData.append("PhotoT", Imager.properties.photoT, Imager.properties.photoName);
                    }
                    formData.delete("Photo");
                    $.ajax({
                        url: 'Register',
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: agpRegisterObj.private.registerAjaxSuccess,
                        error: agpRegisterObj.private.registerAjaxError
                    });
                }
            }
        },
        events: {
            register: function () {
                app.event.customFile();
                app.event.select2();
                app.event.tooltip();
                var e = agpRegisterObj.events;
                e.registerSaveBtn();
                e.photoFrameClick();
                e.photoFileChange();
                e.photoCropBtnClick();
            },
            registerSaveBtn: function () {
                $('#saveBtn').on('click', function () {
                    agpRegisterObj.method.registerForm('agroRegisterForm');
                });
            },
            photoFrameClick: function () {
                $('#picHolder').on('click', function () {
                    $('#' + agpRegisterObj.config.imageFileId).trigger('click');
                });
            },
            photoFileChange: function () {
                $('#' + agpRegisterObj.config.imageFileId).on('change', function (e) {
                    var reader = new FileReader();
                    reader.onload = function (event) {
                        $('#picModal').modal('show');
                        Imager.methods.initCropper(event.target.result);
                    }
                    var file = e.target.files[0];
                    if (file) {
                        if (Imager.methods.isValid(file)) {
                            Imager.properties.photoName = file.name;
                            reader.readAsDataURL(e.target.files[0]);
                        } else {
                            app.alert.showErrorAutoHide('Image is not valid');
                        }
                    }
                });
            },
            photoCropBtnClick: function () {
                $('#btn-crop').on('click', function () {
                    Imager.methods.crop();
                    $('#picModal').modal('hide');
                });
            }
        },
        private: {
            registerAjaxSuccess: function (data) {
                if (data.isSuccessful) {
                    window.location.href = "List"
                } else {
                    app.validation.checkUnsuccessfulResponse(data);
                }
            },
            registerAjaxError: function (err) {
                app.alert.showError(err.statusText);
            }
        }
    }
}

$(function () {
    agpRegisterObj.events.register();
});