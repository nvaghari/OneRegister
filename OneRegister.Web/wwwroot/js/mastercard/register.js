"use strict";
if (typeof (register) === 'undefined') {
    var register = {
        method: {
            register: function () {
                app.form.submit('mcRegisterForm', 'Register', register.method.registerSuccess)
            },
            registerSuccess: function () {
                app.alert.success.show('good job!');
            },
            showPostAddress: function () {
                $('#postAddressHolder').removeClass('d-none');
            },
            hidePostAddress: function () {
                $('#postAddressHolder').addClass('d-none');
            },
            photo: {
                load: function (e) {
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
                            app.alert.error.show('Image is not valid');
                        }
                    }
                },
                crop: function () {
                    Imager.methods.crop();
                    $('#picModal').modal('hide');
                }
            }
        },
        event: {
            photoFrameClick: function (imageId) {
                $('#picHolder').on('click', function () {
                    $('#' + imageId).trigger('click');
                });
            },
            photoFileChange: function (imageId) {
                $('#' + imageId).on('change', function (e) {
                    register.method.photo.load(e);
                });
            },
            photoCropBtnClick: function () {
                $('#btn-crop').on('click', function () {
                    register.method.photo.crop();
                });
            },
            saveBtn: function () {
                $('#saveBtn').on('click', function () {
                    register.method.register();
                });
            },
            isAddressSameClick: function () {
                $('#IsAddressSame').on('change', function (e) {
                    if (e.target.checked) {
                        register.method.hidePostAddress();
                    } else {
                        register.method.showPostAddress();
                    }
                });
            },
            isAddressSameCheck: function () {
                if ($('#IsAddressSame').checked) {
                    register.method.showPostAddress();
                } else {
                    register.method.hidePostAddress();
                }
            },
            register: function () {
                var e = register.event;

                app.event.customFile();
                app.event.select2();
                app.event.tooltip();

                e.photoFrameClick('FaceDmsFile');
                e.photoFileChange('FaceDmsFile');
                e.photoCropBtnClick();

                e.saveBtn();
                e.isAddressSameClick();
                //e.isAddressSameCheck();
            }
        }
    }
}

$(function () {
    register.event.register();
});