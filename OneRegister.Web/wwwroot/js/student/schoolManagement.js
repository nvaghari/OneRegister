var schoolManagementObj = {
    event: {
        schoolChange: function () {
            $('#school').on('change', function () {
                $('#schoolName').val($('#school option:selected').text());
                schoolManagementObj.method.getClassHomeRooms();
            });
        },
        yearChange: function () {
            $('#year').on('change', function () {
                schoolManagementObj.method.getClassHomeRooms();
            });
        },
        classChange: function () {
            $('#class').on('change', function () {
                var selectName = $('#class option:selected').text();
                $('#className').val(schoolManagementObj.method.extractClassName(selectName));
                $('#classLabel').val(schoolManagementObj.method.extractClassLabel(selectName));
            });
        },
        homeRoomChange: function () {
            $('#homeRoom').on('change', function () {
                $('#homeName').val($('#homeRoom option:selected').text());
            });
        },
        schoolEdit: function () {
            $('#schoolEdit').on('click', function () {
                $('#schoolSubmit').text('Update');
                $('#schoolName').val($('#school option:selected').text());
            });
        },
        schoolAdd: function () {
            $('#schoolAdd').on('click', function () {
                $('#schoolSubmit').text('Add');
                $('#schoolName').val('');
            });
        },
        schoolSubmit: function () {
            $('#schoolSubmit').on('click', function () {
                var action = $(this).text();
                if (action == 'Add') {
                    schoolManagementObj.method.addSchool($('#schoolName').val());
                } else {
                    schoolManagementObj.method.updateSchool($('#school').val(), $('#schoolName').val());
                }
            });
        },
        classEdit: function () {
            $('#classEdit').on('click', function () {
                var selectName = $('#class option:selected').text();
                $('#classSubmit').text('Update');
                $('#className').val(schoolManagementObj.method.extractClassName(selectName));
                $('#classLabel').val(schoolManagementObj.method.extractClassLabel(selectName));
            });
        },
        classAdd: function () {
            $('#classAdd').on('click', function () {
                $('#classSubmit').text('Add');
                $('#className').val('');
                $('#classLabel').val('');
            });
        },
        classSubmit: function () {
            $('#classSubmit').on('click', function () {
                var action = $(this).text();
                if (action == 'Add') {
                    var schoolId = $('#school').val();
                    var year = $('#year').val();
                    var className = $('#className').val();
                    var label = $('#classLabel').val();
                    schoolManagementObj.method.addClass(schoolId, year, className, label);
                } else {
                    var classId = $('#class').val();
                    var name = $('#className').val();
                    var label = $('#classLabel').val();
                    schoolManagementObj.method.updateClass(classId, name, label);
                }
            });
        },
        homeEdit: function () {
            $('#homeEdit').on('click', function () {
                $('#homeSubmit').text('Update');
                var name= 
                $('#homeName').val($('#homeRoom option:selected').text());
            });
        },
        homeAdd: function () {
            $('#homeAdd').on('click', function () {
                $('#homeSubmit').text('Add');
                $('#homeName').val('');
            });
        },
        homeSubmit: function () {
            $('#homeSubmit').on('click', function () {
                var action = $(this).text();
                if (action == 'Add') {
                    var schoolId = $('#school').val();
                    var year = $('#year').val();
                    var name = $('#homeName').val();
                    schoolManagementObj.method.addHomeRoom(schoolId, year, name);
                } else {
                    var homeId = $('#homeRoom').val();
                    var name = $('#homeName').val();
                    schoolManagementObj.method.updateHomeRoom(homeId, name);
                }
            });
        }
    },
    method: {
        getSchoolYears: function () {
            $.ajax({
                url: 'School/GetSchoolYears',
                method: 'POST',
                type: 'JSON',
                success: function (data) {
                    if (data.isSuccessful) {
                        schoolManagementObj.method.fillSchool(data.schools);
                        schoolManagementObj.method.fillYear(data.years);
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
        },
        fillSchool: function (data) {
            app.utils.refreshSelect('school', data);
        },
        fillYear: function (data) {
            app.utils.refreshSelect('year', data);
        },
        getClassHomeRooms: function () {
            var selectedYear = $('#year').val();
            var selectedSchool = $('#school').val();
            if (!selectedYear || !selectedSchool) return;
            $.ajax({
                url: 'School/GetClassHomeRooms',
                method: 'POST',
                type: 'JSON',
                data: { 'year': selectedYear, 'schoolId': selectedSchool },
                success: function (data) {
                    if (data.isSuccessful) {
                        schoolManagementObj.method.fillClass(data.classes);
                        schoolManagementObj.method.fillHomeRoom(data.homeRooms);
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
        },
        fillClass: function (data) {
            app.utils.refreshSelect('class', data);
        },
        fillHomeRoom: function (data) {
            app.utils.refreshSelect('homeRoom', data);
        },
        addSchool: function (name) {
            $.ajax({
                url: 'School/AddSchool',
                method: 'POST',
                type: 'JSON',
                data: { 'name': name },
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("School was added successfully. please login again to see new school");
                        schoolManagementObj.method.getSchoolYears();
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
        },
        updateSchool: function (schoolId, name) {
            $.ajax({
                url: 'School/UpdateSchool',
                method: 'POST',
                type: 'JSON',
                data: { 'schoolId': schoolId, 'name': name },
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("School was updated successfully");
                        schoolManagementObj.method.getSchoolYears();
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
        },
        extractClassName: function (name) {
            if (name.indexOf('[') > 0) {
                return name.substring(0, name.indexOf('[') - 1);
            } else {
                return name;
            }
        },
        extractClassLabel: function (name) {
            if (name.indexOf('[') > 0) {
                return name.substring(name.indexOf('[') + 1, name.indexOf(']'));
            } else {
                return '';
            }
        },
        addClass: function (schoolId,year,name,label) {
            $.ajax({
                url: 'School/AddClass',
                method: 'POST',
                type: 'JSON',
                data: { 'schoolId': schoolId, 'year': year, 'name': name, 'label': label },
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("Class was added successfully");
                        schoolManagementObj.method.getClassHomeRooms();
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
        },
        updateClass: function (classId, name, label) {
            $.ajax({
                url: 'School/UpdateClass',
                method: 'POST',
                type: 'JSON',
                data: { 'classId': classId, 'name': name, 'label': label },
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("Class was updated successfully");
                        schoolManagementObj.method.getClassHomeRooms();
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
        },
        addHomeRoom: function (schoolId, year, name) {
            $.ajax({
                url: 'School/AddHomeRoom',
                method: 'POST',
                type: 'JSON',
                data: { 'schoolId': schoolId, 'year': year, 'name': name },
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("HomeRoom was added successfully");
                        schoolManagementObj.method.getClassHomeRooms();
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
        },
        updateHomeRoom: function (homeId, name) {
            $.ajax({
                url: 'School/UpdateHomeRoom',
                method: 'POST',
                type: 'JSON',
                data: { 'homeId': homeId, 'name': name },
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("HomeRoom was updated successfully");
                        schoolManagementObj.method.getClassHomeRooms();
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
        }
    }
};


$(function () {
    schoolManagementObj.event.schoolChange();
    schoolManagementObj.event.yearChange();
    schoolManagementObj.event.classChange();
    schoolManagementObj.event.homeRoomChange();
    schoolManagementObj.event.schoolEdit();
    schoolManagementObj.event.schoolAdd();
    schoolManagementObj.event.schoolSubmit();
    schoolManagementObj.event.classEdit();
    schoolManagementObj.event.classAdd();
    schoolManagementObj.event.classSubmit();
    schoolManagementObj.event.homeEdit();
    schoolManagementObj.event.homeAdd();
    schoolManagementObj.event.homeSubmit();
    schoolManagementObj.method.getSchoolYears();
});