var studentExportObj = {
    config: {
        id: '#exportPreviewGrid'
    },
    dataTable: {
        grid: {},
        data: {},
        init: function () {
            studentExportObj.dataTable.grid = $(studentExportObj.config.id).DataTable({
                data: studentExportObj.dataTable.data,
                order: [[1, "asc"]],
                language: {
                    zeroRecords: 'No matching record found, try to search by different parameters'
                },
                columns: [
                    {
                        data: "studentNumber",
                        searchable: true
                    },
                    { data: "name" },
                    { data: "school" },
                    { data: "gender" },
                    { data: "nationality" },
                    {
                        data: "hasPicture",
                        render: function (data, type, row, meta) {
                            if (data == 'Yes') {
                                return '<i class="fa fa-check text-success"></i>';
                            } else {
                                return '<i class="fa fa-times text-danger"></i>';
                            }

                        }
                    },
                    { data: "identityType" },
                    { data: "identityNumber" }
                ]
            });
        }
    },
    event: {
        schoolChange: function () {
            $('#school').on('change', function () {
                studentExportObj.method.getClassHomeRooms();
            });
        },
        yearChange: function () {
            $('#year').on('change', function () {
                studentExportObj.method.getClassHomeRooms();
            });
        },
        searchClick: function () {
            $('#searchBtn').on('click', function () {
                var params = studentExportObj.method.getParams();
                $.ajax({
                    url: 'ExportSearch',
                    type: 'POST',
                    data: params,
                    success: function (data) {
                        if (data.isSuccessful == true) {
                            studentExportObj.dataTable.data = data.students;
                            studentExportObj.dataTable.grid.destroy();
                            studentExportObj.dataTable.init();

                        } else {
                            app.alert.showError(data.description);
                        }
                    },
                    error: function (err) {
                        app.alert.showAjaxErrorAutoHide(err);
                    }
                });
            });
        }
    },
    method: {
        getClassHomeRooms: function () {
            var selectedYear = $('#year').val();
            var selectedSchool = $('#school').val();
            //if (!selectedYear || !selectedSchool) return;
            $.ajax({
                url: 'GetClassHomeRooms',
                method: 'POST',
                type: 'JSON',
                data: { 'year': selectedYear, 'schoolId': selectedSchool },
                success: function (data) {
                    if (data.isSuccessful) {
                        studentExportObj.method.fillClass(data.classes);
                        studentExportObj.method.fillHomeRoom(data.homeRooms);
                    } else {
                        app.alert.showErrorAutoHide(data.description);
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
            app.utils.refreshSelect('homeroom', data);
        },
        getParams: function () {
            return {
                school: $('#school').val(),
                year: $('#year').val(),
                class: $('#class').val(),
                homeroom: $('#homeroom').val()
            };
        }
    }
};

$(function () {
    studentExportObj.event.schoolChange();
    studentExportObj.event.yearChange();
    studentExportObj.dataTable.init();
    studentExportObj.event.searchClick();
});