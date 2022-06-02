var studentImportObj = {
    config: {
        id: '#importPreviewGrid'
    },
    dataTable: {
        grid: {},
        data: {},
        init: function () {
            studentImportObj.dataTable.grid = $(studentImportObj.config.id).DataTable({
                data: studentImportObj.dataTable.data,
                order: [[17, "asc"]],
                columns: [
                    { data: "service" },
                    { data: "school" },
                    { data: "year" },
                    { data: "class" },
                    { data: "classLabel" },
                    { data: "homeRoom" },
                    { data: "studentNumber" },
                    { data: "name" },
                    { data: "gender" },
                    { data: "nationality" },
                    { data: "identityType" },
                    { data: "identityNumber" },
                    { data: "birthdayString" },
                    { data: "parentName" },
                    { data: "parentPhone" },
                    { data: "address" },
                    { data: "description" },
                    {
                        data: "isAcceptable",
                        render: function (data) {
                            if (data == true) return 'Yes'
                            else return 'No'
                        }
                    }
                ],
                createdRow: function (row, data, dataIndex) {
                    if (data.isAcceptable == false) {
                        $(row).addClass('import-row-wrong');
                    }
                }
            });
        }
    },
    event: {
        registerUploadBtn: function () {
            $('#btnUpload').on('click', function () {
                app.alert.hide();
                app.loading.show();
                studentImportObj.method.uploadFile();
            });
        },
        registerImportBtn: function () {
            $('#btnImport').on('click', function () {
                app.alert.hide();
                app.loading.show();
                studentImportObj.method.importFile();
            });
        }
    },
    method: {
        uploadFile: function () {
            if (!studentImportObj.method.isFileValid()) return;
            var fdata = new FormData();
            var inputFile = $('#fileInput').get(0);
            var files = inputFile.files;
            fdata.append("file", files[0]);
            var token = $('input:hidden[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: 'PreviewFile',
                method: 'POST',
                contentType: false,
                processData: false,
                data: fdata,
                headers: {
                    "RequestVerificationToken": token
                },
                success: function (data) {
                    app.loading.hide();
                    if (data.isSuccessful == true) {
                        studentImportObj.dataTable.data = data.students;
                        studentImportObj.dataTable.grid.destroy();
                        studentImportObj.dataTable.init();
                        studentImportObj.method.enableImportButton();

                    } else {
                        app.alert.showError(data.description);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
        },
        importFile: function () {
            if (!studentImportObj.method.isFileValid()) return;
            var fdata = new FormData();
            var inputFile = $('#fileInput').get(0);
            var files = inputFile.files;
            fdata.append("file", files[0]);
            var token = $('input:hidden[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: 'ImportFile',
                method: 'POST',
                contentType: false,
                processData: false,
                data: fdata,
                headers: {
                    "RequestVerificationToken": token
                },
                success: function (data) {

                    if (data.isSuccessful == true) {
                        app.alert.showSuccess(data.description);
                    } else {
                        app.alert.showError(data.description);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                }
            });
        },
        enableImportButton: function () {
            $('#btnImport').prop('disabled', false);
        },
        isFileValid: function () {
            if (!window.File || !window.FileReader || !window.FileList || !window.Blob) {
                app.alert.showError('The File APIs are not fully supported in this browser.');
                return false;
            }
            var input = document.getElementById('fileInput');
            if (!input) {
                app.alert.showError("Um, couldn't find the fileInput element.");
                return false;
            }
            if (!input.files) {
                app.alert.showError("This browser doesn't seem to support the `files` property of file inputs.");
                return false;
            }
            if (!input.files[0]) {
                app.alert.showError("Please select a file");
                return false;
            }
            var validExtensions = ['xls', 'xlsx', 'csv'];
            var file = input.files[0];
            var extension = file.name.replace(/^.*\./, '');
            if ($.inArray(extension, validExtensions) == -1) {
                app.alert.showError('Please select only this files: xls, xlsx and csv');
                return false;
            }
            return true;
        }
    }
};

$(function () {
    studentImportObj.dataTable.init();
    studentImportObj.event.registerUploadBtn();
    studentImportObj.event.registerImportBtn();
});