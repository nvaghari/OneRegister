var studentRegisterObj = {
    crp: {},
    config: {
        imageFileId: 'Photo',
        schoolSelectId: 'SchoolId',
        yearSelectId: 'Year',
        classSelectId: 'ClassId',
        homeRootSelectId: 'HomeRoomId'
    },
    method: {
        getClasses: function (schoolid, year) {
            $.ajax({
                url: 'GetClasses',
                type: 'POST',
                data: { "schoolid": schoolid, "year": year },
                success: function (data) {
                    app.utils.refreshSelect(studentRegisterObj.config.classSelectId, data.classes);
                    app.utils.refreshSelect(studentRegisterObj.config.homeRootSelectId, data.homeRooms);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        },
        getYearValue: function () {
            return document.getElementById(studentRegisterObj.config.yearSelectId).value;
        },
        getSchoolValue: function () {
            return document.getElementById(studentRegisterObj.config.schoolSelectId).value;
        },
        registerForm: function (formId) {
            app.validation.alert.reset();
            var formData = new FormData(document.getElementById(formId));
            if (Imager.properties.photoC) {
                formData.delete("Photo");
                formData.append("Photo", Imager.properties.photoC, Imager.properties.photoName);
            }
            if (Imager.properties.photoT) {
                formData.append("Thumbnail", Imager.properties.photoT, Imager.properties.photoName);
            }
            $.ajax({
                url: 'Register',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: studentRegisterObj.private.registerAjaxSuccess,
                error: studentRegisterObj.private.registerAjaxError
            });
        }
    },
    events: {
        register: function () {
            app.event.customFile();
            app.event.select2();
            app.event.tooltip();
            var e = studentRegisterObj.events;
            e.fileChange();
            e.imageClick();
            e.schoolChange();
            e.yearChange();
            e.nationalityChange();
            e.cropClick();
            e.registerSaveBtn();
        },
        fileChange: function () {
            $('#' + studentRegisterObj.config.imageFileId).on('change', function (e) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    $('#picModal').modal('show');
                    Imager.methods.initCropper(event.target.result);
                };
                
                var file = e.target.files[0];
                if (file) {
                    if (Imager.methods.isValid(file)) {
                        Imager.properties.photoName = file.name;
                        reader.readAsDataURL(e.target.files[0]);
                    } else {
                        app.alert.showErrorAutoHide('Image is not valid, please choose JPEG files');
                    }
                }
            });
        },
        schoolChange: function () {
            var schoolSelectId = studentRegisterObj.config.schoolSelectId;
            var schoolselect = document.getElementById(schoolSelectId).onchange = function (event) {
                studentRegisterObj.method.getClasses(event.target.value, studentRegisterObj.method.getYearValue());
            };
        },
        yearChange: function () {
            var yearSelectId = studentRegisterObj.config.yearSelectId;
            var yearSelect = document.getElementById(yearSelectId).onchange = function (event) {
                studentRegisterObj.method.getClasses(studentRegisterObj.method.getSchoolValue(),event.target.value);
            };
        },
        imageClick: function () {
            $('#picHolder').on('click', function () {
                $('#' + studentRegisterObj.config.imageFileId).trigger('click');
            });
        },
        nationalityChange: function () {
            $('#NationalityId').on('change', function (event) {
                var selectedOption = event.target.value;
                if (selectedOption == 1) {
                    $('#IdentityTypeId').val(0);
                } else {
                    $('#IdentityTypeId').val(1);
                }
            });
        },
        cropClick: function () {
            $('#btn-crop').on('click', function () {
                Imager.methods.crop();
                $('#picModal').modal('hide');
            });
        },
        registerSaveBtn: function () {
            $('#saveBtn').on('click', function () {
                studentRegisterObj.method.registerForm('studentRegisterForm');
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
};

$(function () {
    studentRegisterObj.events.register();
});