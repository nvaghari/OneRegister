var studentEditObj = {
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
                url: '../GetClasses',
                type: 'POST',
                data: { "schoolid": schoolid, "year": year },
                success: function (data) {
                    app.utils.refreshSelect(studentEditObj.config.classSelectId, data.classes);
                    app.utils.refreshSelect(studentEditObj.config.homeRootSelectId, data.homeRooms);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        },
        getYearValue: function () {
            return document.getElementById(studentEditObj.config.yearSelectId).value;
        },
        getSchoolValue: function () {
            return document.getElementById(studentEditObj.config.schoolSelectId).value;
        },
        getStudentId: function () {
            return $('#Id').val();
        },
        confirmStudent: function () {
            $.ajax({
                url: '../ConfirmStudent',
                type: 'POST',
                data: { 'studentId': studentEditObj.method.getStudentId() },
                success: function (data) {
                    if (data.isSuccessful == true) {
                        app.alert.showInfo('The student was completed and imported into eDuit portal successfully');
                    } else {
                        app.alert.showError(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
            app.confirmModal.dismiss();
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
                url: 'Edit',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: studentEditObj.private.registerAjaxSuccess,
                error: studentEditObj.private.registerAjaxError
            });
        }
    },
    events: {
        register: function () {
            app.event.customFile();
            app.event.select2();
            app.event.tooltip();
            var e = studentEditObj.events;
            e.fileChange();
            e.imageClick();
            e.schoolChange();
            e.yearChange();
            e.nationalityChange();
            e.approveClick();
            e.cropClick();
            e.registerSaveBtn();
        },
        fileChange: function () {
            $('#' + studentEditObj.config.imageFileId).on('change', function (e) {
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
                        app.alert.showErrorAutoHide('Image is not valid');
                    }
                }
            });
        },
        schoolChange: function () {
            var schoolSelectId = studentEditObj.config.schoolSelectId;
            var schoolselect = document.getElementById(schoolSelectId).onchange = function (event) {
                studentEditObj.method.getClasses(event.target.value, studentEditObj.method.getYearValue());
            };
        },
        yearChange: function () {
            var yearSelectId = studentEditObj.config.yearSelectId;
            var yearSelect = document.getElementById(yearSelectId).onchange = function (event) {
                studentEditObj.method.getClasses(studentEditObj.method.getSchoolValue(), event.target.value);
            };
        },
        imageClick: function () {
            $('#picHolder').on('click', function () {
                $('#' + studentEditObj.config.imageFileId).trigger('click');
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
        approveClick: function () {
            $('#btnApprove').on('click', function () {
                app.confirmModal.confirmAction = studentEditObj.method.confirmStudent;
                app.confirmModal.setTitle("Approve Confirmation");
                app.confirmModal.setBody("Please be aware if you approve this student, you won't be able to edit it later in this portal");
                app.confirmModal.popUp();
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
                studentEditObj.method.registerForm('studentRegisterForm');
            });
        }
    },
    private: {
        registerAjaxSuccess: function (data) {
            if (data.isSuccessful) {
                window.location.href = "../List"
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
    studentEditObj.events.register();
});