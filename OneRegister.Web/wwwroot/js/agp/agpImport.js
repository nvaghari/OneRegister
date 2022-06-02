if (typeof (agpImportObj) === "undefined") {
    var agpImportObj = {
        config: {
            id: '#importPreviewGrid'
        },
        dataTable: {
            grid: {},
            data: {},
            init: function () {
                agpImportObj.dataTable.grid = $(agpImportObj.config.id).DataTable({
                    data: agpImportObj.dataTable.data,
                    order: [[19, "asc"]],
                    columns: [
                        { data: "plotNo" },
                        { data: "firstName" },
                        { data: "lastName" },
                        { data: "identityType" },
                        { data: "identityNumber" },
                        { data: "company" },
                        { data: "ssmNo" },
                        { data: "mailingAddress" },
                        { data: "dateOfBirth" },
                        { data: "gender" },
                        { data: "nationality" },
                        { data: "mobileNo" },
                        { data: "emailAddress" },
                        { data: "designation" },
                        { data: "industry" },
                        { data: "natureOfBusiness" },
                        { data: "purposeOfTransaction" },
                        { data: "companyBankAccount" },
                        { data: "accountNo" },
                        { data: "entryDate" },
                        { data: "visaExpiry" },
                        { data: "plksExpiry" },
                        { data: "termOfService" },
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
            registerPreviewBtn: function () {
                $('#btnPreview').on('click', function () {
                    app.alert.hide();
                    app.loading.show();
                    agpImportObj.method.uploadToPreview();
                });
            },
            registerImportBtn: function () {
                $('#btnImport').on('click', function () {
                    app.alert.hide();
                    app.loading.show();
                    agpImportObj.method.importFile(false);
                });
            }
        },
        method: {
            uploadToPreview: function () {
                if (!agpImportObj.method.isFileValid()) return;
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
                            agpImportObj.dataTable.data = data.agps;
                            agpImportObj.dataTable.grid.destroy();
                            agpImportObj.dataTable.init();
                            agpImportObj.method.enableImportButton();
                        } else {
                            app.alert.showError(data.message);
                        }
                    },
                    error: function (err) {
                        app.alert.showAjaxErrorAutoHide(err);
                    }
                });
            },
            importFile: function (fileString) {
                if (!agpImportObj.method.isFileValid()) return;
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
                            app.alert.showSuccess(data.message);
                        } else {
                            app.alert.showError(data.message);
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
    }
}

$(function () {
    agpImportObj.dataTable.init();
    agpImportObj.event.registerPreviewBtn();
    agpImportObj.event.registerImportBtn();
});